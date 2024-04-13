using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Text;
using SteamNexus.Services;
using SteamNexus.Data;

namespace SteamNexus.Controllers
{
    public class HardwareController : Controller
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;
        private readonly CoolPCWebScraping _coolPCWebScraping;

        // Constructor
        public HardwareController(SteamNexusDbContext context , CoolPCWebScraping coolPCWebScraping)
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
            Console.WriteLine("All OK");
            _coolPCWebScraping.UpdateCPU();
            Console.WriteLine("CPU OK");
            _coolPCWebScraping.UpdateGPU();
            Console.WriteLine("GPU OK");
            _coolPCWebScraping.UpdateRAM();
            Console.WriteLine("RAM OK");
            _coolPCWebScraping.UpdateMB();
            Console.WriteLine("MB OK");
            _coolPCWebScraping.UpdateSSD();
            Console.WriteLine("SSD OK");
            _coolPCWebScraping.UpdateHDD();
            Console.WriteLine("HDD OK");
            _coolPCWebScraping.UpdateAirCooler();
            Console.WriteLine("Air Cooler OK");
            _coolPCWebScraping.UpdateLiquidCooler();
            Console.WriteLine("LiquidCooler OK");
            _coolPCWebScraping.UpdateCASE();
            Console.WriteLine("CASE OK");
            _coolPCWebScraping.UpdatePSU();
            Console.WriteLine("PSU OK");
            _coolPCWebScraping.UpdateOS();
            Console.WriteLine("OS OK");


            return "Run Success";
        }


    }
}
