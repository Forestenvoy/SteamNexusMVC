using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using SteamNexus_Server.ViewModels.Game;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Cors;
using System.Text;
using System.Linq;


namespace SteamNexus.Areas.Administrator.Controllers
{
    // 套用 CORS 策略
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class GamesManagementController : Controller
    {
        private readonly SteamNexusDbContext _context;
        public static int progressNum = 0;
        public static bool isPriceDataUsing = false;

        public GamesManagementController(SteamNexusDbContext context)
        {
            _context = context;
        }
       
        //GameDataTable設定
        [HttpGet("IndexJson")]
        public JsonResult IndexJson()
        {
            return Json(_context.Games);
        }
      
        [HttpPost("PostCreatPartialToDB")]
        //[ValidateAntiForgeryToken]
        public async Task<string> PostCreatPartialToDB(CreateViewModel Create)
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
                        return "傳送資料庫失敗";
                    }
                    else
                    {
                        throw;
                    }
                }
                return "成功";
            }


            return "傳送資料庫失敗";
        }

        [HttpGet("GetEditJSON")]
        public async Task<JsonResult> GetEditJSON(int id)
        {
            var game = _context.Games.FindAsync(id).Result;

            return Json(game);
        }

        [HttpPost("PostEditPartialToDB")]
        //[ValidateAntiForgeryToken]
        public async Task<string> PostEditPartialToDB(EditViewModel ViewModel)
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
                        return "傳送資料庫失敗";
                    }
                    else
                    {
                        throw;
                    }
                }
                return "成功";
            }
            //ViewData["MinReqId"] = new SelectList(_context.MinimumRequirements, "MinReqId", "MinReqId", game.MinReqId);
            //ViewData["RecReqId"] = new SelectList(_context.RecommendedRequirements, "RecReqId", "RecReqId", game.RecReqId);
            return "失敗";
        }

        [HttpGet("PostDeletePartialToDB")]
        public async Task<string> PostDeletePartialToDB(int id)
        {
            var game = _context.Games.FindAsync(id).Result;
            if (game != null)
            {
                _context.Games.Remove(game);
            }

            await _context.SaveChangesAsync();

            return "已刪除";
        }
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }

        //(API)Game-Price抓取寫回程式庫(更新)
        [HttpGet("GetGamePriceDataToDB")]
        public async Task<string> GetGamePriceDataToDB()
        {
            if (isPriceDataUsing== false)
            {
                isPriceDataUsing= true;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
                int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
                int allNum = 0;
                int errNum = 0;
                int priceErrNum = 0;

                for (int GameId = 10000; GameId <= num; GameId++)
                {
                    int testNum = GameId - 9999;
                    double testprogressNum = Math.Round(((double)testNum / 1198) * 100, 2); ;
                    progressNum = (int)testprogressNum;
                    Console.WriteLine(testprogressNum);
                    Console.WriteLine(progressNum);
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
                                //await _context.SaveChangesAsync();
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
                                //await _context.SaveChangesAsync();
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
                            //await _context.SaveChangesAsync();
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
                isPriceDataUsing = false;

                return "總次數:" + allNum + "\nAPI找不到次數:" + errNum + "\n價錢錯誤次數:" + priceErrNum;
            }
            else
            {
                return "有人正在使用價格抓取";
            }
        }

        //(前台)進度條
        [HttpGet("GamePriceProgress")]
        public IActionResult GamePriceProgress()
        {
            string progressData = "";
            progressData += $"id={Guid.NewGuid()}\n";
            progressData += "retry:1000\n";
            progressData += $"data:{progressNum}\n\n";
            return Content($"{progressData}", "text/event-stream", Encoding.UTF8);
        }

        //(API)拿取在線人數(新增)
        [HttpGet("GetOnlineUsersDataToDB")]
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

                    continue;
                }
            }
            return "總次數:" + allNum + "\nAPI找不到次數:" + errNum;
        }

        //(API)拿取評論(更新)
        [HttpGet("GetNumberOfCommentsDataToDB")]
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
        [HttpGet("GetDataToDB")]
        public async Task<string> GetDataToDB(bool isMinimumRequirement)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
            int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
            int allNum = 0;
            int APIerr = 0;
            int errNum = 0;
            string ReqData = "";

            for (int GameId = 10000; GameId <= 11977; GameId++)
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

        [HttpGet("GetLineChartData")]
        public async Task<JsonResult> GetLineChartData(int id)
        {
            // 获取相关的 PriceHistories 集合
            var priceHistorie = await _context.PriceHistories
                .Where(ph => ph.GameId == id).OrderBy(ph => ph.Date).Select(ph => new { ph.Date, ph.Price })  // 替换为实际条件
                .ToListAsync();      



            return Json(priceHistorie);
        }
    }
}