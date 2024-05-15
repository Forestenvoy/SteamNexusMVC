using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus_Server.Data;

namespace SteamNexus_Server.Controllers
{
    // 套用 CORS 策略
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;
        public AdvertisementController(SteamNexusDbContext context)
        {
            _context = context;
        }

        [HttpGet("AdvertiseData")]
        public IEnumerable<object> AdvertiseData()
        {
            return _context.Advertisements;
        }

        [HttpPut("UpdateIsShow")]
        public IActionResult UpdateIsShow(int adId, bool isShow)
        {
            var advertisement = _context.Advertisements.Find(adId);
            if (advertisement == null)
            {
                return NotFound("Advertisement not found.");
            }

            advertisement.IsShow = isShow;
            _context.SaveChanges();

            return Ok("狀態更新成功!");
        }
    }
}
