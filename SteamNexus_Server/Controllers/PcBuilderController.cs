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

}
