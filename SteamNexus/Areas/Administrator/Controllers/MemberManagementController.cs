using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.ViewModels.Game;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static SteamNexus.Areas.Administrator.Controllers.MemberManagementController;
using static System.Net.Mime.MediaTypeNames;

namespace SteamNexus.Areas.Administrator.Controllers
{
    [Area("Administrator")]

    public class MemberManagementController : Controller
    {
        private readonly ILogger<MemberManagementController> _logger;
        private SteamNexusDbContext _application;
        private readonly IWebHostEnvironment _webHost;  //上傳圖片使用

        public MemberManagementController(SteamNexusDbContext application, IWebHostEnvironment webHostEnvironment, ILogger<MemberManagementController> logger)
        {
            _application = application;
            _webHost = webHostEnvironment;
            _logger = logger;
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
                result.RoleId,
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


        #region 會員資料刪除無刪除圖片
        //[HttpPost]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    try
        //    {
        //        var user = await _application.Users.FindAsync(id);
        //        if (user == null)
        //        {
        //            // 加入調試信息，確認ID值和數據庫內是否匹配
        //            return Json(new { success = false, message = "用戶未找到，ID: " + id });
        //        }

        //        _application.Users.Remove(user);
        //        await _application.SaveChangesAsync();
        //        return Json(new { success = true, message = "用戶已刪除" });
        //    }
        //    catch (Exception ex)
        //    {
        //        //記錄錯誤
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}
        #endregion


        #region 會員資料刪除並移除圖片

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

                // 刪除用戶的圖片
                if (!string.IsNullOrEmpty(user.Photo) && user.Photo != "default.jpg")
                {
                    var filePath = Path.Combine(_webHost.WebRootPath, "images/headshots", user.Photo);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
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


        #region EditViewModel
        public partial class EditUserViewModel
        {
            [Required]
            public int UserId { get; set; }

            [Required]
            public string RoleName { get; set; }


            [Required]
            [MaxLength(100)]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [MaxLength(50)]
            public string Name { get; set; }

            public bool Gender { get; set; } = true;

            #nullable enable
            [MaxLength(10)]
            [RegularExpression(@"^09\d{8}$", ErrorMessage = "手機號碼必須以09開頭並且是10位數字")]
            [Required(ErrorMessage = "請輸入手機號碼")]
            public string? Phone { get; set; }

            [Display(Name = "生日")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] // 指定日期的顯示格式
            public DateTime? Birthday { get; set; }


            public IFormFile? Photo { get; set; }

        }
        #endregion


        #region 修改資料更新成功無圖片
        //[HttpPost]
        //public async Task<ActionResult> EditUser([FromForm] EditUserViewModel data)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _application.Users
        //                    .Include(u => u.Role)
        //                    .FirstOrDefaultAsync(u => u.UserId == data.UserId);

        //        if (user == null)
        //        {
        //            return NotFound($"無法找到ID為 {data.UserId} 的用戶。");
        //        }

        //        user.UserId = data.UserId;
        //        user.Name = data.Name;
        //        user.Email = data.Email;
        //        user.Phone = data.Phone;
        //        user.Gender = data.Gender;
        //        user.Birthday = data.Birthday;

        //        // 查找新的角色ID
        //        var newRole = await _application.Roles.FirstOrDefaultAsync(r => r.RoleName == data.RoleName);
        //        if (newRole != null && newRole.RoleId != user.RoleId)
        //        {
        //            user.RoleId = newRole.RoleId;
        //        }

        //        await _application.SaveChangesAsync();
        //        TempData["SuccessMessage"] = "用戶資料已成功更新。";
        //        return RedirectToAction("MemberManagement");  // 或其他適當的成功頁面
        //    }
        //    return View(data);  // 返回表單與驗證錯誤
        //}

        #endregion


        #region 修改資料
        [HttpPost]
        public async Task<ActionResult> EditUser([FromForm] EditUserViewModel data)
        {
            if (ModelState.IsValid)
            {
                var user = await _application.Users
                            .Include(u => u.Role)  // 使用 Include 載入相關的角色資料
                            .FirstOrDefaultAsync(u => u.UserId == data.UserId);

                if (user == null)
                {
                    return NotFound($"無法找到ID為 {data.UserId} 的用戶。");
                }

                user.UserId = data.UserId;
                user.Name = data.Name;
                user.Email = data.Email;
                user.Phone = data.Phone;
                user.Gender = data.Gender;
                user.Birthday = data.Birthday;

                // 查找新的角色ID
                var newRole = await _application.Roles.FirstOrDefaultAsync(r => r.RoleName == data.RoleName);
                if (newRole != null && newRole.RoleId != user.RoleId)
                {
                    user.RoleId = newRole.RoleId;  // 只在角色有變化時更新角色ID
                }

                // 處理檔案上傳，沒有預設圖片
                //if (data.Photo != null && data.Photo.Length > 0)
                //{
                //    // 刪除舊的圖片
                //    if (!string.IsNullOrEmpty(user.Photo))
                //    {
                //        var oldFilePath = Path.Combine(_webHost.WebRootPath, "images/headshots", user.Photo);
                //        if (System.IO.File.Exists(oldFilePath))
                //        {
                //            System.IO.File.Delete(oldFilePath);
                //        }
                //    }

                //    // 儲存新的圖片
                //    string filename = Guid.NewGuid().ToString() + Path.GetExtension(data.Photo.FileName);
                //    string uploadfolder = Path.Combine(_webHost.WebRootPath, "images/headshots");
                //    string filepath = Path.Combine(uploadfolder, filename);

                //    using (var fileStream = new FileStream(filepath, FileMode.Create))
                //    {
                //        await data.Photo.CopyToAsync(fileStream);
                //    }

                //    user.Photo = filename;
                //}

                if (data.Photo != null && data.Photo.Length > 0)
                {
                    // 刪除舊的圖片，除非它是預設圖片
                    if (!string.IsNullOrEmpty(user.Photo) && user.Photo != "default.jpg")
                    {
                        var oldFilePath = Path.Combine(_webHost.WebRootPath, "images/headshots", user.Photo);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // 儲存新的圖片
                    //Guid.NewGuid().ToString():將圖片轉成字串(亂數)；Path.GetExtension(imagesFile.FileName):擷取圖片附檔名
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(data.Photo.FileName);
                    //圖片儲存路徑
                    string uploadfolder = Path.Combine(_webHost.WebRootPath, "images/headshots");
                    string filepath = Path.Combine(uploadfolder, filename);

                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await data.Photo.CopyToAsync(fileStream);
                    }

                    user.Photo = filename;
                }
                await _application.SaveChangesAsync();
                return RedirectToAction("MemberManagement");  // 或其他適當的成功頁面
            }
            return View(data);  // 返回表單與驗證錯誤
        }
        #endregion


        //權限
        public IActionResult GetRoles()
        {
            return PartialView("_createRolePartial");
        }


        #region 權限ViewModels
        public class createRoleViewModels
        {
            [Required]
            [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "只能夠輸入英文")]
            public string RoleName { get; set; }
        }
        #endregion


        #region 新增權限
        [HttpPost]
        public async Task<ActionResult> CreateRole([FromForm] createRoleViewModels data)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateRolePartial", data);
            }

            var newRole = new Role { RoleName = data.RoleName };
            _application.Roles.Add(newRole);
            await _application.SaveChangesAsync();

            return Json(new { success = true, message = "角色新增成功" });
        }
        #endregion


        #region 刪除腳色
        [HttpPost]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _application.Roles.FindAsync(id);
                if (role == null)
                {
                    // 加入調試信息，確認ID值和數據庫內是否匹配
                    return Json(new { success = false, message = "角色未找到，ID: " + id });
                }

                _application.Roles.Remove(role);
                await _application.SaveChangesAsync();
                return Json(new { success = true, message = "角色已刪除" });
            }
            catch (Exception ex)
            {
                //記錄錯誤
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion


        #region 會員ViewModels

        public class createMemberViewModel()
        {
            [Required]
            [MaxLength(50)]
            public string Name { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Email { get; set; }

            [Display(Name = "生日")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] // 指定日期的顯示格式
            public DateTime? Birthday { get; set; }

            [MaxLength(10)]
            [RegularExpression(@"^09\d{8}$", ErrorMessage = "手機號碼必須以09開頭並且是10位數字")]
            public string? Phone { get; set; }

            public bool Gender { get; set; } = true;

            public IFormFile? Photo { get; set; }
        }

        #endregion


        #region 新增會員
        public async Task<ActionResult> CreateMember([FromForm] createMemberViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_MemberManagementPartial", data);
            }

            try
            {
                // 處理檔案上傳
                string photoPath = "default.jpg";
                var photoFile = data.Photo;
                if (photoFile != null && photoFile.Length > 0)
                {
                    //Guid.NewGuid().ToString():將圖片轉成字串(亂數)；Path.GetExtension(imagesFile.FileName):擷取圖片附檔名
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
                    //圖片儲存路徑
                    string uploadfolder = Path.Combine(_webHost.WebRootPath, "images/headshots");
                    string filepath = Path.Combine(uploadfolder, filename);

                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(fileStream);
                    }

                    // 僅存儲相對路徑
                    photoPath = Path.Combine(filename);
                }
                else
                {
                    // 如果沒有上傳圖片，使用預設圖片
                    photoPath = Path.Combine("default.jpg");
                }

                // 模擬將數據保存到數據庫的邏輯
                _application.Users.Add(new User
                {
                    Name = data.Name,
                    Email = data.Email,
                    Password = HashPassword(data.Password), // 使用密碼哈希方法進行密碼加密
                    Birthday = data.Birthday,
                    Phone = data.Phone,
                    Gender = data.Gender,
                    Photo = photoPath, // 數據模型中包含存儲照片路徑的屬性
                    RoleId = 10000
                });
                await _application.SaveChangesAsync();

                return Json(new { success = true, message = "會員新增成功" });
            }
            catch (Exception ex)
            {
                // 處理和記錄異常
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                return Json(new { success = false, message = ex.Message, innerException = innerMessage });
            }
        }


        #endregion


        #region 確認權限是否存在
        [HttpGet]
        public async Task<IActionResult> CheckRolesExists(string rolename)
        {
            bool exists = await _application.Roles.AnyAsync(e => e.RoleName == rolename);
            return Json(!exists);  // 返回 false 表示 Role 已存在
        }
        #endregion


        #region 密碼加密
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        #endregion


        #region 確認Email是否存在
        [HttpGet]
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            bool exists = await _application.Users.AnyAsync(u => u.Email == email);
            return Json(!exists);  // 返回 false 表示 Email 已存在
        }
        #endregion

    }
}
