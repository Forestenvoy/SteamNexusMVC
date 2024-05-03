using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SteamNexus;
using SteamNexus.Data;
using SteamNexus.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Add Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// DataBase Connection String
var SteamNexusConnectionString = builder.Configuration.GetConnectionString("SteamNexus");
// Add SteamNexusDbContext
builder.Services.AddDbContext<SteamNexusDbContext>(options => options.UseSqlServer(SteamNexusConnectionString));

// Add CoolPCWebScrabing Service
builder.Services.AddTransient<CoolPCWebScraping>();

// 配置 防偽標籤 Antiforgery ~ 用於防止 CSRF 攻擊
builder.Services.AddAntiforgery(options =>
{
    // 指定在 HTML 表單中生成的隱藏欄位的名稱，該欄位將包含防偽標籤值。 <== 默認名稱可能會被猜出來
    options.FormFieldName = "__Antiforgery__SteamNexus";
    // 防偽標籤名稱
    options.HeaderName = "X-CSRF-TOKEN";
    // 防止 X-Frame-Options 標頭被禁用 <== 網頁不能被攻擊者嵌入到其他網站的 iframe 中
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddControllersWithViews();

#region cookie驗證

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //未登入時未自動導入到這個網址
    option.LoginPath = new PathString("/Home/Index");

    //未授權角色會自動移轉到此網址
    option.AccessDeniedPath = new PathString("/Home/NoRole");
});

#endregion

#region identity註冊驗證、密碼規則

//註冊驗證、密碼規則資訊
builder.Services.Configure<IdentityOptions>(options => { //這個函式設定了身分識別選項。
    options.Password.RequireDigit = true; //密碼是否需要包含數字。
    options.Password.RequireLowercase = true; //密碼是否需要包含小寫字母。
    options.Password.RequireNonAlphanumeric = true; //密碼是否需要包含非字母或數字的特殊字元。
    options.Password.RequireUppercase = true; //密碼是否需要包含大寫字母。
    options.Password.RequiredLength = 8; //最少的密碼長度。
    options.Password.RequiredUniqueChars = 1; //密碼中不重複的數字。

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //鎖定用戶的預設時間。
    options.Lockout.MaxFailedAccessAttempts = 3; //用戶允許的最大登入失敗次數。
    options.Lockout.AllowedForNewUsers = true; //是否允許新用戶被鎖定。

    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+"; //密碼設定字元
    options.User.RequireUniqueEmail = true; //Email驗證，唯一值(不可重複)


    options.SignIn.RequireConfirmedEmail = true; //使用者是否需要確認電子郵件地址後才能登入，需要驗證電子郵件通過後才能夠使用。
});

builder.Services.ConfigureApplicationCookie(options => { //這個函式設定了應用程式 Cookie 的相關選項。
    options.Cookie.HttpOnly = true; //Cookie 是否僅供 HTTP 存取。
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; //Cookie 是否要求始終透過安全連線 (HTTPS) 傳輸。
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5); //Cookie 的預時時間。
    options.LoginPath = "/Identity/Account/Login"; //登入頁面的路徑。
    options.AccessDeniedPath = "/Identity/Account/AccessDenied"; //存取被拒絕時導向的頁面路徑。
    options.SlidingExpiration = true; //是否啟用滑動過期時間。
});

#endregion

#region 驗證Email發送
//Email 
builder.Services.AddTransient<IEmailSender, EmailSendercs>();
builder.Services.AddControllersWithViews();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//cookie登入
app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

// 啟用 防偽標籤 cookie 服務
app.UseAntiforgery();

// Administrator Route Setting
app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// default Route Setting
app.MapControllerRoute(
    name: "Default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
