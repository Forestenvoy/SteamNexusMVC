using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace SteamNexus.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class AdvertiseManagementController : Controller
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;
        private readonly IWebHostEnvironment _environment;
        // Constructor
        public AdvertiseManagementController(SteamNexusDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public JsonResult AdvertiseManagementJson()
        {
            return Json(_context.Advertisements);
        }

        [HttpGet]
        public IActionResult AdvertiseManagement()
        {
            return PartialView("_AdvertiseManagementPartial");
        }

        [HttpPut]
        public IActionResult UpdateIsShow(int adId, bool isShow)
        {
            var advertisement = _context.Advertisements.Find(adId);
            if (advertisement == null)
            {
                return NotFound("Advertisement not found.");
            }

            advertisement.IsShow = isShow;
            _context.SaveChanges();

            return Ok("Updated advertisement status.");
        }

        public class CreateAdViewModel
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string Url { get; set; }
            public string? Discription { get; set; }
            [Required]
            public IFormFile ImageFile { get; set; }
        }


        [HttpPost]
        public async Task<IActionResult> CreateAd([FromForm] CreateAdViewModel data)
        {
            if (ModelState.IsValid)
            {
                Advertisement Ad = new Advertisement
                {
                    Title = data.Title,
                    Url = data.Url,
                    Discription = data.Discription,
                };
                if (data.ImageFile != null && data.ImageFile.Length > 0)
                {
                    string fileName = data.Title + Path.GetExtension(data.ImageFile.FileName);
                    // 構建文件路徑
                    var filePath = Path.Combine(_environment.WebRootPath, "AdImages", fileName);

                    // 寫入圖片檔到指定路徑
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await data.ImageFile.CopyToAsync(stream);
                    }

                    // 更新廣告的圖片路徑
                    Ad.ImagePath = $"{fileName}";
                }

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

        public class Parameter
        {
            public int id { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAd([FromBody]Parameter p)
        {
            var Ad = await _context.Advertisements.FindAsync(p.id);
            if (Ad == null)
            {
                return BadRequest("刪除失敗");
            }

            // 刪除廣告圖片
            if (!string.IsNullOrEmpty(Ad.ImagePath))
            {
                var filePath = Path.Combine(_environment.WebRootPath, "AdImages", Ad.ImagePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Advertisements.Remove(Ad);
            await _context.SaveChangesAsync();
            return Ok("刪除成功");
        }

        public class EditAdViewModel
        {
            [Required]
            public int Id { get; set; }
            [Required]
            public string Title { get; set; }
            public string? Discription { get; set; }
            [Required]
            public string Url { get; set; }
            public IFormFile? ImageFile { get; set; }

        }
        [HttpPost]
        public async Task<IActionResult> EditAd([FromForm]EditAdViewModel data)
        {
            if (ModelState.IsValid)
            {
                var editAd = await _context.Advertisements.FindAsync(data.Id);
                if(editAd == null)
                {
                    return BadRequest("修改失敗");
                }
                editAd.AdvertisementId = data.Id;
                editAd.Title = data.Title;
                editAd.Url = data.Url;
                editAd.Discription = data.Discription;

                if (data.ImageFile != null && data.ImageFile.Length > 0)
                {
                    //刪除舊照片
                    var oldFilePath = Path.Combine(_environment.WebRootPath, "AdImages", editAd.ImagePath);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    //建立新照片
                    string fileName = data.Id + Path.GetExtension(data.ImageFile.FileName);
                    // 構建文件路徑
                    var filePath = Path.Combine(_environment.WebRootPath, "AdImages", fileName);

                    // 寫入圖片檔到指定路徑
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await data.ImageFile.CopyToAsync(stream);
                    }

                    // 更新廣告的圖片路徑
                    editAd.ImagePath = $"{fileName}";
                }

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


    }
}
