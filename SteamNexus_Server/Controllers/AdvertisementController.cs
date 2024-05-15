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
        public async Task<IActionResult> UpdateIsShow(int adId, bool isShow)
        {
            var advertisement = await _context.Advertisements.FindAsync(adId);
            if (advertisement == null)
            {
                return NotFound("Advertisement not found.");
            }

            advertisement.IsShow = isShow;
            await _context.SaveChangesAsync();

            if (advertisement.IsShow) {
                return Ok($"{advertisement.AdvertisementId} 上架成功!");
            }
            else
            {
                return Ok($"{advertisement.AdvertisementId} 下架成功!");
            }
        }

        [HttpGet("GetOneAdData")]
        public async Task<object> GetOneAdData(int AdId)
        {
            var advertisement = await _context.Advertisements.FindAsync(AdId);
            if (advertisement == null)
            {
                return NotFound("Advertisement not found.");
            }

            var data = new { Id=advertisement.AdvertisementId, Title=advertisement.Title, Url=advertisement.Url, Image=advertisement.ImagePath, Discription=advertisement.Discription };
            // 回傳一個ID Name 的物件
            return data;
        }

        public class Parameter
        {
            public int id { get; set; }
        }

        [HttpPost("DeleteAd")]
        public async Task<IActionResult> DeleteAd([FromBody] Parameter p)
        {
            var Ad = await _context.Advertisements.FindAsync(p.id);
            if (Ad == null)
            {
                return BadRequest("刪除失敗");
            }

            // 刪除廣告圖片
            //if (!string.IsNullOrEmpty(Ad.ImagePath))
            //{
            //    var filePath = Path.Combine(_environment.WebRootPath, "AdImages", Ad.ImagePath);
            //    if (System.IO.File.Exists(filePath))
            //    {
            //        System.IO.File.Delete(filePath);
            //    }
            //}

            _context.Advertisements.Remove(Ad);
            await _context.SaveChangesAsync();
            return Ok("刪除成功");
        }
    }
}
