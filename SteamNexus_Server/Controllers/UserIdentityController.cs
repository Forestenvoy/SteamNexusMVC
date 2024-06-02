using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace SteamNexus_Server.Controllers;

//[Authorize //權限標籤
//[AllowAnonymous] //允許匿名登入

// 套用 CORS 策略
[EnableCors("AllowAny")]
[Route("api/[controller]")]
[ApiController]
public class UserIdentityController : ControllerBase
{
    private readonly ILogger<MemberManagementController> _logger;
    private SteamNexusDbContext _application;
    private readonly IWebHostEnvironment _webHost;  //上傳圖片使用

    public IConfiguration _configuration; //JWT引用

    public UserIdentityController(SteamNexusDbContext application,IConfiguration configuration, ILogger<MemberManagementController> logger, IWebHostEnvironment webHost)
    {
        _application = application;
        _configuration = configuration;
        _logger = logger;
        _webHost = webHost;
    }



    #region LonginViewModel
    public class LoginPost
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    #endregion


    #region LoninCookie
    [HttpPost("LoginCookie")]
    public async Task<IActionResult> Login([FromBody] LoginPost data)
    {
        var user = _application.Users
                              .Include(u => u.Role) // 確保 Role 被包含在查詢結果中
                              .SingleOrDefault(a => a.Email == data.Email && a.Password == data.Password);

        if (user == null)
        {
            return Ok("帳號或密碼錯誤");
        }
        else
        {
            // 驗證成功
            var claims = new List<Claim>

        {   //使用系統預設取得資料
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.RoleName),
            
            //自訂異取得資料
            new Claim("FullName", user.Name),
            new Claim("UserId",user.UserId.ToString()),
            // new Claim(ClaimTypes.Role, "Admin")
        };

            //多個權限設定 for 新增使用者
            //var role = from r in _application.Roles where r.RoleId == user.UserId select r;

            //foreach (var temp in role)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, temp.RoleName));
            //}


            //var authProperties = new AuthenticationProperties
            //{
            //AllowRefresh = true, // 允許刷新身份驗證會話
            // Refreshing the authentication session should be allowed.

            //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),   // 設置過期時間為 10 分鐘後
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.

            //IsPersistent = true,  //會話在多次請求中保持持久
            // Whether the authentication session is persisted across 
            // multiple requests. When used with cookies, controls
            // whether the cookie's lifetime is absolute (matching the
            // lifetime of the authentication ticket) or session-based.

            //IssuedUtc = DateTimeOffset.UtcNow,  //設置登入的時間
            // The time at which the authentication ticket was issued.

            //RedirectUri = "/Home/Index" //設置時間過期後重新導向 URI
            // The full path or absolute URI to be used as an http 
            // redirect response value.
            //};

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return Ok($"{user.Name}登入成功");
        }
    }
    #endregion


    #region Logout(cookie) for UserId
    //[HttpDelete("Logout")]
    //public async Task<IActionResult> Logout()
    //{
    //    // 取得目前已驗證的使用者
    //    var user = HttpContext.User;

    //    // 從身份驗證票證中取得使用者 ID，假設使用者 ID 存在於 "UserId" Claim 中
    //    var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId");
    //    var userId = userIdClaim?.Value;

    //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    //    // 回傳使用者 ID 已登出的訊息
    //    return Ok($"{userId}已登出");
    //}

    //[HttpGet("NoLogin")]
    //public IActionResult NoLogin()
    //{
    //    return Ok("未登入");
    //}
    #endregion


    #region Logout(cookie) for Name
    [HttpDelete("Logout")]
    public async Task<IActionResult> Logout()
    {
        // 取得目前已驗證的使用者
        var user = HttpContext.User;

        // 從身份驗證票證中取得 FullName，假設 FullName 存在於 "FullName" Claim 中
        var fullNameClaim = user.Claims.FirstOrDefault(c => c.Type == "FullName");
        var fullName = fullNameClaim?.Value;

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // 回傳 FullName 已登出的訊息
        return Ok($"{fullName}已登出");
    }

    [HttpGet("NoLogin")]
    public IActionResult NoLogin()
    {
        return Ok("未登入");
    }
    #endregion


    #region noAccess
    [HttpGet("noAccess")]
    public IActionResult noAccess()
    {
        return Ok("沒有權限");
    }
    #endregion


    #region LoginJWT
    [HttpPost("JwtLogin")]
    public async Task<IActionResult> JwtLogin([FromBody] LoginPost data)
    {
        var user = _application.Users
                              .Include(u => u.Role) // 確保 Role 被包含在查詢結果中
                              .SingleOrDefault(a => a.Email == data.Email && a.Password == data.Password);

        if (user == null)
        {
            return BadRequest("帳號或密碼錯誤");
        }
        else
        {
            // 驗證成功
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //token識別標籤，唯一值
            //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()), //顯示發行時間
            new Claim(JwtRegisteredClaimNames.Iat, ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString()),  //修正為 Unix 時間
            //new Claim(JwtRegisteredClaimNames.Email, user.Email),

            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role.RoleName),


            //new Claim("UserId", user.UserId.ToString()),
            new Claim("FullName", user.Name),
            //new Claim("Roles", user.Role.RoleName),
            new Claim("image",user.Photo),
                        
        };

            // 如果使用者有多個角色，可以在這裡添加多個角色權限
            //var roles = from r in _application.UserRoles
            //            where r.UserId == user.UserId
            //            select r.Role.RoleName;

            //foreach (var roleName in roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, roleName));
            //}

            // Jwt 金鑰資訊
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.UtcNow.AddDays(1),// 登入過期時間
                        signingCredentials: signIn);

            //直接回傳結果
            //return Ok(new JwtSecurityTokenHandler().WriteToken(token));

            //先宣告再回傳結果
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            //return Ok(new { Token = tokenString });

            //顯示使用者登入訊息
            return Ok(new
            {
                Message = $"{user.Name} 已登入成功",// 顯示登入成功訊息
                Token = tokenString,
            });
        }
    }
    #endregion


    #region JWTCheckUserRoles
    [HttpGet("CheckUserRoles")]
    public IActionResult CheckUserRoles()
    {
        var user = HttpContext.User;
        var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
        var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();
        return Ok(new { UserName = user.Identity.Name, Roles = roles, Claims = claims });
    }
    #endregion


    #region CookieCheckUserRoles
    [HttpGet("CheckUserRolescookie")]
    public IActionResult CheckUserRolescookie()
    {
        var user = HttpContext.User;

        if (user.Identity.IsAuthenticated)
        {
            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            var claims = user.Claims.Select(c => new { c.Type, c.Value }).ToList();

            return Ok(new
            {
                UserName = user.Identity.Name,
                Roles = roles,
                Claims = claims
            });
        }
        else
        {
            return Unauthorized("未登入或Cookie無效");
        }
    }
    #endregion


    #region JWT CheckTime
    [HttpGet("CheckJwtExpiration")]
    public IActionResult CheckJwtExpiration()
    {
        //從Header取得Authorization的數值，並轉換字串
        var authHeader = Request.Headers["Authorization"].ToString();

        //檢查欄位是否有Bearer開頭的欄位
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {   
            //提取token
            var token = authHeader.Substring("Bearer ".Length).Trim();

            //解析token
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return BadRequest("無效的JWT");
            }

            // 將 UTC 時間轉換為當地時間
            var localTime = jwtToken.ValidTo.ToLocalTime();

            //設定顯示時間格式
            var formattedExpiration = localTime.ToString("yyyy/MM/dd:HH:mm:ss");
            return Ok(new { Expiration = formattedExpiration });
        }
        else
        {
            return BadRequest("Authorization header is missing or does not contain a Bearer token");
        }
    }
    #endregion


    #region Get UserId
    [HttpGet("GetUserIdFromToken")]
    public IActionResult GetUserIdFromToken()
    {
        //從Header取得Authorization的數值，並轉換字串
        var authHeader = Request.Headers["Authorization"].ToString();

        //檢查欄位是否有Bearer開頭的欄位
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            //提取token
            var token = authHeader.Substring("Bearer ".Length).Trim();

            //解析token
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return BadRequest("無效的JWT");
            }

            //取得ID
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                Console.WriteLine($"UserId: {userIdClaim.Value}");
                _logger.LogInformation($"UserId: {userIdClaim.Value}");
                return Ok(new { UserId = userIdClaim.Value });
            }
            else
            {
                return BadRequest("未找到用戶 ID");
            }
        }
        else
        {
            return BadRequest("Authorization 欄位不存在或不以 Bearer  開頭");
        }
    }
    #endregion


    #region Get UserInfo by UserId
    [HttpGet("GetUserInfoFromToken")]
    public IActionResult GetUserInfoFromToken()
    {
        // 從Header取得Authorization的數值，並轉換字串
        var authHeader = Request.Headers["Authorization"].ToString();

        // 檢查欄位是否有Bearer開頭的欄位
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            // 提取token
            var token = authHeader.Substring("Bearer ".Length).Trim();

            // 解析token
            var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
            {
                return BadRequest("無效的JWT");
            }

            // 取得ID
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var userId = int.Parse(userIdClaim.Value);
                var user = _application.Users
                                      .Include(u => u.Role) // 確保Role被包含在查詢結果中
                                      .SingleOrDefault(u => u.UserId == userId);

                if (user == null)
                {
                    return NotFound("未找到用戶");
                }

                // 返回會員的所有資訊
                return Ok(new
                {
                    user.UserId,
                    //user.RoleId,
                    user.Email,
                    //user.Password,
                    user.Name,
                    user.Gender,
                    user.Phone,
                    user.Birthday,
                    user.Photo,
                });
            }
            else
            {
                return BadRequest("未找到用戶 ID");
            }
        }
        else
        {
            return BadRequest("Authorization 欄位不存在或不以 Bearer 開頭");
        }
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


    #region ChangePasswordViewModel
    public class ChangePasswordViewModel
    {
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
    }
    #endregion


    #region ChangePassword
    [HttpPost("ChangePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 從 Header 取得 JWT
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest("Authorization header is missing or does not contain a Bearer token");
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        // 解析 JWT
        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        if (jwtToken == null)
        {
            return BadRequest("Invalid JWT");
        }

        // 從 JWT 中取得用戶 ID
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Unable to identify user");
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return BadRequest("Invalid user ID format");
        }

        // 從資料庫取得用戶
        var user = await _application.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound("未找到用戶");
        }

        // 檢查舊密碼是否正確
        if (HashPassword(data.oldPassword) != user.Password)
        {
            return BadRequest("舊密碼不正確");
        }

        // 更新用戶密碼
        user.Password = HashPassword(data.newPassword);
        _application.Users.Update(user);
        await _application.SaveChangesAsync();

        return Ok("密碼修改成功");
    }
    #endregion


    #region EditUserViewModel
    public class EditUserViewModel
    {

        public int UserId { get; set; }

        [MaxLength(50)]
        public string? Name { get; set; }

        public bool? Gender { get; set; } = true;

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


    #region EditMember
    [HttpPut("EditMember")]
    [Authorize]
    public async Task<IActionResult> EditMember([FromForm] EditUserViewModel data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // 從 Header 取得 JWT
        var authHeader = Request.Headers["Authorization"].ToString();
        if (authHeader == null || !authHeader.StartsWith("Bearer "))
        {
            return BadRequest("Authorization header is missing or does not contain a Bearer token");
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();
        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token) as JwtSecurityToken;
        if (jwtToken == null)
        {
            return BadRequest("Invalid JWT");
        }

        // 從 JWT 中取得用戶 ID
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            return Unauthorized("Unable to identify user");
        }

        if (!int.TryParse(userIdClaim.Value, out int userId))
        {
            return BadRequest("Invalid user ID format");
        }

        // 從資料庫取得用戶
        var user = await _application.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound("未找到用戶");
        }

        // 更新用戶資料
        if (data.Name != null) user.Name = data.Name;
        if (data.Gender.HasValue) user.Gender = data.Gender.Value;
        if (data.Phone != null) user.Phone = data.Phone;
        if (data.Birthday.HasValue) user.Birthday = data.Birthday.Value;

        // 檢查是否有上傳新圖片
        var photoFile = data.Photo;
        if (photoFile != null && photoFile.Length > 0)
        {
            // 刪除原本的圖片，排除預設圖片
            if (!string.IsNullOrEmpty(user.Photo) && user.Photo != "default.jpg")
            {
                string existingFilePath = Path.Combine(_webHost.WebRootPath, "images/headshots", user.Photo);
                if (System.IO.File.Exists(existingFilePath))
                {
                    System.IO.File.Delete(existingFilePath);
                }
            }

            // 上傳新圖片
            string filename = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
            string uploadfolder = Path.Combine(_webHost.WebRootPath, "images/headshots");
            string filepath = Path.Combine(uploadfolder, filename);

            using (var fileStream = new FileStream(filepath, FileMode.Create))
            {
                await photoFile.CopyToAsync(fileStream);
            }

            user.Photo = filename;
        }

        _application.Users.Update(user);
        await _application.SaveChangesAsync();

        return Ok("會員資料修改成功");
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

}
