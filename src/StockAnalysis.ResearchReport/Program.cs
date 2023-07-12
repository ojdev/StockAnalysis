// See https://aka.ms/new-console-template for more information
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Edge;
using System;
using System.Text;

var url = "https://data.eastmoney.com/jgdy/tj.html";
var options = new EdgeOptions();
options.AddArguments("headless");
using (IWebDriver driver = new EdgeDriver(options))
{
    driver.Navigate().GoToUrl(url);
    var next = true;
    var path = "D:\\调研.csv";
    string header = "序号,代码,名称,最新价,涨跌幅,接待机构数量,接待方式,接待人员,接待地点,接待日期,公告日期";//12个列
    await File.WriteAllTextAsync(path, header + Environment.NewLine, Encoding.UTF8);
    do
    {
        var source = driver.PageSource;
        var doc = new HtmlDocument();
        doc.LoadHtml(source);
        #region 表格内容
        var table = doc.DocumentNode.SelectSingleNode("//*[@id=\"dataview\"]/div[@class=\"dataview-center\"]/div[@class=\"dataview-body\"]/table");
        foreach (var row in table.SelectNodes(".//tr").Skip(1))
        {
            var index = 1;
            List<string> rowText = new List<string>();
            foreach (var cell in row.SelectNodes("th|td"))
            {
                switch (index)
                {
                    case 1:
                        {
                            rowText.Add(cell.InnerText);
                            break;
                        }
                    case 2:
                        {
                            rowText.Add(cell.InnerText);
                            break;
                        }
                    case 3:
                        {
                            rowText.Add(cell.InnerText);
                            break;
                        }
                    case 4: break;
                    case 5:
                        {
                            rowText.Add(cell.InnerText);
                            break;
                        }
                    case 6:
                        {
                            rowText.Add(cell.InnerText);
                            break;
                        }
                    case 7:
                        {
                            rowText.Add(cell.InnerText);
                            break;
                        }
                    case 8:
                        {
                            var att = cell.SelectSingleNode("span").GetAttributeValue("title", "");
                            rowText.Add(att);
                            break;
                        }
                    case 9:
                        {
                            var att = cell.SelectSingleNode("span").GetAttributeValue("title", "");
                            rowText.Add(att);
                            break;
                        }
                    case 10:
                        {
                            var att = cell.SelectSingleNode("span").GetAttributeValue("title", "");
                            rowText.Add(att);
                            break;
                        }
                    case 11:
                        {
                            rowText.Add(cell.InnerText);
                            break;
                        }
                    case 12:
                        {
                            var att = cell.SelectSingleNode("span").GetAttributeValue("title", "");
                            rowText.Add(att);
                            break;
                        }
                    default: break;
                };
                //1//1
                //2//< a href = "//quote.eastmoney.com/unify/r/1.603666" > 603666 </ a >
                //3//< a href = "/stockdata/603666.html" >< span title = "亿嘉和" > 亿嘉和 </ span ></ a >
                //4//< a target = "_blank" href = "/jgdy/dyxx/603666,2023-06-01.html" style = "color:red;cursor:pointer;" > 详细 </ a > &nbsp;< a href = "/stockdata/603666.html" > 数据 </ a > &nbsp;< a href = "//guba.eastmoney.com/list,603666.html" > 股吧 </ a >
                //5//< span class= "green" > 40.89 </ span >
                //6//< span class= "green" > -1.92 %</ span >
                //7//151
                //8//< span title = "特定对象调研,现场参观,电话会议,网络会议,一对一沟通" > 特定对象调研...</ span >
                //9//< span title = "副总、董秘 张晋博,证券事务代表 杨赟" > 副总、董秘...</ span >
                //10//< span title = "电话会议,公司现场交流,上海、长沙等地" > 电话会议,...</ span >
                //11//2023 - 06 - 01
                //12//< span title = "2023-07-07" > 07 / 07 </ span >

                //Console.WriteLine(cell.InnerHtml + "\t");
                index++;
            }
            Console.WriteLine(string.Join(",", rowText));
            await File.AppendAllTextAsync(path, string.Join(",", rowText) + Environment.NewLine, Encoding.UTF8);
        }
        #endregion

        #region 翻页判断
        var pagerbox = doc.DocumentNode.SelectSingleNode("//*[@id=\"dataview\"]/div[@class=\"dataview-pagination tablepager\"]/div[@class=\"pagerbox\"]");
        var currentPage = pagerbox.SelectSingleNode("a[@class=\"active\"]");

        var firstPage = pagerbox.SelectNodes("a").Where(t => !new string[] { "上一页", "下一页" }.Contains(t.InnerText)).LastOrDefault();
        if (currentPage == null || firstPage == null || currentPage.InnerText == firstPage.InnerText)
        {
            next = false;
        }
        else
        {
            await Task.Delay(1000);
            driver.FindElement(By.XPath("//*[@id=\"dataview\"]/div[@class=\"dataview-pagination tablepager\"]/div[@class=\"pagerbox\"]/a[last()]")).Click();
            await Task.Delay(1500);
        }
        #endregion
    } while (next);
    driver.Quit();
    Console.WriteLine("Hello, World!");
}