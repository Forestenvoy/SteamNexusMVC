using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Text;
using SteamNexus.Services;
using SteamNexus.Data;

namespace SteamNexus.Controllers
{
    public class PCBuilderController : Controller
    {
        private readonly SteamNexusDbContext _context;

        public PCBuilderController(SteamNexusDbContext context)
        {
            _context = context;
        }
        
        
        public IActionResult Index()
        {
            return View();
        }

        // 原價屋網頁爬蟲 從 CLIENT 端 呼叫測試

        // POST: /PCBuilder/WebScrabingTest
        [HttpPost]
        public string WebScrabingTest()
        {
            CoolPCWebScraping cpws = new CoolPCWebScraping(_context);

            cpws.test();
            


            return "Run Success";
        }


    }
}
