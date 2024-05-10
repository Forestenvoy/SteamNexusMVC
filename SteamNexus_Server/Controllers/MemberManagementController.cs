using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace SteamNexus_Server.Controllers
{
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



    }
}
