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


    }
}
