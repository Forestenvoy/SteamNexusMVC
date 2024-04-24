using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Controllers;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.Services;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            [Range(0, 2000, ErrorMessage = "瓦數範圍介於 0 ~ 2000 之間")]
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
                // 加入 資料庫 交易機制
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // 根據ID查詢要更新的產品 
                        var product = await _context.ProductInformations.FindAsync(data.ProductId);
                        if (product != null)
                        {
                            // 更新產品資料
                            product.Wattage = data.Wattage;
                            product.Recommend = data.Recommend;

                            // 設置 EntityState 為 Modified
                            _context.Entry(product).State = EntityState.Modified;

                            // 儲存變更
                            await _context.SaveChangesAsync();

                            // 因為欄位有"時間戳記"屬性 ==> 樂觀併發控制
                            // 1.讀取：交易將資料讀入，這時系統會給交易分派一個時間戳。
                            // 2.校驗：交易執行完畢後，進行提交。這時同步校驗所有交易，如果交易所讀取的資料在讀取之後又被其他交易修改，則產生衝突，交易被中斷(回復)。
                            // 3.寫入：通過校驗階段後，將更新的資料寫入資料庫。

                            // 交易完成 ~ 提交成功，資料庫才會真的異動 !!!
                            await transaction.CommitAsync();

                            // 返回 200 狀態碼 ~ 變更成功
                            return Ok($"{data.ProductId} 資料變更成功");
                        }
                        else
                        {
                            // 返回 404 狀態碼 ~ 找不到產品
                            return NotFound("資料庫找不到此產品");
                        }
                    }
                    // 解決並行存取衝突
                    catch (DbUpdateConcurrencyException)
                    {
                        // 回滾交易
                        await transaction.RollbackAsync();
                        // 返回 409 狀態碼 ~ 同時修改衝突
                        return StatusCode(409, "同時修改衝突，請重新整理後再試");
                    }
                    catch (Exception ex)
                    {
                        // 返回 500 狀態碼 ~ 伺服器內部錯誤
                        return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
                    }
                }
            }
            else
            {
                // 返回 400 狀態碼 ~ 驗證不合法，同時回傳 ErrorMessage
                return BadRequest("瓦數範圍介於 0 ~ 2000 之間");
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
