using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.Models;

namespace SteamNexus.Controllers
{
    public class GamesController : Controller
    {
        private readonly SteamNexusDbContext _context;

        public GamesController(SteamNexusDbContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index()
        {
            var steamNexusDbContext = _context.Games.Include(g => g.MinReq).Include(g => g.RecReq);
            return View(await steamNexusDbContext.ToListAsync());
        }

        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.MinReq)
                .Include(g => g.RecReq)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create()
        {
            ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId");
            ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId");
            return View();
        }

        // POST: Games/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,MinReqId,RecReqId,AppId,Name,OriginalPrice,CurrentPrice,LowestPrice,AgeRating,Comment,CommentNum,ReleaseDate,Publisher,Description,Players,PeakPlayers,ImagePath,VideoPath")] Game game)
        {
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId", game.MinReqId);
            ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId", game.RecReqId);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }
            ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId", game.MinReqId);
            ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId", game.RecReqId);
            return View(game);
        }

        // POST: Games/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,MinReqId,RecReqId,AppId,Name,OriginalPrice,CurrentPrice,LowestPrice,AgeRating,Comment,CommentNum,ReleaseDate,Publisher,Description,Players,PeakPlayers,ImagePath,VideoPath")] Game game)
        {
            if (id != game.GameId)
            {
                return NotFound();
            }

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
                return RedirectToAction(nameof(Index));
            }
            ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId", game.MinReqId);
            ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId", game.RecReqId);
            return View(game);
        }

        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.MinReq)
                .Include(g => g.RecReq)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game != null)
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }

       
    }
}
