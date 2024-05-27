using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus_Server.Data;
using SteamNexus_Server.Migrations;
using SteamNexus_Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SteamNexus_Server.Controllers
{
    // 套用 CORS 策略
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class GameTrackingController : ControllerBase
    {
        private SteamNexusDbContext _application;
        private readonly ILogger<MemberManagementController> _logger;

        public GameTrackingController(SteamNexusDbContext application, ILogger<MemberManagementController> logger)
        {
            _application = application;
            _logger = logger;
        }


        #region GetUserIdFromToken
        private int? GetUserIdFromToken()
        {
            //從Header取得Authorization的數值，並轉換字串
            var authHeader = Request.Headers["Authorization"].ToString();

            //檢查欄位是否有Bearer開頭的欄位
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                //提取token
                var token = authHeader.Substring("Bearer ".Length).Trim();

                //解析token
                var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                {
                    return null;
                }

                //取得ID
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    _logger.LogInformation($"UserId: {userIdClaim.Value}");
                    return userId;
                }
            }
            return null;
        }
        #endregion


        #region TrackingData
        [HttpGet("GameTracking")]
        public async Task<IActionResult> GetTracking()
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


        #region TrackingDataForId
        [HttpGet("GameTrackForId")]
        public async Task<IActionResult> GetGameTracking()
        {
            // 取得使用者 ID
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized("無效的使用者憑證或使用者 ID");
            }

            // 根據使用者 ID 獲取追蹤數據
            var result = await _application.GameTrackings
                .Where(r => r.UserId == userId)
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
                return NotFound("未找到追蹤資料");
            }

            return Ok(result);
        }
        #endregion


        #region 取消追蹤
        [HttpPost("Untrack")]
        public async Task<IActionResult> Untrack([FromBody] int GameTrackingId)
        {
            var tracking = await _application.GameTrackings.FindAsync(GameTrackingId);
            if (tracking == null)
            {
                return NotFound("未找到追蹤");
            }

            _application.GameTrackings.Remove(tracking);
            await _application.SaveChangesAsync();

            return Ok("成功移除追蹤");
        }
        #endregion

    }
}
