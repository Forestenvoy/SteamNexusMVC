using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;

namespace SteamNexus_Server.Controllers;

//[Authorize //權限標籤
//[AllowAnonymous] //允許匿名登入

// 套用 CORS 策略
[EnableCors("AllowAny")]
[Route("api/[controller]")]
[ApiController]
public class UserIdentityController : ControllerBase
{
    private SteamNexusDbContext _application;

    public UserIdentityController(SteamNexusDbContext application)
    {
        _application = application;

    }
    #region LonginViewModel
    public class LoginPost()
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    #endregion


    #region Lonin
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginPost data)
    {
        var user = _application.Users.SingleOrDefault(a => a.Email == data.Email && a.Password == data.Password);

        if (user == null)
        {
            return Ok("帳號或密碼錯誤");
        }
        else
        {
            // 驗證成功
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim("FullName", user.Name),
            new Claim("UserId",user.UserId.ToString()),
            // new Claim(ClaimTypes.Role, "Admin")
        };

            //多個權限設定
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


    #region Logout for UserId
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


    #region Logout for Name
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


}


