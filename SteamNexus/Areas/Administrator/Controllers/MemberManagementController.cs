using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.ViewModels.Game;
using System.ComponentModel.DataAnnotations;
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

        #region JsonUser資料
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
        #endregion


        #region JsonRolesName
        public JsonResult RolesData()
        {
            var result = _application.Roles.Select(result => new
            {
                result.RoleName,
            });
            return Json(result);

        }
        #endregion


        #region Json結合User and Roles
        public IActionResult GetUsersWithRoles()
        {
            var result = _application.Users
                .Join(_application.Roles, u => u.RoleId, r => r.RoleId,
                (u, r) => new
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Gender = u.Gender ? "男" : "女",
                    Birthday = u.Birthday.ToString(),
                    Phone = u.Phone,
                    Photo = u.Photo,
                    RoleName = r.RoleName,
                });

            return Json(result);
        }
        #endregion


        #region 會員資料刪除
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _application.Users.FindAsync(id);
                if (user == null)
                {
                    // 加入調試信息，確認ID值和數據庫內是否匹配
                    return Json(new { success = false, message = "用戶未找到，ID: " + id });
                }

                _application.Users.Remove(user);
                await _application.SaveChangesAsync();
                return Json(new { success = true, message = "用戶已刪除" });
            }
            catch (Exception ex)
            {
                //記錄錯誤
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion


    }
}
