using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.ViewModels.Game;

namespace SteamNexus.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class GamesManagementController : Controller
    {
        private readonly SteamNexusDbContext _context;

        public GamesManagementController(SteamNexusDbContext context)
        {
            _context = context;
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
            return PartialView("_GameIndexManagementPartial", steamNexusDbContext);
        }

        //GameDataTable設定
        [HttpGet]
        public async Task<JsonResult> IndexJson()
        {
            return Json(_context.Games);
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
                GameId = id,
                AppId = game.AppId,
                Name = game.Name,
                OriginalPrice = game.OriginalPrice,
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
            return PartialView("_GameEditManagementPartial", ViewModel);
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
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
