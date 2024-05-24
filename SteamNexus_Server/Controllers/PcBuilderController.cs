using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using SteamNexus_Server.Dtos;
using Microsoft.EntityFrameworkCore;


namespace SteamNexus_Server.Controllers;

// 套用 CORS 策略
[EnableCors("AllowAny")]
[Route("api/[controller]")]
[ApiController]
public class PcBuilderController : ControllerBase
{
    // Dependency Injection
    private readonly SteamNexusDbContext _context;


    // Constructor
    public PcBuilderController(SteamNexusDbContext context)
    {
        _context = context;
    }

    // 回傳上架菜單資料
    // GET: api/PcBuilder/GetMenuList
    [HttpGet("GetMenuLists")]
    public IEnumerable<MenuItemDto> GetMenuLists()
    {
        var menuLists = _context.Menus
            .Where(ml => ml.Active == true)
            .OrderBy(ml => ml.TotalPrice)
            .AsNoTracking() // 防止 EF Core 追蹤查詢結果
            .Select(m => new MenuItemDto
            {
                Id = m.MenuId,
                Name = m.Name,
                TotalPrice = m.TotalPrice,
                DetailCount = m.MenuDetails.Count()
            });

        return menuLists;
    }

    // 回傳產品資料
    // GET: api/PcBuilder
    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetProductData(int type)
    {
        try
        {
            // 尋找特定硬體的產品資料
            var result = _context.ComponentClassifications
                .Where(c => c.ComputerPartCategoryId == type)
                .Join(_context.ProductInformations, c => c.ComponentClassificationId, p => p.ComponentClassificationId,
                (c, p) => new ProductDto
                {
                    Id = p.ProductInformationId,
                    Classification = c.Name,
                    Name = p.Name,
                    Specification = p.Specification,
                    Price = p.Price,
                    Wattage = p.Wattage,
                    Recommend = p.Recommend
                });

            if (result == null || !result.Any())
            {
                return NotFound("Data not found.");
            }

            return Ok(result);
        }
        catch (Exception error)
        {
            // 未來考慮引用日誌框架如 Serilog 記錄異常 
            Console.WriteLine(error.Message);

            // 未來考慮引用中介軟體 RequestDelegate 做 例外處理
            return StatusCode(500, "An internal server error occurred. Please try again later.");
        }

    }

    // 回傳產品資料 RAM
    // GET: api/PcBuilder
    [HttpGet("GetRAMData")]
    public ActionResult<IEnumerable<ProductDto>> GetRAMData()
    {
        try
        {
            // 尋找特定硬體的產品資料
            var result = _context.ComponentClassifications
                .Where(c => c.ComputerPartCategoryId == 10002)
                .Join(
                    _context.ProductInformations,
                    c => c.ComponentClassificationId,
                    p => p.ComponentClassificationId,
                    (c, p) => new { c, p }
                )
                .Join(
                    _context.ProductRAMs,
                    cp => cp.p.ProductInformationId,
                    r => r.ProductInformationId,
                    (cp, r) => new RamDto
                    {
                        Id = cp.p.ProductInformationId,
                        Classification = cp.c.Name,
                        Name = cp.p.Name,
                        Specification = cp.p.Specification,
                        Price = cp.p.Price,
                        Wattage = cp.p.Wattage,
                        Recommend = cp.p.Recommend,
                        Size = r.Size  // 從 ProductRAM 中獲取 Size
                    }
                );

            if (result == null || !result.Any())
            {
                return NotFound("Data not found.");
            }

            return Ok(result);
        }
        catch (Exception error)
        {
            // 未來考慮引用日誌框架如 Serilog 記錄異常 
            Console.WriteLine(error.Message);

            // 未來考慮引用中介軟體 RequestDelegate 做 例外處理
            return StatusCode(500, "An internal server error occurred. Please try again later.");
        }

    }

}
