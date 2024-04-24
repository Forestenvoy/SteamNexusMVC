using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using NuGet.Protocol;

namespace SteamNexus.Areas.Administrator.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Administrator")]
        [OutputCache(NoStore = true, Duration = 0)]
        public IActionResult Index()
        {

            return View();
        }
    }
}
