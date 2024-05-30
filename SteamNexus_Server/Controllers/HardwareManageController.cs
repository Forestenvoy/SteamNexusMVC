using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using SteamNexus_Server.Services;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static SteamNexus_Server.Controllers.HardwareManageController;


namespace SteamNexus_Server.Controllers;


// 套用 CORS 策略
[EnableCors("AllowAny")]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class HardwareManageController : ControllerBase
{
    // Dependency Injection
    private readonly SteamNexusDbContext _context;
    private readonly CoolPCWebScraping _coolPCWebScraping;


    // Constructor
    public HardwareManageController(SteamNexusDbContext context, CoolPCWebScraping coolPCWebScraping)
    {
        _context = context;
        _coolPCWebScraping = coolPCWebScraping;
    }

    // 回傳硬體產品種類
    // GET: api/HardwareManage/GetComputerParts
    [AllowAnonymous]
    [HttpGet("GetComputerParts")]
    public IEnumerable<object> GetComputerParts()
    {
        // 下拉式選單 => 硬體
        var ComputerParts = _context.ComputerPartCategories.Select(c => new
        {
            Id = c.ComputerPartCategoryId,
            c.Name
        });
        return ComputerParts;
    }

    // 回傳硬體資料
    // GET: api/HardwareManage/GetProductData
    [HttpGet("GetProductData")]
    public IEnumerable<object> GetProductData(int Type)
    {
        // 尋找特定硬體
        var result = _context.ComponentClassifications
            .Where(c => c.ComputerPartCategoryId == Type)
            .Join(_context.ProductInformations, c => c.ComponentClassificationId, p => p.ComponentClassificationId,
            (c, p) => new
            {
                ProductId = p.ProductInformationId,
                ComponentClassificationName = c.Name,
                ProductName = p.Name,
                p.Specification,
                p.Price,
                p.Wattage,
                p.Recommend
            });

        return result;
    }

    // 硬體產品 DTO
    public class HardwareProduct
    {
        [Required]
        [Range(10000, 99999)]
        public int ProductId { get; set; }

        [Required]
        [Range(0, 2000, ErrorMessage = "瓦數範圍介於 0 ~ 2000 之間")]
        public int Wattage { get; set; }

        [Required]
        [Range(0, 4)]
        public int Recommend { get; set; }
    }

    // 產品資料 Update
    // POST: api/HardwareManage/ProductDataUpdate
    [HttpPost("ProductDataUpdate")]
    public async Task<IActionResult> ProductDataUpdate([FromBody] HardwareProduct data)
    {
        // 如果驗證合法
        if (ModelState.IsValid)
        {
            // 加入 資料庫 交易機制
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // 根據ID查詢要更新的產品 
                    var product = await _context.ProductInformations.FindAsync(data.ProductId);
                    if (product != null)
                    {
                        // 更新產品資料
                        product.Wattage = data.Wattage;
                        product.Recommend = data.Recommend;

                        // 設置 EntityState 為 Modified
                        _context.Entry(product).State = EntityState.Modified;

                        // 儲存變更
                        await _context.SaveChangesAsync();

                        // 因為欄位有"時間戳記"屬性 ==> 樂觀併發控制
                        // 1.讀取：交易將資料讀入，這時系統會給交易分派一個時間戳。
                        // 2.校驗：交易執行完畢後，進行提交。這時同步校驗所有交易，如果交易所讀取的資料在讀取之後又被其他交易修改，則產生衝突，交易被中斷(回復)。
                        // 3.寫入：通過校驗階段後，將更新的資料寫入資料庫。

                        // 交易完成 ~ 提交成功，資料庫才會真的異動 !!!
                        await transaction.CommitAsync();

                        // 返回 200 狀態碼 ~ 變更成功
                        return Ok($"{data.ProductId} 資料變更成功");
                    }
                    else
                    {
                        // 返回 404 狀態碼 ~ 找不到產品
                        return NotFound("資料庫找不到此產品");
                    }
                }
                // 解決並行存取衝突
                catch (DbUpdateConcurrencyException)
                {
                    // 回滾交易
                    await transaction.RollbackAsync();
                    // 返回 409 狀態碼 ~ 同時修改衝突
                    return StatusCode(409, "同時修改衝突，請重新整理後再試");
                }
                catch (Exception ex)
                {
                    // 返回 500 狀態碼 ~ 伺服器內部錯誤
                    return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
                }
            }
        }
        else
        {
            // 返回 400 狀態碼 ~ 驗證不合法，同時回傳 ErrorMessage
            return BadRequest("瓦數範圍介於 0 ~ 2000 之間");
        }
    }

    // 零件更新操作行為
    public static bool isHardwareUpdate = false;

    // 檢測是否有使用者在做更新操作
    [HttpGet("IsHardwareUpdate")]
    public string IsHardwareUpdate()
    {
        if (isHardwareUpdate)
        {
            return "true";
        }
        return "false";
    }

    // 單一零件更新
    public class HardwareType
    {
        [Required]
        [Range(10000, 10010)]
        public int Type { get; set; }
    }

    // POST: api/HardwareManage/UpdateHardwareOne
    [HttpPost("UpdateHardwareOne")]
    public async Task<IActionResult> UpdateHardwareOne([FromBody] HardwareType data)
    {
        // 如果驗證合法
        if (ModelState.IsValid)
        {
            isHardwareUpdate = true;
            CoolPCWebScraping.eventMessage = "爬蟲啟動中";
            _coolPCWebScraping.UpdateAllComponentClassifications();
            switch (data.Type)
            {
                case (int)ComputerPartCategory.Type.CPU:
                    CoolPCWebScraping.eventMessage = "CPU 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateCPU();
                    CoolPCWebScraping.eventMessage = "CPU 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("CPU 更新成功");
                case (int)ComputerPartCategory.Type.MB:
                    CoolPCWebScraping.eventMessage = "Motherboard 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateMB();
                    CoolPCWebScraping.eventMessage = "Motherboard 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("MB 更新成功");
                case (int)ComputerPartCategory.Type.RAM:
                    CoolPCWebScraping.eventMessage = "RAM 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateRAM();
                    CoolPCWebScraping.eventMessage = "RAM 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("RAM 更新成功");
                case (int)ComputerPartCategory.Type.SSD:
                    CoolPCWebScraping.eventMessage = "SSD 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateSSD();
                    CoolPCWebScraping.eventMessage = "SSD 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("SSD 更新成功");
                case (int)ComputerPartCategory.Type.HDD:
                    CoolPCWebScraping.eventMessage = "HDD 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateHDD();
                    CoolPCWebScraping.eventMessage = "HDD 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("HDD 更新成功");
                case (int)ComputerPartCategory.Type.AirCooler:
                    CoolPCWebScraping.eventMessage = "AirCooler 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateAirCooler();
                    CoolPCWebScraping.eventMessage = "AirCooler 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("AirCooler 更新成功");
                case (int)ComputerPartCategory.Type.LiquidCooler:
                    CoolPCWebScraping.eventMessage = "LiquidCooler 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateLiquidCooler();
                    CoolPCWebScraping.eventMessage = "LiquidCooler 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("LiquidCooler 更新成功");
                case (int)ComputerPartCategory.Type.GPU:
                    CoolPCWebScraping.eventMessage = "GPU 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateGPU();
                    CoolPCWebScraping.eventMessage = "GPU 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("GPU 更新成功");
                case (int)ComputerPartCategory.Type.CASE:
                    CoolPCWebScraping.eventMessage = "CASE 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateCASE();
                    CoolPCWebScraping.eventMessage = "CASE 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("CASE 更新成功");
                case (int)ComputerPartCategory.Type.PSU:
                    CoolPCWebScraping.eventMessage = "PSU 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdatePSU();
                    CoolPCWebScraping.eventMessage = "PSU 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("PSU 更新成功");
                case (int)ComputerPartCategory.Type.OS:
                    CoolPCWebScraping.eventMessage = "OS 解析中";
                    await Task.Delay(500);
                    _coolPCWebScraping.UpdateOS();
                    CoolPCWebScraping.eventMessage = "OS 資料更新中";
                    await Task.Delay(500);
                    CoolPCWebScraping.eventMessage = "更新成功";
                    await Task.Delay(500);
                    isHardwareUpdate = false;
                    return Ok("OS 更新成功");
                default:
                    isHardwareUpdate = false;
                    return BadRequest("更新失敗");
            }
        }
        else
        {
            return BadRequest("更新失敗");
        }
    }

    // 全零件更新
    // POST: api/HardwareManage/UpdateHardwareAll
    [HttpPost("UpdateHardwareAll")]
    public async Task<IActionResult> UpdateHardwareAll()
    {
        isHardwareUpdate = true;
        CoolPCWebScraping.eventMessage = "爬蟲啟動中";
        _coolPCWebScraping.UpdateAllComponentClassifications();

        CoolPCWebScraping.eventMessage = "CPU 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateCPU();
        CoolPCWebScraping.eventMessage = "CPU 資料更新完成";
        await Task.Delay(500);


        CoolPCWebScraping.eventMessage = "GPU 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateGPU();
        CoolPCWebScraping.eventMessage = "GPU 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "RAM 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateRAM();
        CoolPCWebScraping.eventMessage = "RAM 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "Motherboard 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateMB();
        CoolPCWebScraping.eventMessage = "Motherboard 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "SSD 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateSSD();
        CoolPCWebScraping.eventMessage = "SSD 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "HDD 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateHDD();
        CoolPCWebScraping.eventMessage = "HDD 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "Air Cooler 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateAirCooler();
        CoolPCWebScraping.eventMessage = "Air Cooler 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "LiquidCooler 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateLiquidCooler();
        CoolPCWebScraping.eventMessage = "LiquidCooler 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "CASE 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateCASE();
        CoolPCWebScraping.eventMessage = "CASE 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "PSU 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdatePSU();
        CoolPCWebScraping.eventMessage = "PSU 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "OS 解析中";
        await Task.Delay(500);
        _coolPCWebScraping.UpdateOS();
        CoolPCWebScraping.eventMessage = "OS 資料更新完成";
        await Task.Delay(500);

        CoolPCWebScraping.eventMessage = "更新成功";
        await Task.Delay(500);

        isHardwareUpdate = false;
        return Ok("全零件更新成功");
    }

    // SSE 傳送事件進度
    [AllowAnonymous]
    [HttpGet("UpdateMessage")]
    public IActionResult UpdateMessage()
    {
        string message = "";
        message += $"id:{Guid.NewGuid()}\n";
        message += "retry:100\n";
        message += $"data:{CoolPCWebScraping.eventMessage}\n\n";
        return Content($"{message}", "text/event-stream", Encoding.UTF8);
    }

    // 菜單 DTO 
    public class MenuDto
    {
        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        public int TotalPrice { get; set; } = 0;

        [Required]
        public int TotalWattage { get; set; } = 0;
    }

    // Menu 建立
    // POST: api/HardwareManage/CreateMenu
    [HttpPost("CreateMenu")]
    public IActionResult CreateMenu([FromBody] MenuDto data)
    {

        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                Menu menu = new Menu();
                menu.Name = data.Name;
                menu.TotalPrice = data.TotalPrice;
                menu.TotalWattage = data.TotalWattage;

                _context.Menus.Add(menu);
                _context.SaveChanges();

                return Ok(menu.MenuId);
            }
            catch (Exception ex)
            {
                // 返回 500 狀態碼 ~ 伺服器內部錯誤
                return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
            }
        }
        else
        {
            return BadRequest("Menu 資料錯誤");
        }

    }

    // 菜單細節 DTO 
    public class MenuDetailDto
    {
        [Required]
        public int MenuId { get; set; }

        [Required]
        public int ProductInformationId { get; set; }
    }

    // MenuDetail 建立
    // POST: api/HardwareManage/CreateMenuDetail
    [HttpPost("CreateMenuDetail")]
    public IActionResult CreateMenuDetail([FromBody] MenuDetailDto data)
    {
        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                MenuDetail menuDetail = new MenuDetail();
                menuDetail.MenuId = data.MenuId;
                menuDetail.ProductInformationId = data.ProductInformationId;

                _context.MenuDetails.Add(menuDetail);
                _context.SaveChanges();

                return Ok($"{menuDetail.MenuDetailId} 新增成功");
            }
            catch (Exception ex)
            {
                // 返回 500 狀態碼 ~ 伺服器內部錯誤
                return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
            }
        }
        else
        {
            return BadRequest("MenuDetail 資料錯誤");
        }
    }

    // 回傳 菜單資料
    // GET: api/HardwareManage/GetMenuList
    [HttpGet("GetMenuList")]
    public IEnumerable<object> GetMenuList()
    {
        // 下拉式選單 => 硬體
        var MenuLists = _context.Menus.Select(c => new
        {
            Id = c.MenuId,
            c.Name,
            c.TotalPrice,
            c.TotalWattage,
            c.Status,
            c.Active,
            Count = c.MenuDetails.Count()
        });
        return MenuLists;
    }

    // 回傳 菜單資料
    // GET: api/HardwareManage/GetMenu
    [HttpGet("GetMenu")]
    public async Task<object> GetMenu(int MenuId)
    {
        // 下拉式選單 => 硬體
        var MenuDate = await _context.Menus
            .Where(c => c.MenuId == MenuId)
            .Select(c => new
            {
                Id = c.MenuId,
                c.Name,
                c.TotalPrice,
                c.TotalWattage,
                c.Status,
                c.Active,
                Count = c.MenuDetails.Count()
            }).FirstOrDefaultAsync();

        if (MenuDate == null)
        {
            // 找不到回傳 404 
            return NotFound();
        }

        return MenuDate;
    }

    // MenuIdDto
    public class MenuIdDto
    {
        [Required]
        public int MenuId { get; set; }
    }

    // 修改菜單上下架
    // POST: api/HardwareManage/MenuActive
    [HttpPost("MenuActive")]
    public IActionResult MenuActive([FromBody] MenuIdDto data)
    {
        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                var target = _context.Menus.Where(c => c.MenuId == data.MenuId).FirstOrDefault();

                if (target == null)
                {
                    // 找不到回傳 404 
                    return NotFound("找不到 Menu");
                }

                target.Active = !target.Active;

                _context.SaveChanges();


                if (target.Active)
                {
                    return Ok($"{target.Name}上架成功");
                }
                else
                {
                    return Ok($"{target.Name}下架成功");
                }
            }
            catch (Exception ex)
            {
                // 返回 500 狀態碼 ~ 伺服器內部錯誤
                return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
            }
        }
        else
        {
            return BadRequest("Menu 資料錯誤");
        }
    }

    // 刪除菜單
    // POST: api/HardwareManage/MenuActive
    [HttpPost("DeleteMenu")]
    public async Task<IActionResult> DeleteMenu([FromBody] MenuIdDto data)
    {
        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                var menu = await _context.Menus.Include(m => m.MenuDetails).FirstOrDefaultAsync(m => m.MenuId == data.MenuId);

                if (menu == null)
                {
                    // 找不到回傳 404 
                    return NotFound("找不到 Menu");
                }

                // 刪除關聯的 MenuDetails
                _context.MenuDetails.RemoveRange(menu.MenuDetails);

                // 刪除 Menu
                _context.Menus.Remove(menu);

                _context.SaveChanges();

                return Ok($"{menu.Name}刪除成功");

            }
            catch (Exception ex)
            {
                // 返回 500 狀態碼 ~ 伺服器內部錯誤
                return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
            }
        }
        else
        {
            return BadRequest("Menu 資料錯誤");
        }
    }

    // 回傳菜單詳情產品的 ProductId、TypeId
    // GET: api/HardwareManage/GetMenuDetail?MenuId=10000
    [HttpGet("GetMenuDetail")]
    public IEnumerable<object>? GetMenuDetail(int MenuId)
    {
        // 利用導覽屬性 獲取 產品的 TypeId
        var MenuDetailSet = _context.MenuDetails.Where(m => m.MenuId == MenuId)
            .Include(md => md.ProductInformation)
            .ThenInclude(pi => pi != null ? pi.ComponentClassification : null);

        if (MenuDetailSet == null)
        {
            // 找不到回傳 404 
            return null;
        }

        var resultList = new List<object>();

        foreach (var menuDetail in MenuDetailSet)
        {
            if (menuDetail != null && menuDetail.ProductInformation != null)
            {
                var componentClassification = menuDetail.ProductInformation.ComponentClassification;
                if (componentClassification != null)
                {
                    int computerPartCategoryId = componentClassification.ComputerPartCategoryId;
                    int? productInformationId = menuDetail.ProductInformationId;
                    int price = menuDetail.ProductInformation.Price;
                    int wattage = menuDetail.ProductInformation.Wattage;


                    resultList.Add(new { TypeId = computerPartCategoryId, ProductId = productInformationId, Price = price, Wattage = wattage });
                }
            }
        }

        return resultList;
    }

    // 菜單產品更新
    public class MenuDetailUDto
    {
        [Required]
        public int MenuId { get; set; }

        [Required]
        public int OriProductId { get; set; }

        [Required]
        public int NewProductId { get; set; }
    }


    // POST: api/HardwareManage/UpdateMenuDetail
    [HttpPost("UpdateMenuDetail")]
    public async Task<IActionResult> UpdateMenuDetail([FromBody] MenuDetailUDto data)
    {

        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                // 如果已有存在資料
                if (data.OriProductId != 0)
                {
                    var menuDetail = await _context.MenuDetails.Where(md => md.MenuId == data.MenuId).Where(md => md.ProductInformationId == data.OriProductId).FirstOrDefaultAsync();

                    if (menuDetail != null)
                    {
                        menuDetail.ProductInformationId = data.NewProductId;
                        _context.SaveChanges();
                        return Ok("資料變更成功");
                    }
                    else
                    {
                        // 找不到回傳 404 
                        return NotFound("找不到資料");
                    }
                }
                else
                {
                    MenuDetail menuDetail = new MenuDetail();
                    menuDetail.MenuId = data.MenuId;
                    menuDetail.ProductInformationId = data.NewProductId;
                    _context.MenuDetails.Add(menuDetail);
                    _context.SaveChanges();

                    return Ok("資料變更成功");
                }
            }
            catch (Exception ex)
            {
                // 返回 500 狀態碼 ~ 伺服器內部錯誤
                return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
            }
        }
        else
        {
            return BadRequest("資料錯誤");
        }
    }

    // 菜單數據更新
    public class MenuUDto
    {
        [Required]
        public int MenuId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        public int TotalPrice { get; set; } = 0;

        [Required]
        public int TotalWattage { get; set; } = 0;
    }

    [HttpPost("UpdateMenu")]
    public async Task<IActionResult> UpdateMenu([FromBody] MenuUDto data)
    {

        // 如果驗證合法
        if (ModelState.IsValid)
        {
            try
            {
                var menu = await _context.Menus.Where(m=>m.MenuId == data.MenuId).FirstOrDefaultAsync();
                
                if(menu != null)
                {
                    menu.Name = data.Name;
                    menu.TotalPrice = data.TotalPrice;
                    menu.TotalWattage = data.TotalWattage;
                    await _context.SaveChangesAsync();    

                    return Ok($"{menu.Name}更新成功");
                }
                else
                {
                    // 找不到回傳 404 
                    return NotFound("找不到資料");
                }
            }
            catch (Exception ex)
            {
                // 返回 500 狀態碼 ~ 伺服器內部錯誤
                return StatusCode(500, "伺服器內部錯誤：" + ex.Message);
            }
        }
        else
        {
            return BadRequest("資料錯誤");
        }
    }

}