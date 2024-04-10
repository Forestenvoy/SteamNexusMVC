using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;

namespace SteamNexus.Controllers
{
    public class AdminController : Controller
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;

        // Constructor
        public AdminController(SteamNexusDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HardwareManagement()
        {
            // 下拉式選單 => 硬體
            ViewBag.ComputerParts = new SelectList(_context.ComputerPartCategories.Select(c=>c.Name));
            return PartialView("_HardwareManagementPartial");
        }

        // GET: Admin/HardwareData
        [HttpGet]
        public IActionResult HardwareData(int Type)
        {
            // 尋找特定硬體
            var result = _context.ComponentClassifications
                .Where(c=>c.ComputerPartCategoryId == Type)
                .Join(_context.ProductInformations, c=>c.ComponentClassificationId, p=>p.ComponentClassificationId, 
                (c,p)=> new
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

        // 推薦變更呼叫

        public class RecommondData
        {
            public int ProductId { get; set; }
            public int recommend { get; set; }
        }

        // POST: Admin/recommendChange
        [HttpPost]
        public string recommendChange([FromBody]RecommondData data)
        {
            var product = _context.ProductInformations
                .Where(p => p.ProductInformationId == data.ProductId)
                .FirstOrDefault();
            if(product != null)
            {
                product.Recommend = data.recommend;
                _context.SaveChanges();
                return "變更成功";
            }
            else
            {
                return "找不到產品";
            }
        }

        [HttpGet]
        public IActionResult MemberManagement()
        {
            // 
            return PartialView("_MemberManagementPartial");
        }
        
        [HttpGet]
        public IActionResult GameManagement()
        {
            // 
            return PartialView("_GaenManagementPartial");
        }
    }
}
