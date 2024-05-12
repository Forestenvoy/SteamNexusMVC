using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace SteamNexus_Server.Controllers
{
    // 套用 CORS 策略
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
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
        public IEnumerable<Object> GetUsersWithRoles()
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


        #region Is there a Role?
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
            public string Name { get; set; }

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

                    photoPath = Path.Combine("images/headshots", filename);
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
                    RoleId = 10000
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


        #region DeleteUser
        //[HttpDelete("{id}")]
        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _application.Users.FindAsync(id);
                if (user == null)
                {
                    // 加入調試信息，確認ID值和數據庫內是否匹配
                    return NotFound(new { success = false, message = "用戶未找到，ID: " + id });
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
                return Ok(new { success = true, message = "用戶已刪除" });
            }
            catch (Exception ex)
            {
                //記錄錯誤
                return BadRequest(new { success = false, message = ex.Message });
            }
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
