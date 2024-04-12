using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamNexus.Data;
using SteamNexus.ViewModels;
using System.Diagnostics;

namespace SteamNexus.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _application;

        public HomeController(ApplicationDbContext application)
        {
            _application = application;
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //GET /Home/Identity
        public IActionResult Identity()
        {
            //統計註冊人數
            var usersCount = _application.Users.Count();
            ViewData["UsersCount"] = usersCount;
            return View(_application.Users);
        }

        // GET: Home/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Home/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,PhoneNumber,Email,Password,ConfirmPassword,Gender,Birthday,CPUId,GPUId,RAM,Power")] ApplicationUser ApplicationUser)
        {
            if (ModelState.IsValid)
            {
                _application.Add(ApplicationUser);
                await _application.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ApplicationUser);
        }

        public async Task<IActionResult> Edit(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var ApplicationUser = await _application.ApplicationUser.FindAsync(Id);
            if (ApplicationUser == null)
            {
                return NotFound();
            }
            return View(ApplicationUser);
        }

        // POST: Home/Edit/
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, [Bind("Name,PhoneNumber,Email,Gender,Birthday,CPUId,GPUId,RAM,Power")] ApplicationUser ApplicationUser)
        {
            if (Id != ApplicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _application.Update(ApplicationUser);
                    await _application.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(ApplicationUser.Id))
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
            return View(ApplicationUser);
        }


        [Route("/Home/Details/{Id?}")]
        public async Task<IActionResult> Details(string? Id)
        {
            //找不到ID時，return NotFound
            if (Id == null)
            {
                return NotFound();
            }

            var member = await _application.ApplicationUser.FirstOrDefaultAsync(m => m.Id == Id);

            //找出ID是空值，return NotFound
            if (member == null)
            {
                return NotFound();
            }

            //如果有值就顯示
            return View(member);
        }

        // GET: Home/Delete/
        public async Task<IActionResult> Delete(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var advertisement = await _application.ApplicationUser
                .FirstOrDefaultAsync(m => m.Id == Id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        // POST: Home/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            var ApplicationUser = await _application.ApplicationUser.FindAsync(Id);
            if (ApplicationUser != null)
            {
                _application.ApplicationUser.Remove(ApplicationUser);
            }

            await _application.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string Id)
        {
            return _application.ApplicationUser.Any(e => e.Id == Id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
