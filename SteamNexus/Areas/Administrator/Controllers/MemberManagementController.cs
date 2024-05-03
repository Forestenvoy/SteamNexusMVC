using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.ViewModels.Game;
using static System.Net.Mime.MediaTypeNames;

namespace SteamNexus.Areas.Administrator.Controllers
{
    [Area("Administrator")]

    public class MemberManagementController : Controller
    {
        private SteamNexusDbContext _application;
        private readonly IWebHostEnvironment _webHost;  //上傳圖片使用

        public MemberManagementController(SteamNexusDbContext application, IWebHostEnvironment webHostEnvironment)
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
                birthday = result.Birthday.ToString(),
                result.Phone,
                result.Photo,

            });
            return Json(result);

        }
    }
}
