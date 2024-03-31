using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using System.Text;

namespace SteamNexus.Controllers
{
    public class PCBuilderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        // 原價屋網頁爬蟲 從 CLIENT 端 呼叫測試

        // POST: /PCBuilder/WebScrabingTest
        [HttpPost]
        public string WebScrabingTest()
        {
            // 才能讀到 big5
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // HTML 資料讀取
            string url = @"https://coolpc.com.tw/evaluate.php";
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.GetEncoding("big5");
            HtmlDocument doc = web.Load(url);

            // DocumentNode 屬性則表示 HTML 文檔的根節點
            // Descendants("元素") 會尋找指定標籤的所有子元素

            // 返回所有 optionGroup 的集合
            var optionGroupNodes = doc.DocumentNode.Descendants("optgroup");
            // GetAttributeValue("屬性名稱","預設值(找不到返回的值)") 
            var optionGroupCPU = optionGroupNodes.Where(node => node.GetAttributeValue("label","No Data") == "Intel Raptor Lake-s 14代1700 腳位").FirstOrDefault();

            List<string> CPU_14series = new List<string>();
            List<string> CPU_12series = new List<string>();

            if(optionGroupCPU != null)
            {
                var optionNodes = optionGroupCPU.Descendants("option");
                foreach( var node in optionNodes)
                {
                    string content = node.InnerText;


                    // 抓 14代 CPU 
                    if (content.Substring(0,11) == "Intel i3-14" || content.Substring(0,11) == "Intel i5-14" || content.Substring(0,11) == "Intel i7-14" || content.Substring(0, 11) == "Intel i9-14")
                    {
                        CPU_14series.Add(content);
                    }
                    // 抓 12代 CPU
                    else if (content.Substring(0, 11) == "Intel i3-12" || content.Substring(0, 11) == "Intel i5-12" || content.Substring(0, 11) == "Intel i7-12" || content.Substring(0, 11) == "Intel i9-12")
                    {
                        CPU_12series.Add(content);
                    }
                }
            }
            
            Console.WriteLine("---------------------- 12代 CPU ----------------------");
            for (int i = 0; i < CPU_12series.Count();i++)
            {
                Console.WriteLine(CPU_12series[i]);
            }
            Console.WriteLine("---------------------- 14代 CPU ----------------------");
            for (int i = 0; i < CPU_14series.Count(); i++)
            {
                Console.WriteLine(CPU_14series[i]);
            }


            return "Run Success";
        }


    }
}
