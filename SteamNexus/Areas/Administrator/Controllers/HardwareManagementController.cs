using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Controllers;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.Services;
using System.ComponentModel.DataAnnotations;

namespace SteamNexus.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class HardwareManagementController : Controller
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;
        private readonly ILogger<HardwareManagementController> _logger;
        private readonly CoolPCWebScraping _coolPCWebScraping;
        // Constructor
        public HardwareManagementController(SteamNexusDbContext context, ILogger<HardwareManagementController> logger, CoolPCWebScraping coolPCWebScraping)
        {
            _context = context;
            _logger = logger;
            _coolPCWebScraping = coolPCWebScraping;
        }

        public IActionResult Index()
        {
            return View();
        }

        // 回傳硬體產品管理頁面 PartialView
        // GET: Administrator/HardwareManagement/ProductManagement
        [HttpGet]
        public IActionResult ProductManagement()
        {
            // 下拉式選單 => 硬體
            ViewBag.ComputerParts = new SelectList(_context.ComputerPartCategories.Select(c => c.Name));
            return PartialView("_ProductManagementPartial");
        }

        // 根據硬體規格回傳產品列表
        // GET: Administrator/HardwareManagement/HardwareData
        [HttpGet]
        public IActionResult HardwareData(int Type)
        {
            // 尋找特定硬體
            var result = _context.ComponentClassifications
                .Where(c => c.ComputerPartCategoryId == Type)
                .Join(_context.ProductInformations, c => c.ComponentClassificationId, p => p.ComponentClassificationId,
                (c, p) => new
                {
                    ProductId = p.ProductInformationId,
                    ComponentClassificationName = c.Name,
                    ProductName = p.Name,
                    p.Specification,
                    p.Price,
                    p.Wattage,
                    p.Recommend
                });
            return Json(result);
        }

        // 硬體產品 ViewModel
        public class HardwareProduct
        {
            [Required]
            [Range(10000, 99999)]
            public int ProductId { get; set; }

            [Required]
            [Range(0, 2000)]
            public int Wattage { get; set; }

            [Required]
            [Range(0, 4)]
            public int Recommend { get; set; }
        }

        // 產品資料 Update
        // POST: Administrator/HardwareManagement/ProductDataUpdate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDataUpdate([FromBody] HardwareProduct data)
        {
            // 如果驗證合法
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.ProductInformations.FindAsync(data.ProductId);
                    if (product != null)
                    {
                        product.Wattage = data.Wattage;
                        product.Recommend = data.Recommend;
                        await _context.SaveChangesAsync();
                        // 返回 200 狀態碼 ~ 變更成功
                        return Ok($"{data.ProductId}變更成功");
                    }
                    else
                    {
                        // 返回 404 狀態碼 ~ 找不到產品
                        return NotFound("找不到產品");
                    }
                }
                catch (Exception ex)
                {
                    // 返回 500 狀態碼 ~ 伺服器內部錯誤
                    return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
                }
            }
            else
            {
                // 返回 400 狀態碼 ~ 驗證不合法
                return BadRequest(ModelState);
            }
        }












        // 單一零件更新
        public class HardwareType
        {
            [Required]
            [Range(10000, 10010)]
            public int Type { get; set; }
        }

        // POST: Admin/UpdateHardwareOne
        [HttpPost]
        public string UpdateHardwareOne([FromBody] HardwareType data)
        {
            // 如果驗證合法
            if (ModelState.IsValid)
            {
                _coolPCWebScraping.UpdateAllComponentClassifications();
                switch (data.Type)
                {
                    case (int)ComputerPartCategory.Type.CPU:
                        _coolPCWebScraping.UpdateCPU();
                        return "CPU 更新成功";
                    case (int)ComputerPartCategory.Type.MB:
                        _coolPCWebScraping.UpdateMB();
                        return "MB 更新成功";
                    case (int)ComputerPartCategory.Type.RAM:
                        _coolPCWebScraping.UpdateRAM();
                        return "RAM 更新成功";
                    case (int)ComputerPartCategory.Type.SSD:
                        _coolPCWebScraping.UpdateSSD();
                        return "SSD 更新成功";
                    case (int)ComputerPartCategory.Type.HDD:
                        _coolPCWebScraping.UpdateHDD();
                        return "HDD 更新成功";
                    case (int)ComputerPartCategory.Type.AirCooler:
                        _coolPCWebScraping.UpdateAirCooler();
                        return "AirCooler 更新成功";
                    case (int)ComputerPartCategory.Type.LiquidCooler:
                        _coolPCWebScraping.UpdateLiquidCooler();
                        return "LiquidCooler 更新成功";
                    case (int)ComputerPartCategory.Type.GPU:
                        _coolPCWebScraping.UpdateGPU();
                        return "GPU 更新成功";
                    case (int)ComputerPartCategory.Type.CASE:
                        _coolPCWebScraping.UpdateCASE();
                        return "CASE 更新成功";
                    case (int)ComputerPartCategory.Type.PSU:
                        _coolPCWebScraping.UpdatePSU();
                        return "PSU 更新成功";
                    case (int)ComputerPartCategory.Type.OS:
                        _coolPCWebScraping.UpdateOS();
                        return "OS 更新成功";
                    default:
                        return "更新失敗";
                }
            }
            else
            {
                return "更新失敗";
            }
        }

        // 全零件更新
        // POST: Admin/UpdateHardwareAll
        [HttpPost]
        public string UpdateHardwareAll()
        {
            _coolPCWebScraping.UpdateAllComponentClassifications();
            Console.WriteLine("All OK");
            _coolPCWebScraping.UpdateCPU();
            Console.WriteLine("CPU OK");
            _coolPCWebScraping.UpdateGPU();
            Console.WriteLine("GPU OK");
            _coolPCWebScraping.UpdateRAM();
            Console.WriteLine("RAM OK");
            _coolPCWebScraping.UpdateMB();
            Console.WriteLine("MB OK");
            _coolPCWebScraping.UpdateSSD();
            Console.WriteLine("SSD OK");
            _coolPCWebScraping.UpdateHDD();
            Console.WriteLine("HDD OK");
            _coolPCWebScraping.UpdateAirCooler();
            Console.WriteLine("Air Cooler OK");
            _coolPCWebScraping.UpdateLiquidCooler();
            Console.WriteLine("LiquidCooler OK");
            _coolPCWebScraping.UpdateCASE();
            Console.WriteLine("CASE OK");
            _coolPCWebScraping.UpdatePSU();
            Console.WriteLine("PSU OK");
            _coolPCWebScraping.UpdateOS();
            Console.WriteLine("OS OK");


            return "全零件更新成功";
        }
    }
}
