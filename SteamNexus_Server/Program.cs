using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using SteamNexus_Server.Data;
using SteamNexus_Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 加入 CORS 策略
string MyAllowSpecificOrigins = "AllowAny";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy => policy.WithOrigins("*").WithMethods("*").WithHeaders("*")
    );
});


#region cookie驗證

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    //未登入時未自動導入到這個網址
    option.LoginPath = new PathString("/api/Login/NoLogin");

    //未授權角色會自動移轉到此網址
    option.AccessDeniedPath = new PathString("/api/Login/noAccess");

    //登入後兩秒後失效；全部的cookie都會受到影響
    //option.ExpireTimeSpan = TimeSpan.FromSeconds(5);
});

#endregion


// DataBase Connection String
var SteamNexusConnectionString = builder.Configuration.GetConnectionString("SteamNexus");
// Add SteamNexusDbContext
builder.Services.AddDbContext<SteamNexusDbContext>(options => options.UseSqlServer(SteamNexusConnectionString));

// Add CoolPCWebScrabing Service
builder.Services.AddTransient<CoolPCWebScraping>();

var app = builder.Build();

// 啟用 wwwroot
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 套用自定義 CORS 設定
app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

//cookie登入
app.UseCookiePolicy();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();