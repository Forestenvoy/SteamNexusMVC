using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus_Server.Data;
using SteamNexus_Server.Migrations;
using SteamNexus_Server.Models;
using System.ComponentModel.DataAnnotations;
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


        #region TrackingDataForId 移除最後一筆資料會出現錯誤
        //[HttpGet("GameTrack")]
        //[Authorize]
        //public async Task<IActionResult> GetGameTracking()
        //{
        //    // 取得使用者 ID
        //    var userId = GetUserIdFromToken();
        //    if (userId == null)
        //    {
        //        return Unauthorized("無效的使用者憑證或使用者 ID");
        //    }

        //    // 根據使用者 ID 獲取追蹤數據
        //    var result = await _application.GameTrackings
        //        .Where(r => r.UserId == userId)
        //        .Join(_application.Users,
        //              r => r.UserId,
        //              u => u.UserId,
        //              (r, u) => new { r, u })
        //        .Join(_application.Games,
        //              a => a.r.GameId,
        //              g => g.GameId,
        //              (a, g) => new
        //              {
        //                  a.r.GameTrackingId,
        //                  a.r.UserId,
        //                  UserName = a.u.Name,
        //                  g.GameId,
        //                  g.Name,
        //                  g.OriginalPrice,
        //                  g.CurrentPrice,
        //                  g.LowestPrice,
        //                  g.AgeRating,
        //                  g.Comment,
        //                  g.CommentNum,
        //                  g.ReleaseDate,
        //                  g.Publisher,
        //                  g.Description,
        //                  g.Players,
        //                  g.PeakPlayers,
        //                  g.ImagePath,
        //                  g.VideoPath,
        //                  a.r.TrackingDate
        //              })
        //        .ToListAsync();

        //    if (result == null || !result.Any())
        //    {
        //        return NotFound("未找到追蹤資料");
        //    }

        //    return Ok(result);
        //}
        #endregion


        #region TrackGameDataForId
        [HttpGet("GameTrackForId")]
        [Authorize]
        public async Task<IActionResult> GetGameTracking()
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized("無效的使用者憑證或使用者 ID");
            }

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

            // 如果沒有找到任何數據，返回空的結果而不是 404
            if (result == null || !result.Any())
            {
                return Ok(new List<object>());
            }

            return Ok(result);
        }
        #endregion


        #region GameTracking ViewModel
        public class GameTrackingViewModel
        {
            [Required]
            public int GameId { get; set; }
        }
        #endregion


        #region AddGameTracking
        [HttpPost("AddGameTracking")]
        public async Task<IActionResult> AddGameTracking([FromBody] GameTrackingViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 取得使用者 ID
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized("無效的使用者憑證或使用者 ID");
            }

            // 檢查是否已存在相同的追蹤記錄
            var existingTracking = await _application.GameTrackings
                .FirstOrDefaultAsync(gt => gt.GameId == data.GameId && gt.UserId == userId);

            if (existingTracking != null)
            {
                return Conflict("該遊戲已經在追蹤列表中");
            }

            // 創建新的遊戲追蹤記錄
            var gameTracking = new SteamNexus_Server.Models.GameTracking 
            {
                GameId = data.GameId,
                UserId = userId.Value,
                TrackingDate = DateTime.UtcNow
            };

            _application.GameTrackings.Add(gameTracking);
            await _application.SaveChangesAsync();

            //return CreatedAtAction(nameof(GetGameTracking), new { id = gameTracking.GameTrackingId }, gameTracking);
            
            
            // 記錄成功訊息
            _logger.LogInformation($"User {userId.Value} successfully added tracking for Game {data.GameId}");

            return CreatedAtAction(nameof(GetGameTracking), new { id = gameTracking.GameTrackingId }, new
            {
                Message = "新增遊戲追蹤成功",
                GameTracking = gameTracking
            });

        }
        #endregion


        #region DeleteGameTracking
        [HttpDelete("DeleteGameTracking/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteGameTracking(int id)
        {
            // 取得使用者 ID
            var userId = GetUserIdFromToken();
            if (userId == null)
            {
                return Unauthorized("無效的使用者憑證或使用者 ID");
            }

            // 查找追蹤記錄
            var gameTracking = await _application.GameTrackings
                .FirstOrDefaultAsync(gt => gt.GameTrackingId == id && gt.UserId == userId);

            if (gameTracking == null)
            {
                return NotFound("未找到對應的遊戲追蹤記錄");
            }

            // 刪除追蹤記錄
            _application.GameTrackings.Remove(gameTracking);
            await _application.SaveChangesAsync();

            return NoContent(); // 返回 204 No Content 表示成功刪除
        }
        #endregion

    }
}
