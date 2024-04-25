using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.Services;
using System.ComponentModel.DataAnnotations;
using SteamNexus.ViewModels.Game;

namespace SteamNexus.Controllers
{
    public class AdminController : Controller
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;
        private readonly CoolPCWebScraping _coolPCWebScraping;
        // Constructor
        public AdminController(SteamNexusDbContext context, CoolPCWebScraping coolPCWebScraping)
        {
            _context = context;
            _coolPCWebScraping = coolPCWebScraping;
        }


        public IActionResult Index()
        {
            return View();
        }

        // Admin/HardwareManagement
        [HttpGet]
        public IActionResult HardwareManagement()
        {
            // 下拉式選單 => 硬體
            ViewBag.ComputerParts = new SelectList(_context.ComputerPartCategories.Select(c => c.Name));
            return PartialView("_HardwareManagementPartial");
        }

        // GET: Admin/HardwareData
        [HttpGet]
        public IActionResult HardwareData(int Type)
        {
            // 尋找特定硬體
            var result = _context.ComponentClassifications
                .Where(c => c.ComputerPartCategoryId == Type)
                .Join(_context.ProductInformations, c => c.ComponentClassificationId, p => p.ComponentClassificationId,
                (c, p) => new
                {
                    ProductId = p.ProductInformationId,
                    ComponentClassificationName = c.Name,
                    ProductName = p.Name,
                    p.Specification,
                    p.Price,
                    p.Wattage,
                    p.Recommend
                });
            return Json(result);
        }

        // 推薦變更呼叫
        public class RecommondData
        {
            [Required]
            [Range(10000, 99999)]
            public int ProductId { get; set; }

            [Required]
            [Range(0, 4)]
            public int recommend { get; set; }
        }

        // POST: Admin/recommendChange
        [HttpPost]
        public IActionResult recommendChange([FromBody] RecommondData data)
        {
            // 如果驗證合法
            if (ModelState.IsValid)
            {
                var product = _context.ProductInformations
                    .Where(p => p.ProductInformationId == data.ProductId)
                    .FirstOrDefault();
                if (product != null)
                {
                    product.Recommend = data.recommend;
                    _context.SaveChanges();
                    return Ok("變更成功");
                }
                else
                {
                    return Ok("找不到產品");
                }
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        // 單一零件更新
        public class HardwareType
        {
            [Required]
            [Range(10000, 10010)]
            public int Type { get; set; }
        }

        // POST: Admin/UpdateHardwareOne
        [HttpPost]
        public string UpdateHardwareOne([FromBody] HardwareType data)
        {
            // 如果驗證合法
            if (ModelState.IsValid)
            {
                _coolPCWebScraping.UpdateAllComponentClassifications();
                switch (data.Type)
                {
                    case (int)ComputerPartCategory.Type.CPU:
                        _coolPCWebScraping.UpdateCPU();
                        return "CPU 更新成功";
                    case (int)ComputerPartCategory.Type.MB:
                        _coolPCWebScraping.UpdateMB();
                        return "MB 更新成功";
                    case (int)ComputerPartCategory.Type.RAM:
                        _coolPCWebScraping.UpdateRAM();
                        return "RAM 更新成功";
                    case (int)ComputerPartCategory.Type.SSD:
                        _coolPCWebScraping.UpdateSSD();
                        return "SSD 更新成功";
                    case (int)ComputerPartCategory.Type.HDD:
                        _coolPCWebScraping.UpdateHDD();
                        return "HDD 更新成功";
                    case (int)ComputerPartCategory.Type.AirCooler:
                        _coolPCWebScraping.UpdateAirCooler();
                        return "AirCooler 更新成功";
                    case (int)ComputerPartCategory.Type.LiquidCooler:
                        _coolPCWebScraping.UpdateLiquidCooler();
                        return "LiquidCooler 更新成功";
                    case (int)ComputerPartCategory.Type.GPU:
                        _coolPCWebScraping.UpdateGPU();
                        return "GPU 更新成功";
                    case (int)ComputerPartCategory.Type.CASE:
                        _coolPCWebScraping.UpdateCASE();
                        return "CASE 更新成功";
                    case (int)ComputerPartCategory.Type.PSU:
                        _coolPCWebScraping.UpdatePSU();
                        return "PSU 更新成功";
                    case (int)ComputerPartCategory.Type.OS:
                        _coolPCWebScraping.UpdateOS();
                        return "OS 更新成功";
                    default:
                        return "更新失敗";
                }
            }
            else
            {
                return "更新失敗";
            }
        }

        // 全零件更新
        // POST: Admin/UpdateHardwareAll
        [HttpPost]
        public string UpdateHardwareAll()
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


            return "全零件更新成功";
        }


        [HttpGet]
        public IActionResult MemberManagement()
        {
            // 
            return PartialView("_MemberManagementPartial");
        }

        [HttpGet]
        public IActionResult GameManagement()
        {
            // 
            return PartialView("_GaenManagementPartial");
        }


        //GameAJAX設定
        private async Task<DetailsViewModel> GetViewModel(int id)
        {
            var game = await _context.Games
                .FirstOrDefaultAsync(m => m.GameId == id);

            if (game == null)
            {
                return null; // 或者根據您的需求返回其他值
            }

            return new DetailsViewModel
            {
                GameId = id,
                AppId = game.AppId,
                Name = game.Name,
                OriginalPrice = game.OriginalPrice,
                CurrentPrice = game.CurrentPrice,
                LowestPrice = game.LowestPrice,
                AgeRating = game.AgeRating,
                Comment = game.Comment,
                CommentNum = game.CommentNum,
                ReleaseDate = game.ReleaseDate,
                Publisher = game.Publisher,
                Description = game.Description,
                Players = game.Players,
                PeakPlayers = game.PeakPlayers,
                ImagePath = game.ImagePath,
                VideoPath = game.VideoPath
            };
        }


        [HttpGet]
        public IActionResult GetIndexPartialView()
        {
            var steamNexusDbContext = _context.Games;
            return PartialView("_GameIndexManagementPartial1", steamNexusDbContext);
        }

        [HttpGet]
        public IActionResult GetCreatPartialView()
        {
            return PartialView("_GameCreateManagementPartial");
        }

        [HttpPost]
        public async Task<IActionResult> PostCreatPartialToDB(Game game)
        {
            var steamNexusDbContext = _context.Games;

            if (ModelState.IsValid)
            {

                //Game game = new Game
                //{
                //    AppId = Create.AppId,
                //    Name = Create.Name,
                //    OriginalPrice = Create.OriginalPrice,
                //    AgeRating = Create.AgeRating,
                //    ReleaseDate = Create.ReleaseDate,
                //    Publisher = Create.Publisher,
                //    Description = Create.Description,
                //    ImagePath = Create.ImagePath,
                //    VideoPath = Create.VideoPath,
                //};

                _context.Games.Add(game);
                await _context.SaveChangesAsync();
                return GetIndexPartialView();
            }


            return GetIndexPartialView();
        }

        [HttpGet]
        public IActionResult GetEditPartialView(int id)
        {

            var game = _context.Games.FindAsync(id).Result;
          
            EditViewModel ViewModel = new EditViewModel
            {
                GameId= id,
                AppId = (int)game.AppId,
                Name = game.Name,
                OriginalPrice = (int)game.OriginalPrice,
                AgeRating = game.AgeRating,
                ReleaseDate = game.ReleaseDate,
                Publisher = game.Publisher,
                Description = game.Description,
                ImagePath = game.ImagePath,
                VideoPath = game.VideoPath
            };

            if (game == null)
            {
                return NotFound();
            }
            return PartialView("_GameEditeManagementPartial", ViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> PostEditPartialToDB(Game game)
        {
            //if (id != game.GameId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {
                     _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.GameId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return GetIndexPartialView();
            }
            //ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId", game.MinReqId);
            //ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId", game.RecReqId);
            return GetIndexPartialView();
        }

        [HttpGet]
        public IActionResult GetDetailsPartialView(int id)
        {

            var game = _context.Games
               .FirstOrDefaultAsync(m => m.GameId == id).Result;

            if (game == null)
            {
                return NotFound();
            }

            DetailsViewModel ViewModel = new DetailsViewModel
            {
                GameId = id,
                AppId = game.AppId,
                Name = game.Name,
                OriginalPrice = game.OriginalPrice,
                CurrentPrice = game.CurrentPrice,
                LowestPrice = game.LowestPrice,
                AgeRating = game.AgeRating,
                Comment = game.Comment,
                CommentNum = game.CommentNum,
                ReleaseDate = game.ReleaseDate,
                Publisher = game.Publisher,
                Description = game.Description,
                Players = game.Players,
                PeakPlayers = game.PeakPlayers,
                ImagePath = game.ImagePath,
                VideoPath = game.VideoPath
            };

            return PartialView("_GameDetailsManagementPartial", ViewModel);
        }

        [HttpGet]
        public IActionResult GetDeletePartialView(int id)
        {

            var game = _context.Games
                .FirstOrDefaultAsync(m => m.GameId == id).Result;

            if (game == null)
            {
                return NotFound(); // 或者根據您的需求返回其他值
            }

            DetailsViewModel ViewModel = new DetailsViewModel
            {
                GameId = id,
                AppId = game.AppId,
                Name = game.Name,
                OriginalPrice = game.OriginalPrice,
                CurrentPrice = game.CurrentPrice,
                LowestPrice = game.LowestPrice,
                AgeRating = game.AgeRating,
                Comment = game.Comment,
                CommentNum = game.CommentNum,
                ReleaseDate = game.ReleaseDate,
                Publisher = game.Publisher,
                Description = game.Description,
                Players = game.Players,
                PeakPlayers = game.PeakPlayers,
                ImagePath = game.ImagePath,
                VideoPath = game.VideoPath
            };


            return PartialView("_GameDeleteManagementPartial", ViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> PostDeletePartialToDB(int id)
        {
            var game = _context.Games.FindAsync(id).Result;
            if (game != null)
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();

            return GetIndexPartialView();
        }


        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }


        //GET: Admin/AdManagementIndex
        [HttpGet]
        public IActionResult AdManagementIndex()
        {
            return PartialView("_AdManagementIndexPartial", _context.Advertisements);
        }

        [HttpGet]
        public IActionResult AdManagementCreate()
        {
            return PartialView("_AdManagementCreatePartail");
        }

        public class AdManagementData
        {
            [Required]
            [MaxLength(100)]
            public string? Title { get; set; }

            [Required]
            [MaxLength(300)]
            public string? Url { get; set; }

            [Required]
            [MaxLength(300)]
            public string? ImagePath { get; set; }

            public string? Discription { get; set; }
        }

        [HttpPost]
        public string AdManagementCreate([FromBody] AdManagementData data)
        {
            if (ModelState.IsValid)
            {
                Advertisement ad = new Advertisement();
                ad.Title = data.Title;
                ad.Url = data.Url;
                ad.Discription = data.Discription;
                ad.ImagePath = data.ImagePath;
                _context.Add(ad);
                _context.SaveChangesAsync();
                return "ok";
            }
            else
            {
                return "fail";
            }

        }

        [HttpGet]
        public IActionResult AdManagementEdit(int id)
        {
            Console.WriteLine(id);

            var advertisement = _context.Advertisements.Find(id);
            if (advertisement == null)
            {
                return NotFound();
            }
            return PartialView("_AdManagementEditPartail", advertisement);
        }

        public class AdManagementDataAboutId
        {
            [Required]
            public int AdvertisementId { get; set; }

            [Required]
            [MaxLength(100)]
            public string? Title { get; set; }

            [Required]
            [MaxLength(300)]
            public string? Url { get; set; }

            [Required]
            [MaxLength(300)]
            public string? ImagePath { get; set; }

            public string? Discription { get; set; }
        }

        [HttpPost]
        public string AdManagementEdit([FromBody] AdManagementDataAboutId data)
        {
            if (ModelState.IsValid)
            {
                Advertisement? ad = _context.Advertisements.Find(data.AdvertisementId);
                if (ad != null)
                {
                    ad.Title = data.Title;
                    ad.Url = data.Url;
                    ad.Discription = data.Discription;
                    ad.ImagePath = data.ImagePath;
                    _context.SaveChangesAsync();
                    return "ok";
                }
                else
                {
                    return ("找不到");
                }
            }
            else
            {
                return "fail";
            }

        }

    }
}
