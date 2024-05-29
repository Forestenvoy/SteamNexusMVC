using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using SteamNexus_Server.Dtos;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace SteamNexus_Server.Controllers;

// 套用 CORS 策略
[EnableCors("AllowAny")]
[Route("api/[controller]")]
[ApiController]
public class PcBuilderController : ControllerBase
{
    // Dependency Injection
    private readonly SteamNexusDbContext _context;


    // Constructor
    public PcBuilderController(SteamNexusDbContext context)
    {
        _context = context;
    }

    // 回傳上架菜單資料
    // GET: api/PcBuilder/GetMenuList
    [HttpGet("GetMenuLists")]
    public IEnumerable<MenuItemDto> GetMenuLists()
    {
        var menuLists = _context.Menus
            .Where(ml => ml.Active == true)
            .OrderBy(ml => ml.TotalPrice)
            .AsNoTracking() // 防止 EF Core 追蹤查詢結果
            .Select(m => new MenuItemDto
            {
                Id = m.MenuId,
                Name = m.Name,
                TotalPrice = m.TotalPrice,
                DetailCount = m.MenuDetails.Count()
            });

        return menuLists;
    }

    // 回傳產品資料
    // GET: api/PcBuilder
    [HttpGet]
    public ActionResult<IEnumerable<ProductDto>> GetProductData(int type)
    {
        try
        {
            // 尋找特定硬體的產品資料
            var result = _context.ComponentClassifications
                .Where(c => c.ComputerPartCategoryId == type)
                .Join(_context.ProductInformations, c => c.ComponentClassificationId, p => p.ComponentClassificationId,
                (c, p) => new ProductDto
                {
                    Id = p.ProductInformationId,
                    Classification = c.Name,
                    Name = p.Name,
                    Specification = p.Specification,
                    Price = p.Price,
                    Wattage = p.Wattage,
                    Recommend = p.Recommend
                });

            if (result == null || !result.Any())
            {
                return NotFound("Data not found.");
            }

            return Ok(result);
        }
        catch (Exception error)
        {
            // 未來考慮引用日誌框架如 Serilog 記錄異常 
            Console.WriteLine(error.Message);

            // 未來考慮引用中介軟體 RequestDelegate 做 例外處理
            return StatusCode(500, "An internal server error occurred. Please try again later.");
        }

    }

    // 回傳產品資料 RAM
    // GET: api/PcBuilder/GetRAMData
    [HttpGet("GetRAMData")]
    public ActionResult<IEnumerable<ProductDto>> GetRAMData()
    {
        try
        {
            // 尋找特定硬體的產品資料
            var result = _context.ComponentClassifications
                .Where(c => c.ComputerPartCategoryId == 10002)
                .Join(
                    _context.ProductInformations,
                    c => c.ComponentClassificationId,
                    p => p.ComponentClassificationId,
                    (c, p) => new { c, p }
                )
                .Join(
                    _context.ProductRAMs,
                    cp => cp.p.ProductInformationId,
                    r => r.ProductInformationId,
                    (cp, r) => new RamDto
                    {
                        Id = cp.p.ProductInformationId,
                        Classification = cp.c.Name,
                        Name = cp.p.Name,
                        Specification = cp.p.Specification,
                        Price = cp.p.Price,
                        Wattage = cp.p.Wattage,
                        Recommend = cp.p.Recommend,
                        Size = r.Size  // 從 ProductRAM 中獲取 Size
                    }
                );

            if (result == null || !result.Any())
            {
                return NotFound("Data not found.");
            }

            return Ok(result);
        }
        catch (Exception error)
        {
            // 未來考慮引用日誌框架如 Serilog 記錄異常 
            Console.WriteLine(error.Message);

            // 未來考慮引用中介軟體 RequestDelegate 做 例外處理
            return StatusCode(500, "An internal server error occurred. Please try again later.");
        }

    }

    // 菜單產品資料
    public class MenuProductDto
    {
        public string? Type { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Price { get; set; }
        public int Wattage { get; set; }
    }


    // 回傳菜單產品資料
    // GET: api/PcBuilder/GetProductList?MenuId=10000
    [HttpGet("GetProductList")]
    public ActionResult<IEnumerable<MenuProductDto>> GetProductList(int MenuId)
    {
        try
        {
            // 利用導覽屬性 獲取 產品的 TypeId
            var MenuDetailSet = _context.MenuDetails.Where(m => m.MenuId == MenuId)
                .Include(md => md.ProductInformation)
                .ThenInclude(pi => pi != null ? pi.ComponentClassification : null)
                .ThenInclude(cc => cc != null ? cc.ComputerPartCategory : null);

            if (MenuDetailSet == null)
            {
                // 找不到回傳 404 
                return NotFound("Data not found.");
            }

            var resultList = new List<MenuProductDto>();

            foreach (var menuDetail in MenuDetailSet)
            {
                if (menuDetail != null && menuDetail.ProductInformation != null)
                {
                    var componentClassification = menuDetail.ProductInformation.ComponentClassification;
                    if (componentClassification != null)
                    {
                        string typeName = componentClassification.ComputerPartCategory.Name;
                        string type = "";
                        if (typeName != null)
                        {
                            int end = typeName.IndexOf(" ", StringComparison.Ordinal);
                            type = typeName.Substring(0, end);
                        }

                        MenuProductDto menuProductDto = new MenuProductDto
                        {
                            Type = type,
                            Id = menuDetail.ProductInformation.ProductInformationId,
                            Name = menuDetail.ProductInformation.Name + menuDetail.ProductInformation.Specification,
                            Price = menuDetail.ProductInformation.Price,
                            Wattage = menuDetail.ProductInformation.Wattage
                        };
                        resultList.Add(menuProductDto);
                    }
                }
            }
            return Ok(resultList);
        }
        catch (Exception error)
        {
            // 未來考慮引用日誌框架如 Serilog 記錄異常 
            Console.WriteLine(error.Message);

            // 未來考慮引用中介軟體 RequestDelegate 做 例外處理
            return StatusCode(500, "An internal server error occurred. Please try again later.");
        }
    }


    // 計算比例需求資料
    public class RatioDataDto
    {
        // CPU 產品 ID
        public int pCpuId { get; set; }
        // GPU 產品 ID
        public int pGpuId { get; set; }
        // RAM 產品 ID
        public int pRamId { get; set; }
    }
    // 遊戲比例
    public class GameRatioDto
    {
        // 滿足最低配備的比例
        public int min { get; set; }

        // 最低配備遊戲數量
        public int minCount { get; set; }

        // 滿足建議配備的比例
        public int rec { get; set; }

        // 建議配備遊戲數量
        public int recCount { get; set; }
    }

    // 計算遊戲比例
    [HttpPost("calculateRatio")]
    public ActionResult<GameRatioDto> calculateRatio([FromBody] RatioDataDto data)
    {
        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                // 取得 CPU 跑分 joint CPU 跑分
                int? cpuScore = _context.ProductCPUs
                    .Join(_context.CPUs,
                          pc => pc.CPUId,
                          c => c.CPUId,
                          (pc, c) => new { pc, c })
                    .FirstOrDefault(joined => joined.pc.ProductInformationId == data.pCpuId)
                    ?.c.Score;
                if (cpuScore == null)
                {
                    return NotFound("CPU Data not found.");
                }
                // 取得 GPU 跑分 joint GPU 跑分
                int? gpuScore = _context.ProductGPUs
                    .Join(_context.GPUs,
                          pg => pg.GPUId,
                          g => g.GPUId,
                          (pg, g) => new { pg, g })
                    .FirstOrDefault(joined => joined.pg.ProductInformationId == data.pGpuId)
                    ?.g.Score;
                if (gpuScore == null)
                {
                    return NotFound("GPU Data not found.");
                }
                // 取得 RAM 容量
                int? ramSize = _context.ProductRAMs
                    .FirstOrDefault(p => p.ProductInformationId == data.pRamId)
                    ?.Size;
                if (ramSize == null)
                {
                    return NotFound("RAM Data not found.");
                }

                // 計算可玩的最低配備遊戲數量
                var result = _context.MinimumRequirements.Where(mr => mr.CPU.Score <= cpuScore && mr.GPU.Score <= gpuScore && mr.RAM <= ramSize);
                double minRatio = ((double)result.Count() / _context.MinimumRequirements.Count()) * 100;
                // 無條件捨去
                int minRatioRoundedDown = (int)Math.Floor(minRatio);
                // 計算可玩的建議配備遊戲數量
                var result2 = _context.RecommendedRequirements.Where(mr => mr.CPU.Score <= cpuScore && mr.GPU.Score <= gpuScore && mr.RAM <= ramSize);
                double recRatio = ((double)result2.Count() / _context.RecommendedRequirements.Count()) * 100;
                // 無條件捨去
                int recRatioRoundedDown = (int)Math.Floor(recRatio);

                // 回傳比例需求資料
                var gameRatioDto = new GameRatioDto
                {
                    min = minRatioRoundedDown,
                    minCount = result.Count(),
                    rec = recRatioRoundedDown,
                    recCount = result2.Count()
                };

                return Ok(gameRatioDto);
            }
            catch (Exception error)
            {
                // 未來考慮引用日誌框架如 Serilog 記錄異常 
                Console.WriteLine(error.Message);

                // 未來考慮引用中介軟體 RequestDelegate 做 例外處理
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        else
        {
            // 返回 400 狀態碼 ~ 驗證不合法，同時回傳 ErrorMessage
            return BadRequest("Date Error !");
        }
    }

    // 遊戲資料 Dto
    public class MatchGameDataDto
    {
        // 遊戲 ID
        public int? GameId { get; set; }

        // 滿足建議配備的比例
        public string? Name { get; set; }
    }

    // 回傳遊戲列表
    [HttpPost("GetGameList")]
    public ActionResult<IEnumerable<MatchGameDataDto>> GetGameList([FromBody] RatioDataDto data, string mode, int page ,string? keyword)
    {
        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                if (mode != "min" && mode != "rec")
                {
                    return BadRequest("Mode Error !");
                }


                // 取得 CPU 跑分 joint CPU 跑分
                int? cpuScore = _context.ProductCPUs
                    .Join(_context.CPUs,
                          pc => pc.CPUId,
                          c => c.CPUId,
                          (pc, c) => new { pc, c })
                    .FirstOrDefault(joined => joined.pc.ProductInformationId == data.pCpuId)
                    ?.c.Score;
                if (cpuScore == null)
                {
                    return NotFound("CPU Data not found.");
                }
                // 取得 GPU 跑分 joint GPU 跑分
                int? gpuScore = _context.ProductGPUs
                    .Join(_context.GPUs,
                          pg => pg.GPUId,
                          g => g.GPUId,
                          (pg, g) => new { pg, g })
                    .FirstOrDefault(joined => joined.pg.ProductInformationId == data.pGpuId)
                    ?.g.Score;
                if (gpuScore == null)
                {
                    return NotFound("GPU Data not found.");
                }
                // 取得 RAM 容量
                int? ramSize = _context.ProductRAMs
                    .FirstOrDefault(p => p.ProductInformationId == data.pRamId)
                    ?.Size;
                if (ramSize == null)
                {
                    return NotFound("RAM Data not found.");
                }

                // 頁數計算
                int pageSize = 10;
                int skip = (page - 1) * pageSize;

                // 根據模式吐出遊戲列表
                if (mode == "min")
                {
                    // 計算可玩的最低配備遊戲數量
                    var result = _context.MinimumRequirements.Where(mr => mr.CPU.Score <= cpuScore && mr.GPU.Score <= gpuScore && mr.RAM <= ramSize)
                        .Where(mr => string.IsNullOrEmpty(keyword) || (mr.Game.Name != null && mr.Game.Name.Contains(keyword)))
                        .OrderBy(g => g.Game.GameId)
                        .Skip(skip)
                        .Take(pageSize)
                        .Select(g => new MatchGameDataDto { GameId = g.Game.GameId, Name = g.Game.Name });
                    if (result.Any())
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return NotFound("No game found.");
                    }
                }
                else if (mode == "rec")
                {
                    // 計算可玩的建議配備遊戲數量
                    var result2 = _context.RecommendedRequirements.Where(mr => mr.CPU.Score <= cpuScore && mr.GPU.Score <= gpuScore && mr.RAM <= ramSize)
                        .Where(mr => string.IsNullOrEmpty(keyword) || (mr.Game.Name != null && mr.Game.Name.Contains(keyword)))
                        .OrderBy(g => g.Game.GameId)
                        .Skip(skip)
                        .Take(pageSize)
                        .Select(g => new MatchGameDataDto { GameId = g.Game.GameId, Name = g.Game.Name });
                    if (result2.Any())
                    {
                        return Ok(result2);
                    }
                    else
                    {
                        return NotFound("No game found.");
                    }
                }


                return BadRequest("Mode Error !");
            }
            catch (Exception error)
            {
                // 未來考慮引用日誌框架如 Serilog 記錄異常 
                Console.WriteLine(error.Message);

                // 未來考慮引用中介軟體 RequestDelegate 做 例外處理
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        else
        {
            // 返回 400 狀態碼 ~ 驗證不合法，同時回傳 ErrorMessage
            return BadRequest("Date Error !");
        }
    }

    public class GameFilterDto
    {
        // CPU 產品 ID
        public int pCpuId { get; set; }
        // GPU 產品 ID
        public int pGpuId { get; set; }
        // RAM 產品 ID
        public int pRamId { get; set; }
        // 關鍵字
        public string? keyword { get; set; }
        // 模式
        public string? mode { get; set; }
    }

    public class GameQualityDto
    {
        // 遊戲數量
        public int count { get; set; }
    }

    // 計算遊戲數量 ~ 關鍵字搜尋
    [HttpPost("calculateQuantityByKeyword")]
    public ActionResult<GameQualityDto> calculateQuantityByKeyword([FromBody] GameFilterDto data)
    {
        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                // 取得 CPU 跑分 joint CPU 跑分
                int? cpuScore = _context.ProductCPUs
                    .Join(_context.CPUs,
                          pc => pc.CPUId,
                          c => c.CPUId,
                          (pc, c) => new { pc, c })
                    .FirstOrDefault(joined => joined.pc.ProductInformationId == data.pCpuId)
                    ?.c.Score;
                if (cpuScore == null)
                {
                    return NotFound("CPU Data not found.");
                }
                // 取得 GPU 跑分 joint GPU 跑分
                int? gpuScore = _context.ProductGPUs
                    .Join(_context.GPUs,
                          pg => pg.GPUId,
                          g => g.GPUId,
                          (pg, g) => new { pg, g })
                    .FirstOrDefault(joined => joined.pg.ProductInformationId == data.pGpuId)
                    ?.g.Score;
                if (gpuScore == null)
                {
                    return NotFound("GPU Data not found.");
                }
                // 取得 RAM 容量
                int? ramSize = _context.ProductRAMs
                    .FirstOrDefault(p => p.ProductInformationId == data.pRamId)
                    ?.Size;
                if (ramSize == null)
                {
                    return NotFound("RAM Data not found.");
                }

                if (data.mode == "min" && data.keyword != null)
                {
                    // 計算可玩的最低配備遊戲數量
                    var result = _context.MinimumRequirements.Where(mr => mr.CPU.Score <= cpuScore && mr.GPU.Score <= gpuScore && mr.RAM <= ramSize)
                        .Where(mr => mr.Game.Name != null && mr.Game.Name.Contains(data.keyword));

                    return Ok(new GameQualityDto { count = result.Count() });
                }
                else if (data.mode == "rec" && data.keyword != null)
                {
                    // 計算可玩的建議配備遊戲數量
                    var result2 = _context.RecommendedRequirements.Where(mr => mr.CPU.Score <= cpuScore && mr.GPU.Score <= gpuScore && mr.RAM <= ramSize)
                        .Where(mr => mr.Game.Name != null && mr.Game.Name.Contains(data.keyword));

                    return Ok(new GameQualityDto { count = result2.Count() });
                }
                else
                {
                    return BadRequest("Mode Error !");
                }

            }
            catch (Exception error)
            {
                // 未來考慮引用日誌框架如 Serilog 記錄異常 
                Console.WriteLine(error.Message);

                // 未來考慮引用中介軟體 RequestDelegate 做 例外處理
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        else
        {
            // 返回 400 狀態碼 ~ 驗證不合法，同時回傳 ErrorMessage
            return BadRequest("Date Error !");
        }
    }



}
