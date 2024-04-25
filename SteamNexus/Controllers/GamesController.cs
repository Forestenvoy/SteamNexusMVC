using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.ViewModels.Game;


namespace SteamNexus.Controllers
{
    public class GamesController : Controller
    {
        private readonly SteamNexusDbContext _context;

        public GamesController(SteamNexusDbContext context)
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

        //    // GET: Games
        //    public async Task<IActionResult> Index()
        //    {
        //        var steamNexusDbContext = _context.Games.Include(g => g.MinReq).Include(g => g.RecReq);
        //        return View(steamNexusDbContext);
        //    }

        //    private async Task<DetailsViewModel> GetViewModel(int id)
        //    {
        //        var game = await _context.Games
        //            .Include(g => g.MinReq)
        //            .Include(g => g.RecReq)
        //            .FirstOrDefaultAsync(m => m.GameId == id);

        //        if (game == null)
        //        {
        //            return null; // 或者根據您的需求返回其他值
        //        }

        //        return new DetailsViewModel
        //        {
        //            GameId = id,
        //            AppId = game.AppId,
        //            Name = game.Name,
        //            OriginalPrice = game.OriginalPrice,
        //            CurrentPrice = game.CurrentPrice,
        //            LowestPrice = game.LowestPrice,
        //            AgeRating = game.AgeRating,
        //            Comment = game.Comment,
        //            CommentNum = game.CommentNum,
        //            ReleaseDate = game.ReleaseDate,
        //            Publisher = game.Publisher,
        //            Description = game.Description,
        //            Players = game.Players,
        //            PeakPlayers = game.PeakPlayers,
        //            ImagePath = game.ImagePath,
        //            VideoPath = game.VideoPath
        //        };
        //    }

        //    // GET: Games/Details/5
        //    public async Task<IActionResult> Details(int id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var game = await _context.Games
        //            .Include(g => g.MinReq)
        //            .Include(g => g.RecReq)
        //            .FirstOrDefaultAsync(m => m.GameId == id);
        //        if (game == null)
        //        {
        //            return NotFound();
        //        }

        //        var viewModel = await GetViewModel(id);

        //        return View(viewModel);
        //    }

        //    // GET: Games/Create
        //    public IActionResult Create()
        //    {
        //        //ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId");
        //        //ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId");
        //        return View();
        //    }

        //    // POST: Games/Create
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Create([Bind("AppId,Name,OriginalPrice,AgeRating,ReleaseDate,Publisher,Description,ImagePath,VideoPath")] CreateViewModel Create)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            Game game = new Game{
        //                AppId = Create.AppId,
        //                Name = Create.Name,
        //                OriginalPrice = Create.OriginalPrice,
        //                AgeRating = Create.AgeRating,
        //                ReleaseDate= Create.ReleaseDate,
        //                Publisher= Create.Publisher,
        //                Description = Create.Description,
        //                ImagePath = Create.ImagePath,
        //                VideoPath = Create.VideoPath,
        //            };

        //            _context.Games.Add(game);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return View(Create);
        //    }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
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
            //ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId", game.MinReqId);
            //ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId", game.RecReqId);
            return View(ViewModel);
        }

        //    // POST: Games/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(int id, [Bind("GameId,MinReqId,RecReqId,AppId,Name,OriginalPrice,CurrentPrice,LowestPrice,AgeRating,Comment,CommentNum,ReleaseDate,Publisher,Description,Players,PeakPlayers,ImagePath,VideoPath")] Game game)
        //    {
        //        if (id != game.GameId)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(game);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!GameExists(game.GameId))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId", game.MinReqId);
        //        ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId", game.RecReqId);
        //        return View(game);
        //    }

        //    // GET: Games/Delete/5
        //    public async Task<IActionResult> Delete(int id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var viewModel = await GetViewModel(id);

        //        //var game = await _context.Games
        //        //    .Include(g => g.MinReq)
        //        //    .Include(g => g.RecReq)
        //        //    .FirstOrDefaultAsync(m => m.GameId == id);
        //        //if (game == null)
        //        //{
        //        //    return NotFound();
        //        //}

        //        return View(viewModel);
        //    }

        //    // POST: Games/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(int id)
        //    {
        //        var game = await _context.Games.FindAsync(id);
        //        if (game != null)
        //        {
        //            _context.Games.Remove(game);
        //        }

        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool GameExists(int id)
        //    {
        //        return _context.Games.Any(e => e.GameId == id);
        //    }



    }


}
