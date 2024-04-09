using Microsoft.AspNetCore.Mvc;
using SteamNexus.Data;
using SteamNexus.Services;

namespace SteamNexus.Controllers
{
    public class ADController : Controller
    {
        private readonly SteamNexusDbContext _context;

        public ADController(SteamNexusDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
