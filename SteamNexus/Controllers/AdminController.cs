using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;

namespace SteamNexus.Controllers
{
    public class AdminController : Controller
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;

        // Constructor
        public AdminController(SteamNexusDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HardwareManagement()
        {
            ViewBag.CustomerCourtries = new SelectList(_context.ComputerParts.Select(c=>c.Name));
            // 
            return PartialView("_HardwareManagementPartial");
        }

        [HttpGet]
        public IActionResult MemberManagement()
        {
            // 
            return PartialView("_MemberManagementPartial");
        }
        
        [HttpGet]
        public IActionResult GameManagement()
        {
            // 
            return PartialView("_GaenManagementPartial");
        }
    }
}
