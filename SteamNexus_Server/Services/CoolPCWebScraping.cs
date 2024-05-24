using HtmlAgilityPack;
using System.Text;
using SteamNexus_Server.Data;
using SteamNexus_Server.Models;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace SteamNexus_Server.Services
{
    public class CoolPCWebScraping
    {
        // Dependency Injection SteamNexusDbContext
        private readonly SteamNexusDbContext _context;
        // 宣告全品項資料(Html元素集合)
        IEnumerable<HtmlNode>? hardWareNodes;
        // 宣告 optgroup 群組 List 集合
        List<List<string>>? optgroups;
        // 宣告 optgroup 群組名稱 List
        List<string>? optgroupNames;
        // 宣告 事件進度訊息
        public static string eventMessage = "爬蟲啟動中";


        // 建構式
        public CoolPCWebScraping(SteamNexusDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 獲取硬體全品項資料(Html元素集合)
        /// </summary>
        private void _GetHardWareData()
        {
            // 如果已經爬取過就不再爬取
            if (hardWareNodes != null)
            {
                Console.WriteLine("HardWare Data Exist!");
                return;
            }

            // 註冊特定編碼(包含big5)
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            // HtmlWeb 物件 : 用於從 URL 加載 HTML 文檔
            HtmlWeb web = new HtmlWeb();
            // HtmlDocument 物件 : 為 HTML 文檔的 物件模型 DOM
            HtmlDocument doc = new HtmlDocument();

            // HTML 爬蟲讀取資料
            try
            {
                // 原價屋估價網址
                string url = @"https://coolpc.com.tw/evaluate.php";
                // 編碼 big5
                web.OverrideEncoding = Encoding.GetEncoding("big5");
                // 爬蟲讀取資料
                doc = web.Load(url);
            }
            catch (Exception error)
            {
                Console.WriteLine($"Web Scrabing Error\n{error.Message}");
                return;
            }

            // DocumentNode 屬性表示 HTML 文檔的根節點
            // Descendants("X") 會尋找指定標籤<X>的所有子元素集合，X 沒寫預設是回傳所有元素
            // .GetAttributeValue("class","") == ("bf") 會尋找 class="bf" 的所有子元素，找不到會回傳空字串

            // 硬體品項集合 ~ 30項，索引從 0 開始
            hardWareNodes = doc.DocumentNode.Descendants().Where(node => node.GetAttributeValue("class", "") == ("bf"));
            // 檢測硬體品項集合是否有資料
            if (hardWareNodes.Any())
            {
                Console.WriteLine("HardWare Data Get!");
                return;
            }
            else
            {
                Console.WriteLine("HardWare Data not found!");
                return;
            }
        }

        /// <summary>
        /// 獲取硬體單一零件集合資料並解析成List
        /// </summary>
        /// <param name="CoolPCType"></param>
        private void _GetComponentsList(int CoolPCType)
        {

            // 檢測硬體品項集合是否有資料
            if (hardWareNodes == null)
            {
                Console.WriteLine("HardWare Data not found!");
                return;
            }

            // 獲取硬體單一零件集合
            HtmlNode? Components = hardWareNodes?.ElementAtOrDefault(CoolPCType);
            // 檢驗零件集合是否有資料
            if (Components == null)
            {
                Console.WriteLine("This component not found!");
                return;
            }
            // 實體化 optgroup 群組 List 集合
            optgroups = new List<List<string>>();
            // 實體化 optgroup 群組名稱 List
            optgroupNames = new List<string>();
            // 實體化 一個品項 List
            List<string>? options = new List<string>();

            // 獲取 optgroup 內層元素
            IEnumerable<HtmlNode> innerNodes = Components.Descendants();

            // 解構 巢狀元素
            foreach ((HtmlNode node, int index) in innerNodes.Select((node, index) => (node, index)))
            {
                // 檢查節點是否為 <optgroup> 元素
                // StringComparison.OrdinalIgnoreCase 表示比較字串時忽略大小寫，并且按照字母的 Unicode 編碼進行比較
                if (node.Name.Equals("optgroup", StringComparison.OrdinalIgnoreCase))
                {
                    // 獲取 optgroup label 名稱
                    string Name = node.GetAttributeValue("label", "").ToString().Trim();
                    // Console.WriteLine(Name);
                    optgroupNames.Add(Name);

                    // 排除第零筆(母節點Text)、第一筆(第一個optgroup)
                    if (index > 1)
                    {
                        // 把舊的 optgroup資料 存入 List
                        optgroups.Add(options);
                        // 清空品項 List
                        options = new List<string>();
                    }
                }
                // 檢查節點是否為 <option> 元素
                else if (node.Name.Equals("option", StringComparison.OrdinalIgnoreCase))
                {
                    string nodeText = node.InnerText.Trim();
                    options.Add(nodeText);
                }

                // 如果是 最後一個元素，就將品項 List 加入 optgroup List
                if (index == innerNodes.Count() - 1)
                {
                    // 把最後一組的 optgroup資料 存入 List
                    optgroups.Add(options);
                }
            }

            Console.WriteLine("optgroups、optgroupNames Data Get!");
        }

        /// <summary>
        /// 單一零件分類表資料更新
        /// </summary>
        /// <param name="CoolPCType"></param>
        /// <param name="TableType"></param>
        private void _UpdateComponentClassifications(int CoolPCType, int TableType)
        {
            // 獲取 硬體單一零件集合 List
            _GetComponentsList(CoolPCType);

            // 單一零件分類表 更新
            for (int i = 0; i < optgroupNames?.Count(); i++)
            {
                bool exist = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == TableType).Any(x => x.Name == optgroupNames[i]);

                if (!exist)
                {
                    _context.ComponentClassifications.Add(new ComponentClassification
                    {
                        ComputerPartCategoryId = TableType,
                        Name = optgroupNames[i]
                    });
                    Console.WriteLine($"Add {optgroupNames[i]}");
                }
                else
                {
                    Console.WriteLine($"Data Exist");
                }
            }
            _context.SaveChanges();
        }

        /// <summary>
        /// 全零件分類表更新
        /// </summary>
        public virtual void UpdateAllComponentClassifications()
        {
            // 原價屋零件表 序列
            int[] CoolPCType = { 3, 4, 5, 6, 7, 9, 10, 11, 13, 14, 28 };
            // 電腦零件類別表 序列
            int[] TableType = {
                (int)ComputerPartCategory.Type.CPU,
                (int)ComputerPartCategory.Type.MB,
                (int)ComputerPartCategory.Type.RAM,
                (int)ComputerPartCategory.Type.SSD,
                (int)ComputerPartCategory.Type.HDD,
                (int)ComputerPartCategory.Type.AirCooler,
                (int)ComputerPartCategory.Type.LiquidCooler,
                (int)ComputerPartCategory.Type.GPU,
                (int)ComputerPartCategory.Type.CASE,
                (int)ComputerPartCategory.Type.PSU,
                (int)ComputerPartCategory.Type.OS
            };

            // 爬取硬體品項集合
            _GetHardWareData();

            // 全零件分類表 更新
            for (int i = 0; i < CoolPCType.Length; i++)
            {
                _UpdateComponentClassifications(CoolPCType[i], TableType[i]);
            }
        }

        /// <summary>
        /// 主機板產品更新
        /// </summary>
        public virtual void UpdateMB()
        {

            _GetHardWareData();
            // 主機板 List
            _GetComponentsList(4);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 主機板，只需 Intel、AMD，且排除工作站、伺服器
                    if (!(optgroupNames[i].Substring(0, 7) == "Intel H" ||
                          optgroupNames[i].Substring(0, 7) == "Intel B" ||
                          optgroupNames[i].Substring(0, 7) == "Intel Z" ||
                          optgroupNames[i].Substring(0, 5) == "AMD B" ||
                          optgroupNames[i].Substring(0, 5) == "AMD A" ||
                          optgroupNames[i].Substring(0, 5) == "AMD X"))
                    {
                        continue;
                    }

                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 0;
                    if (optgroupNames[i].Substring(0, 7) == "Intel H" || optgroupNames[i].Substring(0, 7) == "Intel B" || optgroupNames[i].Substring(0, 7) == "Intel Z" || optgroupNames[i].Substring(0, 5) == "AMD X")
                    {
                        watts = 132;
                    }
                    else if (optgroupNames[i].Substring(0, 5) == "AMD B" || optgroupNames[i].Substring(0, 5) == "AMD A")
                    {
                        watts = 82;
                    }

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.MB).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{ComponentClassificationId} {optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 3) == "【狂】" ||
                            optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].Substring(0, 5) == "[任搭U]" ||
                            optgroups[i][j].Substring(0, 9) == "【任搭K版CPU】" ||
                            optgroups[i][j].IndexOf("[裝機價]") != -1)
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("(M-ATX") != -1 ? optgroups[i][j].IndexOf("(M-ATX") :
                                      optgroups[i][j].IndexOf("(ATX") != -1 ? optgroups[i][j].IndexOf("(ATX") :
                                      optgroups[i][j].IndexOf("(E-ATX") != -1 ? optgroups[i][j].IndexOf("(E-ATX") :
                                      optgroups[i][j].IndexOf("(Mini-ITX");
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd).Trim();
                        string Spec = optgroups[i][j].Substring(NameEnd, SpecEnd - NameEnd).Trim();
                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;

                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));

                        // 存入資料庫 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                        }
                    }
                    // 保存資料庫變更
                    _context.SaveChanges();
                }
            }

        }

        /// <summary>
        /// 固態硬碟產品更新
        /// </summary>
        public virtual void UpdateSSD()
        {

            _GetHardWareData();
            // 固態硬碟 List
            _GetComponentsList(6);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 3;

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.SSD).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{ComponentClassificationId} {optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("[狂加購]") != -1)
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);


                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("讀");
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        //// 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd).Trim();
                        string Spec = optgroups[i][j].Substring(NameEnd, SpecEnd - NameEnd).Trim();
                        //// 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;


                        int PriceEnd = PriceStr.IndexOf(" ");
                        if(PriceEnd == -1) { PriceEnd = PriceStr.Length; }
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));

                        //Console.WriteLine($"{Name} {Spec} {Price}");

                        // 存入資料庫 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                        }
                    }
                    // 保存資料庫變更
                    _context.SaveChanges();
                }
            }

        }


        /// <summary>
        /// 傳統硬碟產品更新
        /// </summary>
        public virtual void UpdateHDD()
        {

            _GetHardWareData();
            // 傳統硬碟 List
            _GetComponentsList(7);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 筆電版 排除
                    if (optgroupNames[i].Substring(0, 3) == "2.5") { continue; }


                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 20;

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.HDD).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{ComponentClassificationId} {optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("限網單") != -1)
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("】");
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd + 1).Trim();
                        string Spec = optgroups[i][j].Substring(NameEnd + 1, SpecEnd - NameEnd).Trim();
                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;

                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));

                        // Console.WriteLine($"{Name} {Spec} {Price}");

                        // 存入資料庫 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                        }
                    }
                    // 保存資料庫變更
                    _context.SaveChanges();
                }
            }

        }

        /// <summary>
        /// 風冷散熱器產品更新
        /// </summary>
        public virtual void UpdateAirCooler()
        {

            _GetHardWareData();
            // 風冷散熱器 List
            _GetComponentsList(9);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 清單排除
                    if (optgroupNames[i].Substring(0, 3) == "M.2" ||
                        optgroupNames[i] == "高效能散熱膏" ||
                        optgroupNames[i] == "矽膠導熱片" ||
                        optgroupNames[i] == "筆記型專用散熱座")
                    {
                        continue;
                    }


                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 5;

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.AirCooler).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{ComponentClassificationId} {optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("限網單") != -1 ||
                            optgroups[i][j].IndexOf("LGA17XX-SS2 扣具") != -1 ||
                            optgroups[i][j].IndexOf("【提醒】") != -1)
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("/");
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd).Trim();
                        string Spec = optgroups[i][j].Substring(NameEnd, SpecEnd - NameEnd).Trim();
                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;

                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));

                        //Console.WriteLine($"{Name} {Spec} {Price}");

                        // 存入資料庫 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                        }
                    }
                    // 保存資料庫變更
                    _context.SaveChanges();
                }
            }

        }

        /// <summary>
        /// 水冷散熱器產品更新
        /// </summary>
        public virtual void UpdateLiquidCooler()
        {

            _GetHardWareData();
            // 水冷散熱器 List
            _GetComponentsList(10);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 清單排除
                    if (optgroupNames[i] == "水冷套件-水冷液【客訂商品】" ||
                        optgroupNames[i] == "封閉式水冷")
                    {
                        continue;
                    }


                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 10;

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.LiquidCooler).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{ComponentClassificationId} {optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("優惠") != -1 ||
                            optgroups[i][j].IndexOf("W金牌") != -1)
                        {
                            continue;
                        }

                        // Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("/");
                        if (NameEnd == -1) { NameEnd = optgroups[i][j].IndexOf("【"); }
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd).Trim();
                        string Spec = optgroups[i][j].Substring(NameEnd, SpecEnd - NameEnd).Trim();
                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;

                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));


                        //Console.WriteLine($"{Name} {Spec} {Price}");

                        // 存入資料庫 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                        }
                    }
                    // 保存資料庫變更
                    _context.SaveChanges();
                }
            }

        }

        /// <summary>
        /// 機殼產品更新
        /// </summary>
        public virtual void UpdateCASE()
        {

            _GetHardWareData();
            // 機殼 List
            _GetComponentsList(13);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 清單排除
                    if (optgroupNames[i] == "特價 or 活動專區" ||
                        optgroupNames[i].IndexOf("工業機架式機殼") != -1)
                    {
                        continue;
                    }


                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 0;

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.CASE).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{ComponentClassificationId} {optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("電源(2年)") != -1 ||
                            optgroups[i][j].IndexOf("銅牌電源") != -1 ||
                            optgroups[i][j].IndexOf("金牌") != -1 ||
                            optgroups[i][j].IndexOf("50W") != -1 ||
                            optgroups[i][j].IndexOf("水冷") != -1 ||
                            optgroups[i][j].IndexOf("散熱器") != -1)
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("/");
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd).Trim();
                        string Spec = optgroups[i][j].Substring(NameEnd, SpecEnd - NameEnd).Trim();
                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;

                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));


                        //Console.WriteLine($"{Name} {Spec} {Price}");

                        // 存入資料庫 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                        }
                    }
                    // 保存資料庫變更
                    _context.SaveChanges();
                }
            }

        }

        /// <summary>
        /// 電源供應器產品更新
        /// </summary>
        public virtual void UpdatePSU()
        {

            _GetHardWareData();
            // 電源供應器 List
            _GetComponentsList(14);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 清單排除
                    if (optgroupNames[i] == "特價 or 活動專區")
                    {
                        continue;
                    }


                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 0;

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.PSU).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{ComponentClassificationId} {optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;")
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("牌");
                        if (NameEnd == -1) { NameEnd = optgroups[i][j].IndexOf("金"); }
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd + 1).Trim();
                        string Spec = optgroups[i][j].Substring(NameEnd + 1, SpecEnd - NameEnd).Trim();
                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;

                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));


                        //Console.WriteLine($"{Name} {Spec} {Price}");

                        // 存入資料庫 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                        }
                    }
                    // 保存資料庫變更
                    _context.SaveChanges();
                }
            }

        }

        /// <summary>
        /// 作業系統產品更新
        /// </summary>
        public virtual void UpdateOS()
        {

            _GetHardWareData();
            // 作業系統 List
            _GetComponentsList(28);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 清單排除
                    if (optgroupNames[i].IndexOf("Microsoft Windows") == -1)
                    {
                        continue;
                    }


                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 0;

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.OS).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{ComponentClassificationId} {optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("【搭購價】") != -1)
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("64位元");
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd).Trim();
                        string Spec = optgroups[i][j].Substring(NameEnd, SpecEnd - NameEnd).Trim();
                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;

                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));


                        //Console.WriteLine($"{Name} {Spec} {Price}");

                        // 存入資料庫 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                        }
                    }
                    // 保存資料庫變更
                    _context.SaveChanges();
                }
            }

        }


        /// <summary>
        /// 記憶體產品更新
        /// </summary>
        public virtual void UpdateRAM()
        {

            _GetHardWareData();
            // 記憶體 List
            _GetComponentsList(5);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 清單排除
                    if (optgroupNames[i].IndexOf("KLEVV科賦") != -1 ||
                        optgroupNames[i].IndexOf("筆記型") != -1 ||
                        optgroupNames[i].IndexOf("伺服器") != -1 ||
                        optgroupNames[i].IndexOf("DDR3") != -1)
                    {
                        continue;
                    }

                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 2;
                    if (optgroupNames[i].IndexOf("雙通道") != -1) { watts = 4; }
                    else if (optgroupNames[i].IndexOf("四通道") != -1) { watts = 8; }

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.RAM).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    //Console.WriteLine("-----------------------");
                    //Console.WriteLine($"{optgroupNames[i]} watts: {watts}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("限量") != -1)
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("▼");
                        if (NameEnd == -1) { NameEnd = optgroups[i][j].IndexOf(","); }
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        //Console.WriteLine($"{NameEnd}!{SpecEnd}!{PriceFirst}");

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd).Trim();
                        string Spec = "";
                        if (NameEnd != SpecEnd)
                        {
                            Spec = optgroups[i][j].Substring(NameEnd, SpecEnd - NameEnd).Trim();
                        }

                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst).Trim();
                        int Price = 0;
                        int PriceEnd = PriceStr.IndexOf(" ");
                        //Console.WriteLine($"{Name}!!!{PriceStr}");
                        if (PriceEnd != -1)
                        {
                            Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));
                        }
                        else { Price = int.Parse(PriceStr.Substring(1)); }

                        Console.WriteLine($"{Name}!!!{Spec}!!!{Price}");

                        // 記憶體容量
                        // 找到 XXGB 的索引值
                        int Sizeindex = optgroups[i][j].IndexOf("GB");
                        // 略過 RGB
                        if (optgroups[i][j][Sizeindex - 1] == 'R')
                        {
                            string temp = optgroups[i][j].Substring(Sizeindex + 1);
                            int newindex = temp.IndexOf("GB");
                            Sizeindex = Sizeindex + newindex + 1;
                        }
                        int SizeStart = Sizeindex - 3;
                        string SizeStr = optgroups[i][j].Substring(SizeStart, 3);
                        int Size = 0;
                        if (!int.TryParse(SizeStr, out Size))
                        {
                            SizeStr = SizeStr.Substring(1);
                            if (!int.TryParse(SizeStr, out Size))
                            {
                                SizeStr = SizeStr.Substring(1);
                                Size = int.Parse(SizeStr);
                            }
                        }

                        //Console.WriteLine($"{Name}~~~{Size}");

                        // 存入資料庫 => 產品資訊表 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                            // 保存資料庫變更
                            _context.SaveChanges();
                            // 存入資料庫
                            int ProductId = productInfo.ProductInformationId;
                            ProductRAM productRAM = new ProductRAM();
                            productRAM.ProductInformationId = ProductId;
                            productRAM.Size = Size;
                            _context.ProductRAMs.Add(productRAM);
                            _context.SaveChanges();
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                            // 保存資料庫變更
                            _context.SaveChanges();
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 顯示卡產品更新
        /// </summary>
        public virtual void UpdateGPU()
        {

            _GetHardWareData();
            // 顯示卡 List
            _GetComponentsList(11);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 清單排除
                    if (optgroupNames[i].IndexOf("NVIDIA / AMD 顯示卡周邊配件") != -1 ||
                        optgroupNames[i].IndexOf("NVIDIA / AMD 外接顯卡轉接盒") != -1 ||
                        optgroupNames[i].IndexOf("繪圖卡") != -1 ||
                        optgroupNames[i] == "NVIDIA GeForce 210" ||
                        optgroupNames[i] == "AMD R7 240 系列")
                    {
                        continue;
                    }

                    // 產品資訊 List 更新

                    // 瓦數
                    int watts = 0;
                    switch (optgroupNames[i])
                    {
                        case "INTEL ARC 顯示卡":
                            watts = 225;
                            break;
                        case "NVIDIA GT710":
                            watts = 19;
                            break;
                        case "NVIDIA GT730":
                            watts = 65;
                            break;
                        case "NVIDIA GT1030":
                            watts = 30;
                            break;
                        case "NVIDIA GTX1630 (DDR6)":
                            watts = 75;
                            break;
                        case "NVIDIA GTX1650 (DDR6)":
                            watts = 90;
                            break;
                        case "NVIDIA RTX3050-6GB":
                            watts = 70;
                            break;
                        case "NVIDIA RTX3050-8GB":
                            watts = 130;
                            break;
                        case "NVIDIA RTX3060-8GB":
                            watts = 170;
                            break;
                        case "NVIDIA RTX3060-12GB":
                            watts = 170;
                            break;
                        case "NVIDIA RTX4060-8GB":
                            watts = 115;
                            break;
                        case "NVIDIA RTX4060Ti-8GB":
                            watts = 160;
                            break;
                        case "NVIDIA RTX4060Ti-16G":
                            watts = 165;
                            break;
                        case "NVIDIA RTX4070-12GB":
                            watts = 200;
                            break;
                        case "NVIDIA RTX4070 SUPER 12GB":
                            watts = 220;
                            break;
                        case "NVIDIA RTX4070TI-12GB":
                            watts = 285;
                            break;
                        case "NVIDIA RTX4070Ti SUPER 16GB":
                            watts = 285;
                            break;
                        case "NVIDIA RTX4080-16GB":
                            watts = 320;
                            break;
                        case "NVIDIA RTX4080 SUPER 16GB":
                            watts = 320;
                            break;
                        case "NVIDIA RTX4090":
                            watts = 450;
                            break;
                        case "AMD RX6500XT":
                            watts = 107;
                            break;
                        case "AMD RX6600":
                            watts = 150;
                            break;
                        case "AMD RX6650XT":
                            watts = 180;
                            break;
                        case "AMD Radeon RX7600-8G":
                            watts = 166;
                            break;
                        case "AMD Radeon RX7600XT OC 16G":
                            watts = 215;
                            break;
                        case "AMD Radeon RX7700XT-12G":
                            watts = 245;
                            break;
                        case "AMD Radeon RX7800XT-16G":
                            watts = 263;
                            break;
                        case "AMD Radeon RX7900GRE-16G":
                            watts = 260;
                            break;
                        case "AMD Radeon RX7900XT-20G":
                            watts = 300;
                            break;
                        case "AMD Radeon RX7900XTX-24G":
                            watts = 355;
                            break;
                    }

                    // GPU跑分ID
                    int GPUID = 10000;
                    SteamNexus_Server.Models.GPU? GPUitem = null;
                    try
                    {
                        switch (optgroupNames[i])
                        {
                            case "INTEL ARC 顯示卡":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("Arc A750")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA GT710":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("GT 710")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA GT730":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("GT 730")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA GT1030":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("GT 1030")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA GTX1630 (DDR6)":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("GTX 1630")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA GTX1650 (DDR6)":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("GTX 1650")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX3050-6GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 3050")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX3050-8GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 3050")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX3060-8GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 3060 8GB")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX3060-12GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 3060 12GB")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4060-8GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4060")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4060Ti-8GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4060 Ti")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4060Ti-16G":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4060 Ti 16G")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4070-12GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4070")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4070 SUPER 12GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4070 SUPER")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4070TI-12GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4070 Ti")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4070Ti SUPER 16GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4070 Ti SUPER")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4080-16GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4080")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4080 SUPER 16GB":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4080 SUPER")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "NVIDIA RTX4090":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RTX 4090")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD RX6500XT":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 6500 XT")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD RX6600":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 6600")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD RX6650XT":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 6650 XT")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD Radeon RX7600-8G":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 7600")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD Radeon RX7600XT OC 16G":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 7600 XT")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD Radeon RX7700XT-12G":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 7700 XT")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD Radeon RX7800XT-16G":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 7800 XT")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD Radeon RX7900GRE-16G":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 7900 GRE")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD Radeon RX7900XT-20G":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 7900 XT")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                            case "AMD Radeon RX7900XTX-24G":
                                GPUitem = _context.GPUs.Where(x => x.Name.Contains("RX 7900 XTX")).FirstOrDefault();
                                GPUID = GPUitem?.GPUId ?? 10000;
                                break;
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error.Message);
                        return;
                    }

                    // Console.WriteLine($"{optgroupNames[i]} watts: {watts} GPUID: {GPUID}");

                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.GPU).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    Console.WriteLine("-----------------------");
                    Console.WriteLine($"{optgroupNames[i]}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("搭機") != -1 ||
                            optgroups[i][j].IndexOf("INTEL 原廠卡 ARC A750 8G(2050MHz/27cm/三年保)限量版顯示卡-任搭優惠") != -1)
                        {
                            continue;
                        }

                        // Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("(");
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd).Trim();
                        string Spec = "";
                        if (NameEnd != SpecEnd)
                        {
                            Spec = optgroups[i][j].Substring(NameEnd, SpecEnd - NameEnd).Trim();
                        }

                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;
                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));

                        //Console.WriteLine($"{Name} {Spec} {Price}");

                        // 存入資料庫 => 產品資訊表 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                            // 保存資料庫變更
                            _context.SaveChanges();
                            // 存入資料庫 => ProductGPUs
                            int ProductId = productInfo.ProductInformationId;
                            ProductGPU productGPU = new ProductGPU();
                            productGPU.ProductInformationId = ProductId;
                            productGPU.GPUId = GPUID;
                            _context.ProductGPUs.Add(productGPU);
                            _context.SaveChanges();
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                            // 保存資料庫變更
                            _context.SaveChanges();
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 中央處理器產品更新
        /// </summary>
        public virtual void UpdateCPU()
        {
            _GetHardWareData();
            // 中央處理器 List
            _GetComponentsList(3);

            // 檢測該 optgroups List 是否有資料
            if (optgroupNames == null || optgroups == null)
            {
                Console.WriteLine("optgroups is null");
                return;
            }
            else
            {
                // 資料更新
                for (int i = 0; i < optgroupNames.Count(); i++)
                {
                    // 清單排除
                    if (optgroupNames[i].IndexOf("Sapphire Rapids Xeon") != -1 ||
                        optgroupNames[i].IndexOf("TRX") != -1 ||
                        optgroupNames[i].IndexOf("WRX") != -1)
                    {
                        continue;
                    }

                    // 產品資訊 List 更新


                    // ComponentClassificationId
                    var ComponentClassification = _context.ComponentClassifications.Where(x => x.ComputerPartCategoryId == (int)ComputerPartCategory.Type.CPU).Where(x => x.Name == optgroupNames[i]).FirstOrDefault();
                    if (ComponentClassification == null) { continue; }
                    int ComponentClassificationId = ComponentClassification.ComponentClassificationId;

                    Console.WriteLine("-----------------------");
                    Console.WriteLine($"{optgroupNames[i]}");

                    for (int j = 0; j < optgroups[i].Count(); j++)
                    {
                        // 例外排除
                        if (optgroups[i][j].Substring(0, 8) == "&#x2764;" ||
                            optgroups[i][j].Substring(0, 8) == "&#x21AA;" ||
                            optgroups[i][j].IndexOf("Processor") != -1)
                        {
                            continue;
                        }
                        if(optgroups[i][j].Substring(0, 3) != "AMD" &&
                            optgroups[i][j].Substring(0, 5) != "Intel")
                        {
                            continue;
                        }

                        //Console.WriteLine(optgroups[i][j]);

                        // 找到各段資訊的索引值斷點
                        int NameEnd = optgroups[i][j].IndexOf("】");
                        int SpecEnd = optgroups[i][j].IndexOf(",");
                        int PriceFirst = optgroups[i][j].LastIndexOf("$");

                        if (NameEnd == -1 || SpecEnd == -1 || PriceFirst == -1) { continue; }

                        // 名稱、規格
                        string Name = optgroups[i][j].Substring(0, NameEnd+1).Trim();
                        string Spec = "";
                        if (NameEnd != SpecEnd)
                        {
                            Spec = optgroups[i][j].Substring(NameEnd+1, SpecEnd - NameEnd-1).Trim();
                        }

                        // 價格
                        string PriceStr = optgroups[i][j].Substring(PriceFirst);
                        int Price = 0;
                        int PriceEnd = PriceStr.IndexOf(" ");
                        Price = int.Parse(PriceStr.Substring(1, PriceEnd - 1));

                        //Console.WriteLine($"{Name}!!{Spec}!!{Price}");

                        // 瓦數
                        int watts = 0;
                        if(optgroups[i][j].IndexOf("Intel") != -1)
                        {
                            if (optgroups[i][j].IndexOf("K") != -1)
                            {
                                watts = 125;
                            }
                            else
                            {
                                watts = 65;
                            }
                        }
                        else if(optgroups[i][j].IndexOf("AMD") != -1)
                        {
                            if (optgroups[i][j].IndexOf("X3D") != -1)
                            {
                                watts = 120;
                            }
                            else if(optgroups[i][j].IndexOf("900X") != -1 || optgroups[i][j].IndexOf("950X") != -1)
                            {
                                watts = 170;
                            }
                            else if (optgroups[i][j].IndexOf("X") != -1)
                            {
                                watts = 105;
                            }
                            else
                            {
                                watts = 65;
                            }
                        }

                        //Console.WriteLine($"{Name}!!{watts}");

                        // CPU跑分 ID
                        // 擷取 有效搜尋字串
                        string CPUstr = "00000";
                        if(optgroups[i][j].IndexOf("Intel") != -1)
                        {
                            // Intel CPU
                            int CPUStart = optgroups[i][j].IndexOf("-")+1;
                            int CPUstrNum = optgroups[i][j].Substring(CPUStart).IndexOf("【");
                            CPUstr = optgroups[i][j].Substring(CPUStart,CPUstrNum).Trim();
                            if (CPUstr.IndexOf("特別版") != -1)
                            {
                                CPUstr = CPUstr.Substring(0,CPUstr.Length-3).Trim();
                            }
                        }
                        else
                        {
                            // AMD CPU
                            int CPUStart = optgroups[i][j].IndexOf("R") + 3;
                            int CPUstrNum = optgroups[i][j].IndexOf("【") - CPUStart;
                            CPUstr = optgroups[i][j].Substring(CPUStart, CPUstrNum).Trim();
                            if (CPUstr.IndexOf(" ") != -1)
                            {
                                CPUstr = CPUstr.Substring(0, CPUstr.IndexOf(" ")).Trim();
                            }
                            else if(CPUstr.IndexOf("代理盒裝") != -1)
                            {
                                CPUstr = CPUstr.Substring(0, CPUstr.Length - 4).Trim();
                            }
                            else if (CPUstr.IndexOf("盒") != -1)
                            {
                                CPUstr = CPUstr.Substring(0, CPUstr.Length - 1).Trim();
                            }
                            CPUstr = " " + CPUstr;
                        }
                        var CPUitem = _context.CPUs.Where(x=>x.Name.Contains(CPUstr)).FirstOrDefault();
                        int CPUID = CPUitem?.CPUId ?? 10000;

                        // Console.WriteLine($"{CPUstr}!!{CPUID}");

                        // 存入資料庫 => 產品資訊表 Create or Update
                        var item = _context.ProductInformations.Where(x => x.ComponentClassificationId == ComponentClassificationId)
                            .Where(x => x.Name == Name).FirstOrDefault();

                        if (item == null)
                        {
                            // Create
                            ProductInformation productInfo = new ProductInformation();
                            productInfo.ComponentClassificationId = ComponentClassificationId;
                            productInfo.Name = Name;
                            productInfo.Specification = Spec;
                            productInfo.Price = Price;
                            productInfo.Wattage = watts;
                            _context.ProductInformations.Add(productInfo);
                            // 保存資料庫變更
                            _context.SaveChanges();
                            // 存入資料庫 => ProductGPUs
                            int ProductId = productInfo.ProductInformationId;
                            ProductCPU productCPU = new ProductCPU();
                            productCPU.ProductInformationId = ProductId;
                            productCPU.CPUId = CPUID;
                            _context.ProductCPUs.Add(productCPU);
                            _context.SaveChanges();
                        }
                        else
                        {
                            // Update
                            item.Specification = Spec;
                            item.Price = Price;
                            // 保存資料庫變更
                            _context.SaveChanges();
                        }
                    }
                }
            }
        }

        // 硬體資料全更新
        public virtual void UpdateAll()
        {
            UpdateAllComponentClassifications();

            UpdateCPU();
            UpdateGPU();
            UpdateRAM();
            UpdateMB();
            UpdateSSD();
            UpdateHDD();
            UpdateAirCooler();
            UpdateLiquidCooler();
            UpdateCASE();
            UpdatePSU();
            UpdateOS();
        }






        // 測試用
        public virtual void test()
        {
            _GetHardWareData();
            _GetComponentsList(4);

            if (optgroups != null && optgroupNames != null)
            {
                Console.WriteLine(optgroups.Count());
                Console.WriteLine(optgroupNames.Count());
            }

        }

    }
}
