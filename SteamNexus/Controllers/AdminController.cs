using Microsoft.AspNetCore.Mvc;

namespace SteamNexus.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult HardwareManagement()
        {
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
