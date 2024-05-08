using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.Services;
using System;
using System.Reflection.Metadata;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Url,Discription,")] Advertisement advertisement, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                Advertisement Ad = new Advertisement
                {
                    Title = advertisement.Title,
                    Url = advertisement.Url,
                    Discription = advertisement.Discription,
                };
                if (imageFile != null && imageFile.Length > 0)
                {
                    // 構建文件路徑
                    var filePath = Path.Combine(_environment.WebRootPath, "AdImages", imageFile.FileName);

                    // 寫入圖片檔到指定路徑
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // 更新廣告的圖片路徑
                    Ad.ImagePath = $"{imageFile.FileName}";
                }

                try
                {
                    _context.Advertisements.Add(Ad);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("錯誤");
                }
            }
            return View(advertisement);
        }




        // GET: AdvertiseManagementController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdvertiseManagementController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdvertiseManagementController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdvertiseManagementController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AdvertiseManagementController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AdvertiseManagementController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AdvertiseManagementController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
