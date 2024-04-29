using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus.Data;
using SteamNexus.Services;

namespace SteamNexus.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class AdvertiseManagementController : Controller
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;
        // Constructor
        public AdvertiseManagementController(SteamNexusDbContext context)
        {
            _context = context;
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
