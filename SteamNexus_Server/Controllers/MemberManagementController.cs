using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SteamNexus_Server.Controllers
{
    // 套用 CORS 策略
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MemberManagementController : ControllerBase
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


        #region UserData
        [HttpGet("GetUsersWithRoles")]
        //[Authorize(Roles = "Member")] //有登入就可以看(系統管理員)
        //[Authorize] //有登入就可以看(一般會員)
        public IEnumerable<object> GetUsersWithRoles()
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

            return result;
        }
        #endregion


        #region RolesData
        [HttpGet("RolesData")]
        //[Authorize(Roles = "Admin,Adv")]
        public IEnumerable<object> RolesData()
        {
            var result = _application.Roles.Select(result => new
            {
                result.RoleId,
                result.RoleName,
            });
            return (result);
        }
        #endregion


        #region CreateRole ViewModels
        public class createRoleViewModels
        {
            [Required]
            [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "只能夠輸入英文")]
            public string RoleName { get; set; }
        }
        #endregion


        #region CreateRole

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromForm] createRoleViewModels data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newRole = new Role { RoleName = data.RoleName };
            _application.Roles.Add(newRole);
            await _application.SaveChangesAsync();

            return Ok(new { success = true, message = "角色新增成功" });
        }
        #endregion


        #region DeleteRole for id
        [HttpPost("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            try
            {
                var role = await _application.Roles.FindAsync(id);
                if (role == null)
                {
                    // 加入調試信息，確認ID值和數據庫內是否匹配
                    return Content(HttpStatusCode.NotFound, new { success = false, message = "角色未找到，ID: " + id });
                }

                _application.Roles.Remove(role);
                await _application.SaveChangesAsync();
                return Ok(new { success = true, message = "角色已刪除" });
            }
            catch (Exception ex)
            {
                //記錄錯誤
                return Content(HttpStatusCode.InternalServerError, new { success = false, message = ex.Message });
            }
        }
        #endregion


        #region DeleteRole for Name
        [HttpPost("DeleteRoleName")]
        public async Task<IActionResult> DeleteRoleName(string roleName)
        {
            try
            {
                var role = await _application.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
                if (role == null)
                {
                    // 加入調試信息，確認角色名和數據庫內是否匹配
                    return Content(HttpStatusCode.NotFound, new { success = false, message = "角色未找到，角色名稱: " + roleName });
                }

                _application.Roles.Remove(role);
                await _application.SaveChangesAsync();
                return Ok(new { success = true, message = "角色已刪除" });
            }
            catch (Exception ex)
            {
                //記錄錯誤
                return Content(HttpStatusCode.InternalServerError, new { success = false, message = ex.Message });
            }
        }
        #endregion


        #region Check Roles
        [HttpGet("CheckRolesExists")]
        public async Task<IActionResult> CheckRolesExists(string rolename)
        {
            bool exists = await _application.Roles.AnyAsync(e => e.RoleName == rolename);
            return Ok(!exists);  // 返回 false 表示角色已存在，true 表示不存在
        }
        #endregion


        #region CreateMember ViewModels
        public class CreateMemberViewModel
        {
            [Required]
            [MaxLength(50)]
            public string? Name { get; set; }

            [Required]
            public string Password { get; set; }

            [Required]
            public string ConfirmPassword { get; set; }

            [Required]
            public string Email { get; set; }

            [Display(Name = "生日")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? Birthday { get; set; }

            [MaxLength(10)]
            [RegularExpression(@"^09\d{8}$", ErrorMessage = "手機號碼必須以09開頭並且是10位數字")]
            public string? Phone { get; set; }

            public bool Gender { get; set; } = true;

            public IFormFile? Photo { get; set; }
        }
        #endregion


        #region CreateMember
        [HttpPost("CreateMember")]
        
        public async Task<IActionResult> CreateMember([FromForm] CreateMemberViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string photoPath = "default.jpg";
                var photoFile = data.Photo;
                if (photoFile != null && photoFile.Length > 0)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
                    string uploadfolder = Path.Combine(_webHost.WebRootPath, "images/headshots");
                    string filepath = Path.Combine(uploadfolder, filename);

                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await photoFile.CopyToAsync(fileStream);
                    }

                    //photoPath = Path.Combine("images/headshots", filename);
                    photoPath = filename;
                }

                _application.Users.Add(new User
                {
                    Name = data.Name,
                    Email = data.Email,
                    Password = HashPassword(data.Password),
                    Birthday = data.Birthday,
                    Phone = data.Phone,
                    Gender = data.Gender,
                    Photo = photoPath,
                    RoleId = 10001
                });

                await _application.SaveChangesAsync();

                return Ok(new { success = true, message = "會員新增成功" });
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = ex.Message, innerException = innerMessage });
            }
        }
        #endregion


        #region DeleteUser no Delete Tracking
        ////[HttpDelete("{id}")]
        //[HttpPost("DeleteUser")]
        //public async Task<IActionResult> DeleteUser(int id)
        //{
        //    try
        //    {
        //        var user = await _application.Users.FindAsync(id);
        //        if (user == null)
        //        {
        //            // 加入調試信息，確認ID值和數據庫內是否匹配
        //            return NotFound(new { success = false, message = "使用者未找到，ID: " + id });
        //        }

        //        // 刪除用戶的圖片
        //        if (!string.IsNullOrEmpty(user.Photo) && user.Photo != "default.jpg")
        //        {
        //            var filePath = Path.Combine(_webHost.WebRootPath, "images/headshots", user.Photo);
        //            if (System.IO.File.Exists(filePath))
        //            {
        //                System.IO.File.Delete(filePath);
        //            }
        //        }

        //        _application.Users.Remove(user);
        //        await _application.SaveChangesAsync();
        //        return Ok(new { success = true, message = "使用者已刪除" });
        //    }
        //    catch (Exception ex)
        //    {
        //        //記錄錯誤
        //        return BadRequest(new { success = false, message = ex.Message });
        //    }
        //}
        #endregion


        #region DeleteUser and GameTracking
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _application.Users.FindAsync(id);
                if (user == null)
                {
                    // 加入調試信息，確認ID值和數據庫內是否匹配
                    return NotFound(new { success = false, message = "使用者未找到，ID: " + id });
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

                // 刪除用戶的遊戲追蹤記錄
                var userTrackings = _application.GameTrackings.Where(gt => gt.UserId == id);
                _application.GameTrackings.RemoveRange(userTrackings);

                _application.Users.Remove(user);
                await _application.SaveChangesAsync();
                return Ok(new { success = true, message = "使用者及其遊戲追蹤記錄已刪除" });
            }
            catch (Exception ex)
            {
                //記錄錯誤
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        #endregion


        #region UpdateUserViewModel

        public class EditUserViewModel
        {

            public int UserId { get; set; }


            public string RoleName { get; set; }


            [MaxLength(100)]
            [EmailAddress]
            public string Email { get; set; }


            [MaxLength(50)]
            public string Name { get; set; }

            public bool Gender { get; set; } = true;

            #nullable enable
            [MaxLength(10)]
            [RegularExpression(@"^09\d{8}$", ErrorMessage = "手機號碼必須以09開頭並且是10位數字")]
            public string? Phone { get; set; }

            [Display(Name = "生日")]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime? Birthday { get; set; }

            public IFormFile? Photo { get; set; }
        }
        #endregion


        #region UpdateUser for data
        [HttpPost("EditUser")]
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
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(data.Photo.FileName);
                    string uploadfolder = Path.Combine(_webHost.WebRootPath, "images/headshots");
                    string filepath = Path.Combine(uploadfolder, filename);

                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await data.Photo.CopyToAsync(fileStream);
                    }

                    user.Photo = filename;
                }

                await _application.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest(ModelState);  // 返回表單與驗證錯誤
        }
        #endregion


        #region UpdateUser for id
        [HttpPut("{id}")]
        public async Task<ActionResult> EditUser(int id, [FromForm] EditUserViewModel data)
        {
            if (id != data.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

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
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(data.Photo.FileName);
                    string uploadfolder = Path.Combine(_webHost.WebRootPath, "images/headshots");
                    string filepath = Path.Combine(uploadfolder, filename);

                    using (var fileStream = new FileStream(filepath, FileMode.Create))
                    {
                        await data.Photo.CopyToAsync(fileStream);
                    }

                    user.Photo = filename;
                }

                await _application.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest(ModelState);  // 返回表單與驗證錯誤
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


        #region Check Email
        [HttpGet("CheckEmailExists")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            bool exists = await _application.Users.AnyAsync(u => u.Email == email);
            return Ok(!exists);  // 返回 false 表示 Email 已存在，true表示Email不存在
        }
        #endregion


        #region DeleteRole in method
        private IActionResult Content(HttpStatusCode notFound, object value)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
