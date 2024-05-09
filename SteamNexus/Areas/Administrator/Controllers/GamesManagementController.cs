using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SteamNexus.Data;
using SteamNexus.Models;
using SteamNexus.ViewModels.Game;
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
        [ValidateAntiForgeryToken]
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

        //(API)Game-Price抓取寫回程式庫(更新)
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
                    Console.WriteLine(GameId);
                    await Task.Delay(1400);

                    allNum++;
                    var game = await _context.Games.FindAsync(GameId);
                    PriceHistory PriceHistory = new PriceHistory
                    {
                        GameId = GameId
                    };

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
                            PriceHistory.Price = 0;
                            try
                            {
                                //_context.Entry(game).State = EntityState.Modified;
                                _context.Update(game);
                                _context.PriceHistories.Add(PriceHistory);
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
                                    PriceHistory.Price = prices;
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
                                    PriceHistory.Price = final;
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
                                _context.PriceHistories.Add(PriceHistory);
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
                        try
                        {
                            PriceHistory.Price = (int)game.OriginalPrice;
                            //_context.Entry(game).State = EntityState.Modified;
                            _context.PriceHistories.Add(PriceHistory);
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

        //(API)拿取在線人數(新增)
        [HttpGet]
        public async Task<string> GetOnlineUsersDataToDB()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
            int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
            int allNum = 0;
            int errNum = 0;
            string player_count = "";

            for (int GameId = 10000; GameId <= num; GameId++)
            {
                allNum++;
                Console.WriteLine(GameId);
                //await Task.Delay(1400);
                var game = await _context.Games.FindAsync(GameId);
                if (game == null)
                {
                    continue;// 如果找不到遊戲，繼續下一個遊戲的處理
                }
                int? AppId = game.AppId;
                try
                {
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
                        PlayersHistory playersHistory = new PlayersHistory
                        {
                            GameId = GameId,
                            Players = 0
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
                        errNum++;
                        continue;
                    }
                }
                catch
                {
                    continue;
                }
            }
            return "總次數:" + allNum + "\nAPI找不到次數:" + errNum;
        }

        //(API)拿取評論(更新)
        [HttpGet]
        public async Task<string> GetNumberOfCommentsDataToDB()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
            int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
            int allNum = 0;
            int errNum = 0;
            string CommentsNum = "";
            string CommentsWord = "";
            for (int GameId = 10000; GameId <= num; GameId++)
            {
                Console.WriteLine(GameId);

                allNum++;
                var game = await _context.Games.FindAsync(GameId);
                if (game == null)
                {
                    continue;// 如果找不到遊戲，繼續下一個遊戲的處理
                }

                int? AppId = game.AppId;

                try
                {
                    HttpResponseMessage Response = await client.GetAsync($"https://store.steampowered.com/appreviews/{AppId}?purchase_type=all&language=all");
                    Response.EnsureSuccessStatusCode();
                    string data = await Response.Content.ReadAsStringAsync();
                    dynamic jsonData = JsonConvert.DeserializeObject(data);
                    string Comments = jsonData["review_score"];
                    Comments = Comments.Replace(",", "");
                    Match numberOfComments = Regex.Match(Comments, @"\d+");
                    Match wordOfComments = Regex.Match(Comments, @"壓倒性好評|極度好評|大多好評|褒貶不一|壓倒性負評|大多負評");
                    CommentsWord = wordOfComments.Value.Replace(">", "");
                    CommentsWord = CommentsWord.Replace("<", "");
                    CommentsNum = numberOfComments.Value;

                    int.TryParse(CommentsNum, out int CommentsNums);
                    game.Comment = CommentsWord;
                    game.CommentNum = CommentsNums;
                    try
                    {
                        //_context.Entry(game).State = EntityState.Modified;
                        _context.Update(game);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        Console.WriteLine($"{GameId}錯誤");
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

        //(API)拿取配備(新增)
        public async Task<string> GetDataToDB(bool isMinimumRequirement)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
            int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
            int allNum = 0;
            int APIerr = 0;
            int errNum = 0;
            string ReqData = "";

            for (int GameId = 10935; GameId <= num; GameId++)
            {
                await Task.Delay(1400);
                dynamic requirementDB;
                if (isMinimumRequirement)
                {
                    requirementDB = new MinimumRequirement();
                }
                else
                {
                    requirementDB = new RecommendedRequirement();
                }
                requirementDB.OriCpu = "Intel Core i5-12400";
                requirementDB.OriGpu = "Intel Iris Xe";
                requirementDB.OriRam = "4 GB 記憶體";

                requirementDB.GameId = GameId;
                Console.WriteLine($"{GameId}    {DateTime.Now}");

                allNum++;
                var game = await _context.Games.FindAsync(GameId);
                if (game == null)
                {
                    continue;// 如果找不到遊戲，繼續下一個遊戲的處理
                }

                int? AppId = game.AppId;

                try
                {
                    HttpResponseMessage Response = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={AppId}&l=zh-tw");
                    Response.EnsureSuccessStatusCode();
                    string data = await Response.Content.ReadAsStringAsync();
                    dynamic jsonData = JsonConvert.DeserializeObject(data);
                    dynamic gameInfo = jsonData[$"{AppId}"]["data"]["pc_requirements"];
                    ReqData = isMinimumRequirement ? gameInfo["minimum"] : gameInfo["recommended"];

                    var doc = new HtmlDocument();
                    doc.LoadHtml(ReqData);

                    var ulElement = doc.DocumentNode.SelectSingleNode("//ul[@class='bb_ul']");
                    var liElements = ulElement.SelectNodes("li");

                    foreach (var liElement in liElements)
                    {
                        string strongText = liElement.SelectSingleNode("strong")?.InnerText.Trim();
                        string liText = liElement.InnerText.Trim();
                        if (strongText == null)
                        {
                            continue;
                        }
                        switch (strongText)
                        {
                            case "作業系統:":
                            case "作業系統 *:":
                            case "OS *:":
                            case "OS:":
                            case "作業系統：":
                            case "Supported OS:":
                                requirementDB.OS = liText.Replace("作業系統:", "").Replace("\t", "").Replace("作業系統 *:", "").Replace("OS *:", "").Replace("OS:", "").Replace("作業系統：", "").Replace("Supported OS:", "").Trim();
                                break;
                            case "處理器:":
                            case "Processor:":
                            case "處理器：":
                                requirementDB.OriCpu = liText.Replace("處理器:", "").Replace("\t", "").Replace("Processor:", "").Replace("處理器：", "").Trim();
                                break;
                            case "記憶體:":
                            case "Memory:":
                            case "記憶體：":
                                string RAM = liText.Replace("記憶體:", "").Replace("\t", "").Replace("Memory:", "").Replace("記憶體：", "").Trim();
                                requirementDB.OriRam = RAM;
                                Match MatchRAM = Regex.Match(RAM, @"\d{1,2}");
                                int parsedRAM;
                                if (int.TryParse(MatchRAM.Value, out parsedRAM))
                                {
                                    requirementDB.RAM = parsedRAM;
                                }
                                else
                                {
                                    Console.WriteLine("記憶體大小格式不正確");
                                }
                                break;
                            case "顯示卡:":
                            case "Graphics:":
                            case "Video Card:":
                            case "顯示卡：":
                            case "Video:":
                                requirementDB.OriGpu = liText.Replace("顯示卡:", "").Replace("\t", "").Replace("Graphics:", "").Replace("Video Card:", "").Replace("顯示卡：", "").Replace("Video:", "").Trim();
                                break;
                            case "DirectX:":
                            case "DirectX®:":
                            case "DirectX 版本：":
                                requirementDB.DirectX = liText.Replace("DirectX:", "").Replace("\t", "").Replace("DirectX®:", "").Replace("DirectX 版本：", "").Trim();
                                break;
                            case "網路:":
                                requirementDB.Network = liText.Replace("網路:", "").Replace("\t", "").Trim();
                                break;
                            case "儲存空間:":
                            case "Hard Drive:":
                            case "Hard Disk Space:":
                            case "硬碟空間：":
                                requirementDB.Storage = liText.Replace("儲存空間:", "").Replace("\t", "").Replace("Hard Drive:", "").Replace("Hard Disk Space:", "").Replace("硬碟空間：", "").Replace("儲存空間:", "").Trim();
                                break;
                            case "備註:":
                            case "Additional:":
                            case "Display:":
                            case "Peripherals:":
                                requirementDB.Note = liText.Replace("備註:", "").Replace("\t", "").Replace("Additional:", "").Replace("Display:", "").Replace("Peripherals:", "").Trim();
                                break;
                            case "音效卡:":
                            case "Sound:":
                            case "音效卡：":
                                requirementDB.Audio = liText.Replace("音效卡:", "").Replace("Sound:", "").Replace("\t", "").Replace("音效卡：", "").Trim();
                                break;
                            default:
                                errNum++;
                                Console.WriteLine($"{strongText}錯誤");
                                break;
                        };

                    }
                    Console.WriteLine($"遊戲最低需求:CPU:{requirementDB.OriCpu},顯示卡:{requirementDB.OriGpu},記憶體:{requirementDB.OriRam},{requirementDB.RAM},作業系統:{requirementDB.OS},DirectX版本:{requirementDB.DirectX},網路需求:{requirementDB.Network},儲存空間:{requirementDB.Storage},音效卡:{requirementDB.Audio},備註:{requirementDB.Note}");
                    try
                    {
                        if (isMinimumRequirement)
                        {
                            _context.MinimumRequirements.Add((MinimumRequirement)requirementDB);
                        }
                        else
                        {
                            _context.RecommendedRequirements.Add((RecommendedRequirement)requirementDB);
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        return "傳回資料庫錯誤";
                    }
                }
                catch
                {
                    APIerr++;
                    Console.WriteLine("找不到API");
                    try
                    {
                        if (isMinimumRequirement)
                        {
                            _context.MinimumRequirements.Add((MinimumRequirement)requirementDB);
                        }
                        else
                        {
                            _context.RecommendedRequirements.Add((RecommendedRequirement)requirementDB);
                        }
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        return "傳回資料庫錯誤";
                    }
                };
            }
            return "總次數:" + allNum + "\nAPI找不到次數:" + APIerr + "\n欄位錯誤:" + errNum;
        }
    }
}
////(API)拿取最低配備(新增)
//public async Task<string> GetMinReqDataToDB()
//{
//    HttpClient client = new HttpClient();
//    client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
//    int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
//    int allNum = 0;
//    int APIerr = 0;
//    int errNum = 0;
//    string MinReqData = "";

//    for (int GameId = 11402; GameId <= 11600; GameId++)
//    {
//        MinimumRequirement MinReqDB = new MinimumRequirement()
//        {
//            OriCpu = "Intel Core i5-12400",
//            OriGpu = "Intel Iris Xe"
//        };

//        MinReqDB.GameId = GameId;
//        Console.WriteLine($"{GameId}    {DateTime.Now}");

//        allNum++;
//        var game = await _context.Games.FindAsync(GameId);
//        if (game == null)
//        {
//            continue;// 如果找不到遊戲，繼續下一個遊戲的處理
//        }

//        int? AppId = game.AppId;

//        try
//        {
//            HttpResponseMessage Response = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={AppId}&l=zh-tw");
//            Response.EnsureSuccessStatusCode();
//            string data = await Response.Content.ReadAsStringAsync();
//            dynamic jsonData = JsonConvert.DeserializeObject(data);
//            dynamic gameInfo = jsonData[$"{AppId}"]["data"]["pc_requirements"];
//            MinReqData = gameInfo["minimum"];
//            //RecReqData = gameInfo["recommended"];

//            var doc = new HtmlDocument();
//            doc.LoadHtml(MinReqData);

//            // 找到包含最低配備的 <ul> 元素
//            var ulElement = doc.DocumentNode.SelectSingleNode("//ul[@class='bb_ul']");

//            // 從 <ul> 元素中提取所有的 <strong> 元素
//            var liElements = ulElement.SelectNodes("li");

//            // 輸出提取到的最低配備項目
//            foreach (var liElement in liElements)
//            {
//                //抓取li中的strong用來給switch辨識欄位
//                string strongText = liElement.SelectSingleNode("strong")?.InnerText.Trim();
//                //.InnerText用來抓取文本內容,不會抓到HTML標籤
//                string liText = liElement.InnerText.Trim();
//                if (strongText == null)
//                {
//                    continue;
//                }
//                switch (strongText)
//                {
//                    case "作業系統:":
//                    case "作業系統 *:":
//                    case "OS *:":
//                    case "OS:":
//                    case "作業系統：":
//                    case "Supported OS:":
//                        MinReqDB.OS = liText.Replace("作業系統:", "").Replace("\t", "").Replace("作業系統 *:", "").Replace("OS *:", "").Replace("OS:", "").Replace("作業系統：", "").Replace("Supported OS:", "").Trim();
//                        break;
//                    case "處理器:":
//                    case "Processor:":
//                    case "處理器：":
//                        MinReqDB.OriCpu = liText.Replace("處理器:", "").Replace("\t", "").Replace("Processor:", "").Replace("處理器：", "").Trim();
//                        break;
//                    case "記憶體:":
//                    case "Memory:":
//                    case "記憶體：":
//                        string RAM = liText.Replace("記憶體:", "").Replace("\t", "").Replace("Memory:", "").Replace("記憶體：", "").Trim();
//                        MinReqDB.OriRam = RAM;
//                        Match MatchRAM = Regex.Match(RAM, @"\d{1,2}");

//                        int parsedRAM;
//                        if (int.TryParse(MatchRAM.Value, out parsedRAM))
//                        {
//                            // 解析成功，parsedRAM 變數存儲了轉換後的整數值
//                            MinReqDB.RAM = parsedRAM;
//                        }
//                        else
//                        {
//                            // 解析失敗，可能是因為記憶體大小的格式不正確
//                            Console.WriteLine("記憶體大小格式不正確");
//                        }
//                        break;
//                    case "顯示卡:":
//                    case "Graphics:":
//                    case "Video Card:":
//                    case "顯示卡：":
//                    case "Video:":
//                        MinReqDB.OriGpu = liText.Replace("顯示卡:", "").Replace("\t","").Replace("Graphics:", "").Replace("Video Card:", "").Replace("顯示卡：", "").Replace("Video:", "").Trim();
//                        break;
//                    case "DirectX:":
//                    case "DirectX®:":
//                    case "DirectX 版本：":
//                        MinReqDB.DirectX = liText.Replace("DirectX:", "").Replace("\t", "").Replace("DirectX®:", "").Replace("DirectX 版本：", "").Trim();
//                        break;
//                    case "網路:":
//                        MinReqDB.Network = liText.Replace("網路:", "").Replace("\t", "").Trim();
//                        break;
//                    case "儲存空間:":
//                    case "Hard Drive:":
//                    case "Hard Disk Space:":
//                    case "硬碟空間：":
//                        MinReqDB.Storage = liText.Replace("儲存空間:", "").Replace("\t", "").Replace("Hard Drive:", "").Replace("Hard Disk Space:", "").Replace("硬碟空間：", "").Replace("儲存空間:", "").Trim();
//                        break;
//                    case "備註:":
//                    case "Additional:":
//                    case "Display:":
//                    case "Peripherals:":
//                        MinReqDB.Note = liText.Replace("備註:", "").Replace("\t", "").Replace("Additional:", "").Replace("Display:", "").Replace("Peripherals:", "").Trim();
//                        break;
//                    case "音效卡:":
//                    case "Sound:":
//                    case "音效卡：":
//                        MinReqDB.Audio = liText.Replace("音效卡:", "").Replace("Sound:", "").Replace("\t", "").Replace("音效卡：", "").Trim();
//                        break;
//                    default:
//                        errNum++;
//                        Console.WriteLine($"{strongText}錯誤");
//                        break;
//                };

//            }
//            Console.WriteLine($"遊戲最低需求:CPU:{MinReqDB.OriCpu},顯示卡:{MinReqDB.OriGpu},記憶體:{MinReqDB.OriRam},{MinReqDB.RAM},作業系統:{MinReqDB.OS},DirectX版本:{MinReqDB.DirectX},網路需求:{MinReqDB.Network},儲存空間:{MinReqDB.Storage},音效卡:{MinReqDB.Audio},備註:{MinReqDB.Note}");
//            try
//            {
//                _context.MinimumRequirements.Add(MinReqDB);
//                await _context.SaveChangesAsync();
//            }
//            catch
//            {
//                return "傳回資料庫錯誤";
//            }
//        }
//        catch
//        {
//            APIerr++;
//            Console.WriteLine("找不到API");
//        };
//    }
//    return "總次數:" + allNum + "\nAPI找不到次數:" + APIerr+ "\n欄位錯誤:"+ errNum;
//}

////(API)拿取最高配備(新增)
//public async Task<string> GetRecReqDataToDB()
//{
//    HttpClient client = new HttpClient();
//    client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
//    int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
//    int allNum = 0;
//    int APIerr = 0;
//    int errNum = 0;
//    string RecReqData = "";

//    for (int GameId = 10000; GameId <= 10200; GameId++)
//    {
//        RecommendedRequirement RecReqDB = new RecommendedRequirement()
//        {
//            OriCpu = "Intel Core i5-12400",
//            OriGpu = "Intel Iris Xe"
//        };

//        RecReqDB.GameId = GameId;
//        Console.WriteLine($"{GameId}+{DateTime.Now}");

//        allNum++;
//        var game = await _context.Games.FindAsync(GameId);
//        if (game == null)
//        {
//            continue;// 如果找不到遊戲，繼續下一個遊戲的處理
//        }

//        int? AppId = game.AppId;

//        try
//        {
//            HttpResponseMessage Response = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={AppId}&l=zh-tw");
//            Response.EnsureSuccessStatusCode();
//            string data = await Response.Content.ReadAsStringAsync();
//            dynamic jsonData = JsonConvert.DeserializeObject(data);
//            dynamic gameInfo = jsonData[$"{AppId}"]["data"]["pc_requirements"];
//            RecReqData = gameInfo["recommended"];
//            //RecReqData = gameInfo["recommended"];

//            var doc = new HtmlDocument();
//            doc.LoadHtml(RecReqData);

//            // 找到包含最低配備的 <ul> 元素
//            var ulElement = doc.DocumentNode.SelectSingleNode("//ul[@class='bb_ul']");

//            // 從 <ul> 元素中提取所有的 <strong> 元素
//            var liElements = ulElement.SelectNodes("li");

//            // 輸出提取到的最低配備項目
//            foreach (var liElement in liElements)
//            {
//                //抓取li中的strong用來給switch辨識欄位
//                string strongText = liElement.SelectSingleNode("strong")?.InnerText.Trim();
//                //.InnerText用來抓取文本內容,不會抓到HTML標籤
//                string liText = liElement.InnerText.Trim();
//                if (strongText == null)
//                {
//                    continue;
//                }
//                switch (strongText)
//                {
//                    case "作業系統:":
//                    case "作業系統 *:":
//                    case "OS *:":
//                    case "OS:":
//                    case "作業系統：":
//                    case "Supported OS:":
//                        RecReqDB.OS = liText.Replace("作業系統:", "").Replace("\t", "").Replace("作業系統 *:", "").Replace("OS *:", "").Replace("OS:", "").Replace("作業系統：", "").Replace("Supported OS:", "").Trim();
//                        break;
//                    case "處理器:":
//                    case "Processor:":
//                    case "處理器：":
//                        RecReqDB.OriCpu = liText.Replace("處理器:", "").Replace("\t", "").Replace("Processor:", "").Replace("處理器：", "").Trim();
//                        break;
//                    case "記憶體:":
//                    case "Memory:":
//                    case "記憶體：":
//                        string RAM = liText.Replace("記憶體:", "").Replace("\t", "").Replace("Memory:", "").Replace("記憶體：", "").Trim();
//                        RecReqDB.OriRam = RAM;
//                        Match MatchRAM = Regex.Match(RAM, @"\d{1,2}");

//                        int parsedRAM;
//                        if (int.TryParse(MatchRAM.Value, out parsedRAM))
//                        {
//                            // 解析成功，parsedRAM 變數存儲了轉換後的整數值
//                            RecReqDB.RAM = parsedRAM;
//                        }
//                        else
//                        {
//                            // 解析失敗，可能是因為記憶體大小的格式不正確
//                            Console.WriteLine("記憶體大小格式不正確");
//                        }
//                        break;
//                    case "顯示卡:":
//                    case "Graphics:":
//                    case "Video Card:":
//                    case "顯示卡：":
//                    case "Video:":
//                        RecReqDB.OriGpu = liText.Replace("顯示卡:", "").Replace("\t", "").Replace("Graphics:", "").Replace("Video Card:", "").Replace("顯示卡：", "").Replace("Video:", "").Trim();
//                        break;
//                    case "DirectX:":
//                    case "DirectX®:":
//                    case "DirectX 版本：":
//                        RecReqDB.DirectX = liText.Replace("DirectX:", "").Replace("\t", "").Replace("DirectX®:", "").Replace("DirectX 版本：", "").Trim();
//                        break;
//                    case "網路:":
//                        RecReqDB.Network = liText.Replace("網路:", "").Replace("\t", "").Trim();
//                        break;
//                    case "儲存空間:":
//                    case "Hard Drive:":
//                    case "Hard Disk Space:":
//                    case "硬碟空間：":
//                        RecReqDB.Storage = liText.Replace("儲存空間:", "").Replace("\t", "").Replace("Hard Drive:", "").Replace("Hard Disk Space:", "").Replace("硬碟空間：", "").Replace("儲存空間:", "").Trim();
//                        break;
//                    case "備註:":
//                    case "Additional:":
//                    case "Display:":
//                    case "Peripherals:":
//                        RecReqDB.Note = liText.Replace("備註:", "").Replace("\t", "").Replace("Additional:", "").Replace("Display:", "").Replace("Peripherals:", "").Trim();
//                        break;
//                    case "音效卡:":
//                    case "Sound:":
//                    case "音效卡：":
//                        RecReqDB.Audio = liText.Replace("音效卡:", "").Replace("Sound:", "").Replace("\t", "").Replace("音效卡：", "").Trim();
//                        break;
//                    default:
//                        errNum++;
//                        Console.WriteLine($"{strongText}錯誤");
//                        break;
//                };

//            }
//            Console.WriteLine($"遊戲最低需求:CPU:{RecReqDB.OriCpu},顯示卡:{RecReqDB.OriGpu},記憶體:{RecReqDB.OriRam},{RecReqDB.RAM},作業系統:{RecReqDB.OS},DirectX版本:{RecReqDB.DirectX},網路需求:{RecReqDB.Network},儲存空間:{RecReqDB.Storage},音效卡:{RecReqDB.Audio},備註:{RecReqDB.Note}");
//            try
//            {
//                _context.RecommendedRequirements.Add(RecReqDB);
//                await _context.SaveChangesAsync();
//            }
//            catch
//            {
//                return "傳回資料庫錯誤";
//            }
//        }
//        catch
//        {
//            APIerr++;
//            Console.WriteLine("找不到API");
//        };
//    }
//    return "總次數:" + allNum + "\nAPI找不到次數:" + APIerr + "\n欄位錯誤:" + errNum;
//}