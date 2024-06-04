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
using SteamNexus_Server.Dtos.Game;
using SteamNexus_Server.Services;
using System.Timers;
using System;
using Microsoft.AspNetCore.Authorization;


namespace SteamNexus.Areas.Administrator.Controllers
{
    // 套用 CORS 策略
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class GamesManagementController : Controller
    {
        private readonly ScheduledTaskService _scheduledTaskService;
        private readonly SteamNexusDbContext _context;
        private readonly GameTimer _GamePriceToDB;
        static System.Timers.Timer _timer;
        //public static int progressNum = 0;
        public static bool isPriceDataUsing = false;
        public static bool isOnlineUserUsing = false;
        public static bool isNumberOfCommentsDataUsing = false;

        public GamesManagementController(SteamNexusDbContext context, GameTimer GamePriceToDB, ScheduledTaskService scheduledTaskService)
        {
            _context = context;
            _GamePriceToDB = GamePriceToDB;
            _scheduledTaskService = scheduledTaskService ?? throw new ArgumentNullException(nameof(scheduledTaskService));
        }

        //GameDataTable設定
        [HttpGet("IndexJson")]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> IndexJson()
        {
            return Json(_context.Games);
        }

        //拿取全部遊戲的Tag
        [HttpGet("AllGameTagData")]
        public async Task<JsonResult> AllGameTagData()
        {
            var tagData = from a in _context.TagGroups
                          join b in _context.Tags on a.TagId equals b.TagId
                          select new
                          {
                              gameId = a.GameId,
                              name = b.Name
                          };

            // 分組並轉換結果
            var groupedData = tagData
                              .GroupBy(t => t.gameId)
                              .Select(g => new
                              {
                                  gameId = g.Key,
                                  tags = g.Select(t => new { Name = t.name }).ToList()
                              })
                      .ToList();

            return Json(groupedData);
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<string> PostDeletePartialToDB(int id)
        {
            var historiesToDelete = _context.PlayersHistories.Where(ph => ph.GameId == id);
            _context.PlayersHistories.RemoveRange(historiesToDelete);

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
        [Authorize(Roles = "Admin")]
        public async Task<string> GetGamePriceDataToDB()
        {
            if (isPriceDataUsing == false)
            {
                isPriceDataUsing = true;
                try
                {
                    _scheduledTaskService.StartPriceDailyTimer();

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return "500 Internal server error";
                }
                Console.WriteLine("價格計時器開啟");
                return $"{isPriceDataUsing}";
            }
            else
            {
                isPriceDataUsing = false;
                try
                {
                    _scheduledTaskService.StopPriceDailyTimer();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return "500 Internal server error";
                }
                Console.WriteLine("價格計時器關閉");
                return $"{isPriceDataUsing}";
            }



        }

        //(前台)進度條
        [HttpGet("GamePriceProgress")]
        public IActionResult GamePriceProgress()
        {
            string progressData = "";
            progressData += $"id={Guid.NewGuid()}\n";
            progressData += "retry:1000\n";
            progressData += $"data:{GameTimer.progressNum}\n\n";
            return Content($"{progressData}", "text/event-stream", Encoding.UTF8);
        }

        ////(後台)拿取是否開啟
        [HttpGet("isTimeOpen")]
        public async Task<JsonResult> isTimeOpen()
        {
            string[] array;
            array = [ $"{isPriceDataUsing}", $"{isOnlineUserUsing}", $"{isNumberOfCommentsDataUsing}" ];
            return Json(array);
        }

        //(API)拿取在線人數(新增)
        [HttpGet("GetOnlineUsersDataToDB")]
        [Authorize(Roles = "Admin")]
        public async Task<string> GetOnlineUsersDataToDB()
        {
            if (isOnlineUserUsing == false)
            {
                isOnlineUserUsing = true;
                try
                {
                    _scheduledTaskService.StartPeopleTimer();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return "500 Internal server error";
                }
                Console.WriteLine("在線人數計時器開啟");
                return $"{isOnlineUserUsing}";
            }
            else
            {
                isOnlineUserUsing = false;
                try
                {
                    _scheduledTaskService.StoppeopleHalfHourTimer();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return "500 Internal server error";
                }
                Console.WriteLine("在線人數計時器關閉");
                return $"{isOnlineUserUsing}";
            }
        }

        //(API)拿取評論(更新)
        [HttpGet("GetNumberOfCommentsDataToDB")]
        [Authorize(Roles = "Admin")]
        public async Task<string> GetNumberOfCommentsDataToDB()
        {
            if (isNumberOfCommentsDataUsing == false)
            {
                isNumberOfCommentsDataUsing = true;
                try
                {
                    _scheduledTaskService.StartNumberOfCommentsTimer();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return "500 Internal server error";
                }
                Console.WriteLine("評論計時器關閉");
                return $"{isNumberOfCommentsDataUsing}";
            }
            else
            {
                isNumberOfCommentsDataUsing = false;
                try
                {
                    _scheduledTaskService.StoptNumberOfCommentsTimer();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return "500 Internal server error";
                }
                Console.WriteLine("評論計時器關閉");
                return $"{isNumberOfCommentsDataUsing}";
            }
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

        //(前台)拿取價格
        [HttpGet("GetLineChartData")]
        public async Task<JsonResult> GetLineChartData(int id)
        {
            // 获取相关的 PriceHistories 集合
            var priceHistorie = await _context.PriceHistories
                .Where(ph => ph.GameId == id).OrderBy(ph => ph.Date).Select(ph => new { ph.Date, ph.Price })  // 替换为实际条件
                .ToListAsync();

            return Json(priceHistorie);
        }

        //(前台)拿取tag
        [HttpGet("GetTagGroup")]
        public async Task<JsonResult> GetTagGroup(int id)
        {
            // 获取相关的 PriceHistories 集合
            var TagGroupFind = await _context.TagGroups
                               .Where(ph => ph.GameId == id)
                               .Select(ph => new { ph.TagId })
                               .ToListAsync();

            var tagIds = TagGroupFind.Select(t => t.TagId).ToList();

            var tags = await _context.Tags
                .Where(t => tagIds.Contains(t.TagId))
                .Select(t => new { t.Name })
                .ToListAsync();

            var tagNames = tags.Select(t => t.Name).ToList();

            return Json(tagNames);
        }

        //(前台)拿取Language中文
        [HttpGet("GetLanguageGroup")]
        public async Task<JsonResult> GetLanguageGroup(int id)
        {
            // 获取相关的 PriceHistories 集合
            var LanguageGroupFind = await _context.GameLanguages
                               .Where(ph => ph.GameId == id)
                               .Select(ph => new { ph.LanguageId, ph.Support })
                               .ToListAsync();

            var LanguageIds = LanguageGroupFind.Select(t => t.LanguageId).ToList();

            var LanguagesGroup = await _context.Languages
                .Where(t => LanguageIds.Contains(t.LanguageId))
                .Select(t => new
                {
                    t.LanguageId,
                    t.Name
                })
                .ToListAsync();

            var result = from lg in LanguageGroupFind
                         join lang in LanguagesGroup on lg.LanguageId equals lang.LanguageId
                         select new
                         {
                             lang.Name,
                             lg.Support
                         };

            //var LanguageNames = Languages.Select(t => t.Name).ToList();

            return Json(result);
        }

        //(前台)拿取最低配備
        [HttpGet("GetMinReqData")]
        public async Task<JsonResult> GetMinReqData(int id)
        {
            var MinReqData = await _context.MinimumRequirements.FindAsync(id);

            return Json(MinReqData);
        }
        //(前台)拿取最高配備
        [HttpGet("GetRecReqData")]
        public async Task<JsonResult> GetRecReqData(int id)
        {
            var RecReqData = await _context.RecommendedRequirements.FindAsync(id);

            return Json(RecReqData);
        }
        //(前台)拿取相關遊戲
        [HttpGet("GetGameTagSameData")]
        public async Task<JsonResult> GetGameTagSameData(int id)
        {
            // 獲取目標遊戲的所有 TagId
            var targetTagIds = await _context.TagGroups
                                             .Where(ph => ph.GameId == id)
                                             .Select(ph => ph.TagId)
                                             .ToListAsync();

            // 獲取包含這些 TagId 的所有遊戲並進行分組和計算匹配數量
            var gameTagMatches = await _context.TagGroups
                                               .Where(tg => targetTagIds.Contains(tg.TagId))
                                               .GroupBy(tg => tg.GameId)
                                               .Select(g => new
                                               {
                                                   GameId = g.Key,
                                                   MatchCount = g.Count(tg => targetTagIds.Contains(tg.TagId))
                                               })
                                               .OrderByDescending(g => g.MatchCount)
                                               .Select(g => new
                                               {
                                                   GameId = g.GameId // 仅返回游戏 ID，不返回 MatchCount
                                               })
                                               .Skip(1)
                                               .Take(20)
                                               .ToListAsync();

            var GameTagSameData = await _context.Games
                                               .Where(g => gameTagMatches.Select(gtm => gtm.GameId).Contains(g.GameId))
                                               .ToListAsync();

            // 返回 JSON 結果
            return Json(GameTagSameData);
        }

        //(前台)拿取指定遊戲的最低價
        [HttpGet("GetGamePricelowestData")]
        public async Task<JsonResult> GetGamePricelowestData(int id)
        {
            var threeMonthsAgo = DateTime.Now.AddMonths(-3);
            var lowestData = _context.PriceHistories
                .Where(pc => pc.GameId == id && pc.Date >= threeMonthsAgo)
                .OrderBy(record => record.Price)
                .Select(group => new
                {
                    Date = group.Date,
                    LowestPrice = group.Price
                }).FirstOrDefault();


            // 返回 JSON 結果
            return Json(lowestData);
        }

        //(前台)拿取指定遊戲的在線人數
        [HttpGet("GetGamePeopleData")]
        public async Task<JsonResult> GetGamePeopleData(int id)
        {
            var peopleData = _context.PlayersHistories
                .Where(pc => pc.GameId == id)
                .OrderByDescending(record => record.Date)
                .Select(group => new
                {
                    Date = group.Date,
                    Players = group.Players
                }).FirstOrDefault();


            // 返回 JSON 結果
            return Json(peopleData);
        }

        //(前台)熱門標籤
        [HttpGet("TagsData")]
        public async Task<JsonResult> TagsData()
        {
            return Json(_context.Tags.Take(20));
        }

        //(首頁)用標籤拿取相關遊戲
        [HttpGet("GetGameSameData")]
        public async Task<JsonResult> GetGameSameData(int tagId, int tagNum)
        {
            int gameNum = 10000 + tagNum;

            var GetGameSameeData = await _context.TagGroups
                .Where(pc => pc.TagId == tagId)
                .Select(ph => ph.GameId).ToListAsync(); //拿到含有此tagId的gameId

            var gameTagMatches = await _context.Games //再搜尋此gameId的細部資料
                                              .Where(tg => GetGameSameeData.Contains(tg.GameId) && tg.GameId >= gameNum)
                                              .Skip(tagNum)
                                              .Take(20)
                                              .ToListAsync();


            // 返回 JSON 結果
            return Json(gameTagMatches);
        }

        //GameDataTable設定
        [HttpGet("GetGameData")]
        public async Task<JsonResult> GetGameData(int num)
        {
            int gameNum = 10000 + num;


            var GetGameData = await _context.Games
                .Where(pc => pc.GameId >= gameNum)
                .Take(20)
                .ToListAsync();


            return Json(GetGameData);
        }

        //GameDataTable設定
        [HttpGet("inputChangSeach")]
        public JsonResult inputChangSeach(string input, int num)
        {
            var SeachData = _context.Games //再搜尋此gameId的細部資料
                                          .Where(tg => tg.Name.Contains(input))
                                          .OrderBy(game => game.GameId)
                                          .ThenBy(game => game.Name)
                                          .Skip(num)
                                          .Take(20);

            return Json(SeachData);
        }

        
    }
}

////(API)Game-Price抓取寫回程式庫(更新)
//[HttpGet("GetGamePriceDataToDB")]
//public async Task<string> GetGamePriceDataToDB()
//{
//    if (isPriceDataUsing== false)
//    {
//        isPriceDataUsing= true;
//        HttpClient client = new HttpClient();
//        client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");
//        int? num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
//        int allNum = 0;
//        int errNum = 0;
//        int priceErrNum = 0;

//        for (int GameId = 10000; GameId <= num; GameId++)
//        {
//            int testNum = GameId - 9999;
//            double testprogressNum = Math.Round(((double)testNum / 1198) * 100, 2); ;
//            progressNum = (int)testprogressNum;
//            Console.WriteLine(testprogressNum);
//            Console.WriteLine(progressNum);
//            Console.WriteLine(GameId);
//            await Task.Delay(1400);

//            allNum++;
//            var game = await _context.Games.FindAsync(GameId);

//            PriceHistory PriceHistory = new PriceHistory
//            {
//                GameId = GameId
//            };

//            if (game == null)
//            {
//                continue;// 如果找不到遊戲，繼續下一個遊戲的處理
//            }
//            int? AppId = game.AppId;

//            HttpResponseMessage Response = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={AppId}&l=zh-tw");
//            Response.EnsureSuccessStatusCode();

//            string data = await Response.Content.ReadAsStringAsync();
//            dynamic jsonData = JsonConvert.DeserializeObject(data);
//            try
//            {
//                dynamic gameInfo = jsonData[$"{AppId}"]["data"];
//                //string Id = gameInfo["steam_appid"];
//                if (gameInfo["is_free"] == true)
//                {
//                    game.OriginalPrice = 0;
//                    game.CurrentPrice = 0;
//                    PriceHistory.Price = 0;
//                    try
//                    {
//                        //_context.Entry(game).State = EntityState.Modified;
//                        _context.Update(game);
//                        _context.PriceHistories.Add(PriceHistory);
//                        await _context.SaveChangesAsync();
//                    }
//                    catch (DbUpdateConcurrencyException)
//                    {
//                        Console.WriteLine("錯誤");
//                        if (!GameExists(game.GameId))
//                        {
//                            return "錯誤";
//                        }
//                        else
//                        {
//                            continue;
//                        }
//                    }
//                }
//                else
//                {

//                    dynamic price = gameInfo["price_overview"];
//                    string initial_formatted = price["initial_formatted"];
//                    string finalFormatted = price["final_formatted"];
//                    if (initial_formatted == "")
//                    {
//                        //將字串中的,拿掉以便match中沒有斷句
//                        finalFormatted = finalFormatted.Replace(",", "");
//                        //使用正規表達式去擷取數字
//                        Match match = Regex.Match(finalFormatted, @"\d+");

//                        if (match.Success)
//                        {
//                            string finalPrice = match.Value;
//                            int.TryParse(finalPrice, out int prices);
//                            game.OriginalPrice = prices;
//                            game.CurrentPrice = prices;
//                            PriceHistory.Price = prices;
//                        }
//                        else
//                        {
//                            priceErrNum++;
//                        }
//                    }
//                    else
//                    {
//                        //將字串中的,拿掉以便match中沒有斷句
//                        initial_formatted = initial_formatted.Replace(",", "");
//                        finalFormatted = finalFormatted.Replace(",", "");
//                        //使用正規表達式去擷取數字
//                        Match initialmatch = Regex.Match(initial_formatted, @"\d+");
//                        Match finalmatch = Regex.Match(finalFormatted, @"\d+");

//                        if (initialmatch.Success && finalmatch.Success)
//                        {
//                            string initialPrice = initialmatch.Value;
//                            string finalPrice = finalmatch.Value;
//                            int.TryParse(initialPrice, out int initial);
//                            int.TryParse(finalPrice, out int final);
//                            game.OriginalPrice = initial;
//                            game.CurrentPrice = final;
//                            PriceHistory.Price = final;
//                        }
//                        else
//                        {
//                            priceErrNum++;
//                        }

//                    }
//                    try
//                    {
//                        //_context.Entry(game).State = EntityState.Modified;
//                        _context.Update(game);
//                        _context.PriceHistories.Add(PriceHistory);
//                        await _context.SaveChangesAsync();
//                    }
//                    catch (DbUpdateConcurrencyException)
//                    {
//                        Console.WriteLine("錯誤");
//                        if (!GameExists(game.GameId))
//                        {
//                            return "錯誤";
//                        }
//                        else
//                        {
//                            continue;
//                        }
//                    }
//                }
//            }
//            catch
//            {
//                try
//                {
//                    PriceHistory.Price = (int)game.OriginalPrice;
//                    //_context.Entry(game).State = EntityState.Modified;
//                    _context.PriceHistories.Add(PriceHistory);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    Console.WriteLine("錯誤");
//                    if (!GameExists(game.GameId))
//                    {
//                        return "錯誤";
//                    }
//                    else
//                    {
//                        continue;
//                    }
//                }
//                errNum++;
//                continue;
//            }
//        }
//        isPriceDataUsing = false;

//        return "總次數:" + allNum + "\nAPI找不到次數:" + errNum + "\n價錢錯誤次數:" + priceErrNum;
//    }
//    else
//    {
//        return "有人正在使用價格抓取";
//    }
//}