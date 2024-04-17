using Microsoft.AspNetCore.Mvc;

namespace SteamNexus.Areas.Administrator.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Administrator")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
