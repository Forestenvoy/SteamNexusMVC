using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus_Server.Data;

namespace SteamNexus_Server.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        // Dependency Injection
        private readonly SteamNexusDbContext _context;
        public SearchController(SteamNexusDbContext context)
        {
            _context = context;
        }

        public class GameData
        {
            public int GameId { get; set; }
            public string Name { get; set; }
            public string ImagePath {  get; set; }
        }

        [HttpGet("GetKeywordSearch")]
        public async Task<IActionResult> GetKeywordSearch(string keyword)
        {
            var GameData = new GameData();

            var results =  _context.Games.Where(i => i.Name.Contains(keyword, System.StringComparison.OrdinalIgnoreCase));

            

            return Ok(GameData);
        }

        


    }
}
