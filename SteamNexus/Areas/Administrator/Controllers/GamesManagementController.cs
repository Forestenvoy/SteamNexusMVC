using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.ViewModels.Game;
using System;
using System.Text.Json;
using System.Text.RegularExpressions;

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
        public async Task<IActionResult> PostCreatPartialToDB(CreateViewModel Create)
        {
            if (ModelState.IsValid)
            {
                Game game = new Game
                {
                    AppId = Create.AppId,
                    Name = Create.Name,
                    OriginalPrice = Create.OriginalPrice,
                    AgeRating = Create.AgeRating,
                    ReleaseDate = Create.ReleaseDate,
                    Publisher = Create.Publisher,
                    Description = Create.Description,
                    ImagePath = Create.ImagePath,
                    VideoPath = Create.VideoPath,
                };

                try
                {
                    _context.Games.Add(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("錯誤");
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


            return PartialView("_GameCreateManagementPartial", Create);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostEditPartialToDB(EditViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                var game = _context.Games.FindAsync(ViewModel.GameId).Result;
                game.GameId = ViewModel.GameId;
                game.AppId = ViewModel.AppId;
                game.Name = ViewModel.Name;
                game.OriginalPrice = ViewModel.OriginalPrice;
                game.AgeRating = ViewModel.AgeRating;
                game.ReleaseDate = ViewModel.ReleaseDate;
                game.Publisher = ViewModel.Publisher;
                game.Description = ViewModel.Description;
                game.ImagePath = ViewModel.ImagePath;
                game.VideoPath = ViewModel.VideoPath;

                try
                {
                    //_context.Entry(game).State = EntityState.Modified;
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    Console.WriteLine("錯誤");
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
            return PartialView("_GameEditManagementPartial", ViewModel);
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

        //(API)Game-Price抓取寫回程式庫
        [HttpGet]
        public async Task<string> GetGamePriceDataToDB()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
            int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
            int allNum = 0;
            int errNum = 0;
            int priceErrNum = 0;
            try
            {
                for (int GameId = 10000; GameId <= num; GameId++)
                {
                    await Task.Delay(1400);

                    allNum++;
                    var game = await _context.Games.FindAsync(GameId);
                    if (game == null)
                    {
                        continue;// 如果找不到遊戲，繼續下一個遊戲的處理
                    }

                    int? AppId = game.AppId;

                    HttpResponseMessage Response = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={AppId}&l=zh-tw");
                    Response.EnsureSuccessStatusCode();

                    string data = await Response.Content.ReadAsStringAsync();
                    dynamic jsonData = JsonConvert.DeserializeObject(data);
                    try
                    {
                        dynamic gameInfo = jsonData[$"{AppId}"]["data"];
                        //string Id = gameInfo["steam_appid"];
                        if (gameInfo["is_free"] == true)
                        {
                            game.OriginalPrice = 0;
                            game.CurrentPrice = 0;
                            try
                            {
                                //_context.Entry(game).State = EntityState.Modified;
                                _context.Update(game);
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                Console.WriteLine("錯誤");
                                if (!GameExists(game.GameId))
                                {
                                    return "錯誤";
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        else
                        {
                            dynamic price = gameInfo["price_overview"];
                            string initial_formatted = price["initial_formatted"];
                            string finalFormatted = price["final_formatted"];
                            if (initial_formatted == "")
                            {
                                //將字串中的,拿掉以便match中沒有斷句
                                finalFormatted = finalFormatted.Replace(",", "");
                                //使用正規表達式去擷取數字
                                Match match = Regex.Match(finalFormatted, @"\d+");

                                if (match.Success)
                                {
                                    string finalPrice = match.Value;
                                    int.TryParse(finalPrice, out int prices);
                                    game.OriginalPrice = prices;
                                    game.CurrentPrice = prices;
                                }
                                else
                                {
                                    priceErrNum++;
                                }
                            }
                            else
                            {
                                //將字串中的,拿掉以便match中沒有斷句
                                initial_formatted = initial_formatted.Replace(",", "");
                                finalFormatted = finalFormatted.Replace(",", "");
                                //使用正規表達式去擷取數字
                                Match initialmatch = Regex.Match(initial_formatted, @"\d+");
                                Match finalmatch = Regex.Match(finalFormatted, @"\d+");

                                if (initialmatch.Success && finalmatch.Success)
                                {
                                    string initialPrice = initialmatch.Value;
                                    string finalPrice = finalmatch.Value;
                                    int.TryParse(initialPrice, out int initial);
                                    int.TryParse(finalPrice, out int final);
                                    game.OriginalPrice = initial;
                                    game.CurrentPrice = final;
                                }
                                else
                                {
                                    priceErrNum++;
                                }
                                
                            }
                            try
                            {
                                //_context.Entry(game).State = EntityState.Modified;
                                _context.Update(game);
                                await _context.SaveChangesAsync();
                            }
                            catch (DbUpdateConcurrencyException)
                            {
                                Console.WriteLine("錯誤");
                                if (!GameExists(game.GameId))
                                {
                                    return "錯誤";
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                    catch
                    {
                        errNum++;
                        continue;
                    }
                }
            }
            catch
            {
                return "總次數:" + allNum;
            }

            return "總次數:" + allNum + "\nAPI找不到次數:" + errNum + "\n價錢錯誤次數:" + priceErrNum;
        }


        //拿取在線人數(更新)
        [HttpGet]
        public async Task<string> GetOnlineUsersDataToDB()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
            int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
            int allNum = 0;
            int errNum = 0;
            string player_count = "";

            for (int GameId = 10000; GameId <= 10200; GameId++)
            {
                allNum++;
                Console.WriteLine(GameId);
                await Task.Delay(1400);
                var game = await _context.Games.FindAsync(GameId);
                int? AppId = game.AppId;
                if (game == null)
                {
                    continue;// 如果找不到遊戲，繼續下一個遊戲的處理
                }
                HttpResponseMessage Response = await client.GetAsync($"https://api.steampowered.com/ISteamUserStats/GetNumberOfCurrentPlayers/v1/?format=json&appid={AppId}");
                Response.EnsureSuccessStatusCode();

                string data = await Response.Content.ReadAsStringAsync();
                dynamic jsonData = JsonConvert.DeserializeObject(data);
                try
                {
                    player_count = jsonData["response"]["player_count"];
                    int.TryParse(player_count, out int players);

                    PlayersHistory playersHistory = new PlayersHistory
                    {
                        GameId = GameId,
                        Players = players
                    };

                    try
                    {
                        _context.PlayersHistories.Add(playersHistory);
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        return "傳回資料庫錯誤";
                    }
                }
                catch
                {
                    errNum++;
                    continue;
                }
            }
            return "總次數:" + allNum + "\nAPI找不到次數:" + errNum;
        }
    }
}
