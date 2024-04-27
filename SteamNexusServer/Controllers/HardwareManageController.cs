using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SteamNexusServer.Data;
using SteamNexusServer.Models;


namespace SteamNexusServer.Controllers
{
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
        [HttpGet(Name = "GetComputerParts")]
        public IEnumerable<string> GetComputerParts()
        {
            // 下拉式選單 => 硬體
            var ComputerParts = _context.ComputerPartCategories.Select(c => c.Name);
            return ComputerParts;
        }

    }
}
