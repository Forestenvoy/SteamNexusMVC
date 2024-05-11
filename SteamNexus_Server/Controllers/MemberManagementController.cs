using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using System.ComponentModel.DataAnnotations;
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
        public async Task<IActionResult> CreateRole([FromBody] createRoleViewModels data)
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



    }
}
