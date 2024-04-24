using Microsoft.AspNetCore.Mvc;
using SteamNexus.Data;
using static System.Net.Mime.MediaTypeNames;

namespace SteamNexus.Areas.Administrator.Controllers
{
    [Area("Administrator")]

    public class MemberManagementController : Controller
    {
        private ApplicationDbContext _application;
        private readonly IWebHostEnvironment _webHost;  //上傳圖片使用

        public MemberManagementController(ApplicationDbContext application, IWebHostEnvironment webHostEnvironment)
        {
            _application = application;
            _webHost = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MemberManagement()
        {
            return PartialView("_MemberManagementPartial");
        }

        //將資料轉換成JSON檔
        public IActionResult MemberData()
        {
            var result = _application.Users.Select(result => new
            {
                result.Name,
                result.Email,
               Gender = result.Gender ? "男" : "女",
                birthday = result.Birthday.ToShortDateString(),
                result.CPUId,
                result.GPUId,
                result.RAM,
                result.Images,
                result.Power,

            });
            return Json(result);

        }
    }
}
