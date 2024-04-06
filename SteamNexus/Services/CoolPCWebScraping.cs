using HtmlAgilityPack;
using System.Text;
using SteamNexus.Data;
using SteamNexus.Models;
using System.Xml.Serialization;

namespace SteamNexus.Services
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

        // 主機板
        public virtual void UpdateComponentData()
        {
            
            _GetHardWareData();
            // 主機板 List
            _GetComponentsList(4);



        }


        // 測試用
        public virtual void test()
        {
            _GetComponentsList(6);

            if (optgroups != null && optgroupNames != null)
            {
                Console.WriteLine(optgroups[0].Count());
                Console.WriteLine(optgroupNames.Count());
            }

        }




        // 爬蟲初次測試
        public void First()
        {
            //// 才能讀到 big5
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            //// HTML 資料讀取
            //string url = @"https://coolpc.com.tw/evaluate.php";
            //HtmlWeb web = new HtmlWeb();
            //web.OverrideEncoding = Encoding.GetEncoding("big5");
            //HtmlDocument doc = web.Load(url);

            //// DocumentNode 屬性則表示 HTML 文檔的根節點
            //// Descendants("元素") 會尋找指定標籤的所有子元素

            //// 返回所有 optionGroup 的集合
            //var optionGroupNodes = doc.DocumentNode.Descendants("optgroup");
            //// GetAttributeValue("屬性名稱","預設值(找不到返回的值)") 
            //var optionGroupCPU = optionGroupNodes.Where(node => node.GetAttributeValue("label","No Data") == "Intel Raptor Lake-s 14代1700 腳位").FirstOrDefault();

            //List<string> CPU_14series = new List<string>();
            //List<string> CPU_12series = new List<string>();

            //if(optionGroupCPU != null)
            //{
            //    var optionNodes = optionGroupCPU.Descendants("option");
            //    foreach( var node in optionNodes)
            //    {
            //        string content = node.InnerText;


            //        // 抓 14代 CPU 
            //        if (content.Substring(0,11) == "Intel i3-14" || content.Substring(0,11) == "Intel i5-14" || content.Substring(0,11) == "Intel i7-14" || content.Substring(0, 11) == "Intel i9-14")
            //        {
            //            CPU_14series.Add(content);
            //        }
            //        // 抓 12代 CPU
            //        else if (content.Substring(0, 11) == "Intel i3-12" || content.Substring(0, 11) == "Intel i5-12" || content.Substring(0, 11) == "Intel i7-12" || content.Substring(0, 11) == "Intel i9-12")
            //        {
            //            CPU_12series.Add(content);
            //        }
            //    }
            //}

            //Console.WriteLine("---------------------- 12代 CPU ----------------------");
            //for (int i = 0; i < CPU_12series.Count();i++)
            //{
            //    Console.WriteLine(CPU_12series[i]);
            //}
            //Console.WriteLine("---------------------- 14代 CPU ----------------------");
            //for (int i = 0; i < CPU_14series.Count(); i++)
            //{
            //    Console.WriteLine(CPU_14series[i]);
            //}
        }

    }
}
