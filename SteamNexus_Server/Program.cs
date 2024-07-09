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
    // API �A��²��
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "SteamNexus API",
        Description = "SteamNexus API �������",
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

    // JWT Token ���ҫ��s���� �۰ʶ�J Header  
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

    // Ū�� XML �ɮײ��� API ����
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

// �[�J CORS ����
string MyAllowSpecificOrigins = "AllowAny";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy => policy.WithOrigins("https://www.steamnexus.org", "http://localhost:5173").WithMethods("*").WithHeaders("*")
    );
});

#region cookie����

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
//{
//    //���n�J�ɥ��۰ʾɤJ��o�Ӻ��}
//    option.LoginPath = new PathString("/api/Login/NoLogin");

//    //�����v����|�۰ʲ���즹���}
//    option.AccessDeniedPath = new PathString("/api/Login/noAccess");

//    //�n�J����ᥢ�ġF������cookie���|����v�T
//    //option.ExpireTimeSpan = TimeSpan.FromSeconds(5);
//});

#endregion


#region JWT����
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //�o�������
        ValidateIssuer = true,
        // �]�m���Ī��o���
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //����������
        ValidateAudience = true,
        //�]�m���Ī�������
        ValidAudience = builder.Configuration["Jwt:Audience"],
        //�n�J�ɶ����ҡA�w�]�Otrue�A�i�g�i���g
        ValidateLifetime = true,
        //���� Token ��ñ�W���_
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

//API���n����n�J������
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

// ���U�p�ɾ��A��
builder.Services.AddTransient<ScheduledTaskService>();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // �ҥ� Swagger �������
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
// �ҥ� wwwroot
app.UseStaticFiles();
// cookie�n�J
app.UseCookiePolicy();
// �ҥ� �����n�� ����
app.UseRouting();
// �M�Φ۩w�q CORS �]�w
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();

// �t�m Map �����n��
app.Map("/map1", Map1);
app.Map("/map2", Map2);

//app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// �ҥΦ۩w�q�����n��
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