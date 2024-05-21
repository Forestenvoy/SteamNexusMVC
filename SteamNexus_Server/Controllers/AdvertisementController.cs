using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            var data = new { Id=advertisement.AdvertisementId, Title=advertisement.Title, Url=advertisement.Url, Image=advertisement.ImagePath, Description=advertisement.Description };
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

        public class AdData
        {
            [Required]
            [MaxLength(100)]
            public string Title { get; set; }
            [Required]
            public string Url { get; set; }
            [Required]
            public string ImagePath { get; set; }
            [MaxLength(200)]
            public string? Description { get; set; }
        }

        [HttpPost("CreateAd")]
        public async Task<IActionResult> CreateAd([FromBody] AdData adData)
        {
            if (ModelState.IsValid)
            {
                Advertisement Ad = new Advertisement
                {
                    Title = adData.Title,
                    Url = adData.Url,
                    ImagePath = adData.ImagePath,
                    Description = adData.Description,
                };
                //if (data.ImageFile != null && data.ImageFile.Length > 0)
                //{
                //    string fileName = data.Title + Path.GetExtension(data.ImageFile.FileName);
                //    // 構建文件路徑
                //    var filePath = Path.Combine(_environment.WebRootPath, "AdImages", fileName);

                //    // 寫入圖片檔到指定路徑
                //    using (var stream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await data.ImageFile.CopyToAsync(stream);
                //    }

                //    // 更新廣告的圖片路徑
                //    Ad.ImagePath = $"{fileName}";
                //}

                try
                {
                    _context.Advertisements.Add(Ad);
                    await _context.SaveChangesAsync();

                    //// 在保存廣告成功後取得廣告的 ID
                    //int adId = Ad.AdvertisementId;

                    //// 使用廣告的 ID 來命名圖片檔案
                    //string fileName = $"{adId}{Path.GetExtension(data.ImageFile.FileName)}";
                    //var filePath = Path.Combine(_environment.WebRootPath, "AdImages", fileName);

                    //// 寫入圖片檔到指定路徑
                    //using (var stream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await data.ImageFile.CopyToAsync(stream);
                    //}

                    //// 更新廣告的圖片路徑
                    //Ad.ImagePath = fileName;
                    //await _context.SaveChangesAsync();

                    return Ok("廣告新增成功!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("廣告新增失敗!");
                }
            }
            return BadRequest("廣告新增失敗!");
        }

        public class EditAdViewModel
        {
            [Required]
            public int Id { get; set; }
            [Required]
            [MaxLength(100)]
            public string Title { get; set; }
            [MaxLength(200)]
            public string? Description { get; set; }
            [Required]
            public string Url { get; set; }
            [Required]
            public string ImagePath { get; set; }
        }

        [HttpPost("EditAd")]
        public async Task<IActionResult> EditAd([FromBody] EditAdViewModel data)
        {
            if (ModelState.IsValid)
            {
                var editAd = await _context.Advertisements.FindAsync(data.Id);
                if (editAd == null)
                {
                    return BadRequest("修改失敗");
                }
                editAd.AdvertisementId = data.Id;
                editAd.Title = data.Title;
                editAd.Url = data.Url;
                editAd.ImagePath = data.ImagePath;
                editAd.Description = data.Description;

                //if (data.ImageFile != null && data.ImageFile.Length > 0)
                //{
                //    //刪除舊照片
                //    var oldFilePath = Path.Combine(_environment.WebRootPath, "AdImages", editAd.ImagePath);
                //    if (System.IO.File.Exists(oldFilePath))
                //    {
                //        System.IO.File.Delete(oldFilePath);
                //    }

                //    //建立新照片
                //    string fileName = data.Id + Path.GetExtension(data.ImageFile.FileName);
                //    // 構建文件路徑
                //    var filePath = Path.Combine(_environment.WebRootPath, "AdImages", fileName);

                //    // 寫入圖片檔到指定路徑
                //    using (var stream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await data.ImageFile.CopyToAsync(stream);
                //    }

                //    // 更新廣告的圖片路徑
                //    editAd.ImagePath = $"{fileName}";
                //}

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok("廣告修改成功!");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("廣告修改失敗!");
                }
            }
            return BadRequest("廣告修改失敗!");
        }

        [HttpGet("GetAdSlides")]
        public async Task<ActionResult<IEnumerable<Advertisement>>> GetAdSlides()
        {
            var advertisements = await _context.Advertisements.Where(ad => ad.IsShow).ToListAsync();

            return advertisements;
        }

    }
}
