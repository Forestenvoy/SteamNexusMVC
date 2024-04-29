using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;


namespace SteamNexus_Server.Controllers
{

    // 套用 CORS 策略
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class HardwareManageController : ControllerBase
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;

        // Constructor
        public HardwareManageController(SteamNexusDbContext context)
        {
            _context = context;
        }

        // 回傳硬體產品種類
        // GET: api/HardwareManage/GetComputerParts
        [HttpGet("GetComputerParts")]
        public IEnumerable<string> GetComputerParts()
        {
            // 下拉式選單 => 硬體
            var ComputerParts = _context.ComputerPartCategories.Select(c => c.Name);
            return ComputerParts;
        }




    }
}
