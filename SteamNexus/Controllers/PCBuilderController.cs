using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Text;
using SteamNexus.Services;
using SteamNexus.Data;

namespace SteamNexus.Controllers
{
    public class PCBuilderController : Controller
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;
        private readonly CoolPCWebScraping _coolPCWebScraping;

        // Constructor
        public PCBuilderController(SteamNexusDbContext context , CoolPCWebScraping coolPCWebScraping)
        {
            _context = context;
            _coolPCWebScraping = coolPCWebScraping;
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
            _coolPCWebScraping.UpdateAllComponentClassifications();

            _coolPCWebScraping.UpdateCPU();
            _coolPCWebScraping.UpdateGPU();
            _coolPCWebScraping.UpdateRAM();
            _coolPCWebScraping.UpdateMB();
            _coolPCWebScraping.UpdateSSD();
            _coolPCWebScraping.UpdateHDD();
            _coolPCWebScraping.UpdateAirCooler();
            _coolPCWebScraping.UpdateLiquidCooler();
            _coolPCWebScraping.UpdateCASE();
            _coolPCWebScraping.UpdatePSU();
            _coolPCWebScraping.UpdateOS();


            return "Run Success";
        }


    }
}
