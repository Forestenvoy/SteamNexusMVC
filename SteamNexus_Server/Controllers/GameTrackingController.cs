using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus_Server.Data;
using SteamNexus_Server.Migrations;
using SteamNexus_Server.Models;

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

        #region TrackingData
        [HttpGet("GameTracking")]
        public async Task<IActionResult> GameTracking()
        {
            var result = await _application.GameTrackings
                .Join(_application.Users,
                      r => r.UserId,
                      u => u.UserId,
                      (r, u) => new { r, u })
                .Join(_application.Games,
                      a => a.r.GameId,
                      g => g.GameId,
                      (a, g) => new
                      {
                          a.r.GameTrackingId,
                          a.r.UserId,
                          UserName = a.u.Name,
                          g.GameId,
                          g.Name,
                          g.OriginalPrice,
                          g.CurrentPrice,
                          g.LowestPrice,
                          g.AgeRating,
                          g.Comment,
                          g.CommentNum,
                          g.ReleaseDate,
                          g.Publisher,
                          g.Description,
                          g.Players,
                          g.PeakPlayers,
                          g.ImagePath,
                          g.VideoPath,
                          a.r.TrackingDate
                      })
                .ToListAsync();

            if (result == null || !result.Any())
            {
                return NotFound("No tracking data found.");
            }

            return Ok(result);
        }
        #endregion



    }
}
