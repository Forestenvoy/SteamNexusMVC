using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SteamNexus_Server.Data;
using System.Text.RegularExpressions;
using SteamNexus_Server.Models;

namespace SteamNexus_Server.Services
{
    

    public class GamePriceToDB
    {
        private readonly SteamNexusDbContext _context;
        
        public static int progressNum = 0;
        public static bool isPriceDataUsing = false;

        public GamePriceToDB(SteamNexusDbContext context)
        {
            _context = context;
            
        }
        
        public async Task GetGamePriceDataToDB()
        {

            if (isPriceDataUsing == false)
            {
                isPriceDataUsing = true;
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Accept-Language", "zh-TW");

                var num = _context.Games.OrderByDescending(g => g.GameId).FirstOrDefault()?.GameId ?? 0;
                var allNum = 0;
                var errNum = 0;
                var priceErrNum = 0;

                for (var GameId = 10000; GameId <= num; GameId++)
                {
                    var testNum = GameId - 9999;
                    var testprogressNum = Math.Round(((double)testNum / 1198) * 100, 2);
                    progressNum = (int)testprogressNum;
                    Console.WriteLine(testprogressNum);
                    Console.WriteLine(progressNum);
                    Console.WriteLine(GameId);

                    await Task.Delay(1400);

                    allNum++;
                    var game = await _context.Games.FindAsync(GameId);

                    var priceHistory = new PriceHistory { GameId = GameId };

                    if (game == null) continue;

                    var AppId = game.AppId;
                    var response = await client.GetAsync($"https://store.steampowered.com/api/appdetails?appids={AppId}&l=zh-tw");
                    response.EnsureSuccessStatusCode();

                    var data = await response.Content.ReadAsStringAsync();
                    dynamic jsonData = JsonConvert.DeserializeObject(data);

                    try
                    {
                        dynamic gameInfo = jsonData[$"{AppId}"]["data"];
                        if (gameInfo["is_free"] == true)
                        {
                            game.OriginalPrice = 0;
                            game.CurrentPrice = 0;
                            priceHistory.Price = 0;
                        }
                        else
                        {
                            dynamic price = gameInfo["price_overview"];
                            var initial_formatted = ((string)price["initial_formatted"]).Replace(",", "");
                            var finalFormatted = ((string)price["final_formatted"]).Replace(",", "");

                            var initialMatch = Regex.Match(initial_formatted, @"\d+");
                            var finalMatch = Regex.Match(finalFormatted, @"\d+");

                            if (initialMatch.Success && finalMatch.Success)
                            {
                                int.TryParse(initialMatch.Value, out var initial);
                                int.TryParse(finalMatch.Value, out var final);
                                game.OriginalPrice = initial;
                                game.CurrentPrice = final;
                                priceHistory.Price = final;
                            }
                            else
                            {
                                priceErrNum++;
                            }
                        }

                        _context.Update(game);
                        _context.PriceHistories.Add(priceHistory);
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        priceHistory.Price = (int)game.OriginalPrice;
                        _context.PriceHistories.Add(priceHistory);
                        await _context.SaveChangesAsync();
                        errNum++;
                        continue;
                    }
                }

                isPriceDataUsing = false;
                Console.WriteLine($"總次數: {allNum}\nAPI找不到次數: {errNum}\n價錢錯誤次數: {priceErrNum}");
            }
            else
            {
                Console.WriteLine("有人正在使用價格抓取");
            }
        }

        //(API)Game-Price抓取寫回程式庫(更新)
        //public async Task GetGamePriceDataToDB()
        //{
        //    if (isPriceDataUsing == false)
        //    {
        //        isPriceDataUsing = true;
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
        //                            Console.WriteLine("錯誤");
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
        //                            Console.WriteLine("錯誤");
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
        //                        Console.WriteLine("錯誤");
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

        //        Console.WriteLine("總次數:" + allNum + "\nAPI找不到次數:" + errNum + "\n價錢錯誤次數:" + priceErrNum);
        //    }
        //    else
        //    {
        //        Console.WriteLine("有人正在使用價格抓取");
        //    }
        //}

        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.GameId == id);
        }
    }
}
