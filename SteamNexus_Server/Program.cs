using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SteamNexus_Server.Data;
using SteamNexus_Server.Services;
using System.Text;
using SteamNexus_Server.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // API 服務簡介
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SteamNexus API",
        Description = "SteamNexus API 說明文件",
        //TermsOfService = new Uri("https://igouist.github.io/"),
        //Contact = new OpenApiContact
        //{
        //    Name = "Igouist",
        //    Email = string.Empty,
        //    Url = new Uri("https://igouist.github.io/about/"),
        //},
        //License = new OpenApiLicense
        //{
        //    Name = "TEST",
        //    Url = new Uri("https://igouist.github.io/about/"),
        //}
    });

    // JWT Token 驗證按鈕產生 自動塞入 Header  
    options.AddSecurityDefinition("Bearer",
    new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization"
    });

    options.AddSecurityRequirement(
    new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // 讀取 XML 檔案產生 API 說明
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// 加入 CORS 策略
string MyAllowSpecificOrigins = "AllowAny";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy => policy.WithOrigins("https://www.steamnexus.org", "http://localhost:5173").WithMethods("*").WithHeaders("*")
    );
});

#region cookie驗證

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
//{
//    //未登入時未自動導入到這個網址
//    option.LoginPath = new PathString("/api/Login/NoLogin");

//    //未授權角色會自動移轉到此網址
//    option.AccessDeniedPath = new PathString("/api/Login/noAccess");

//    //登入後兩秒後失效；全部的cookie都會受到影響
//    //option.ExpireTimeSpan = TimeSpan.FromSeconds(5);
//});

#endregion


#region JWT驗證
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //發行者驗證
        ValidateIssuer = true,
        // 設置有效的發行者
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //接收者驗證
        ValidateAudience = true,
        //設置有效的接收者
        ValidAudience = builder.Configuration["Jwt:Audience"],
        //登入時間驗證，預設是true，可寫可不寫
        ValidateLifetime = true,
        //驗證 Token 的簽名金鑰
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//API都要受到登入的限制
//builder.Services.AddMvc(options =>
//{
//    options.Filters.Add(new AuthorizeFilter());
//});

#endregion


// DataBase Connection String
var SteamNexusConnectionString = builder.Configuration.GetConnectionString("SteamNexus");
// Add SteamNexusDbContext
builder.Services.AddDbContext<SteamNexusDbContext>(options => options.UseSqlServer(SteamNexusConnectionString));


// Add CoolPCWebScrabing Service
builder.Services.AddTransient<CoolPCWebScraping>();
builder.Services.AddTransient<GameTimer>();

// 註冊計時器服務
builder.Services.AddTransient<ScheduledTaskService>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 啟用 Swagger 說明文件
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SteamNexus API");
    });
}


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
// 啟用 wwwroot
app.UseStaticFiles();
// cookie登入
app.UseCookiePolicy();
// 啟用 中介軟體 路由
app.UseRouting();
// 套用自定義 CORS 設定
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

// 配置 Map 中介軟體
app.Map("/map1", Map1);
app.Map("/map2", Map2);

//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// 啟用自定義中介軟體
app.UseCustom();
app.Run(async context =>
{
    await context.Response.WriteAsync("Run \r\n");
});

app.Run();

// Define Map1
static void Map1(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test 1");
    });
}

static void Map2(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test 2");
    });
}