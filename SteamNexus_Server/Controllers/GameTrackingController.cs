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
    public class GameTrackingController : ControllerBase
    {
        private SteamNexusDbContext _application;

        public GameTrackingController(SteamNexusDbContext application)
        {
            _application = application;
        }
    }
}
