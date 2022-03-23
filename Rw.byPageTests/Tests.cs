using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V85.IndexedDB;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace Rw.byPageTests
{
    public class Tests
    {
        private IWebDriver driver;

        public static IEnumerable<IWebDriver> SetBrowsers
        {
            get
            {
                {
                    var driverOptionsChrome = new ChromeOptions();
                    driverOptionsChrome.AddArgument(" --no-sandbox");
                    yield return new ChromeDriver(driverOptionsChrome);
                }

                {
                    var driverOptions = new EdgeOptions();
                    driverOptions.AddArgument(" --no-sandbox");
                    yield return new EdgeDriver(driverOptions);
                }

            }
        }

        [TestCaseSource(nameof(SetBrowsers))]

        public void Test1_OpeningSiteFromGoogle(IWebDriver driver)
        {
            By googleSearchInput = By.XPath("//input[@name = 'q']");
            By googleSearchButton = By.XPath("//input[@name= 'btnK']");

            driver.Navigate().GoToUrl("https://www.google.com");

            driver.FindElement(googleSearchInput).SendKeys("белорусская железная дорога");

            Thread.Sleep(300);

            driver.FindElement(googleSearchButton).Click();

            driver.FindElement(By.PartialLinkText("www.rw.by")).Click();

            //how to Check the page is load fine?
            
            driver.Quit();
        }

        [Test]
        public void Test2_WorkWithTheMainPage()
        {

            var driverOptionsChrome = new ChromeOptions();
            driverOptionsChrome.AddArgument(" --no-sandbox");
            driver = new ChromeDriver(driverOptionsChrome);

            driver.Navigate().GoToUrl("https://www.rw.by/");

            driver.FindElement(By.XPath("//a[@href='/en/']")).Click();

            Thread.Sleep(200);

            var newsCount = driver.FindElement(By.ClassName("index-news-list")).FindElements(By.TagName("dt")).Count;
            Assert.That(newsCount >= 4);

            var copyright = driver
                .FindElement(By.ClassName("copyright"))
                .Text
                .Contains("© 2022 Belarusian Railway");

            Assert.IsTrue(copyright);


            var menuItemsButtons = driver
                .FindElement(By.ClassName("menu-items"))
                .Text;

            Assert.IsTrue(menuItemsButtons.Contains("PRESS CENTER"));
            Assert.IsFalse(menuItemsButtons.Contains("Timetable"));
            Assert.IsTrue(menuItemsButtons.ToLower().Contains("passenger services"));
            Assert.IsTrue(menuItemsButtons.ToLower().Contains("freight"));
            Assert.IsTrue(menuItemsButtons.ToLower().Contains("corporate"));
            Assert.IsTrue(menuItemsButtons.ToLower().Contains("tickets"));
            driver.Quit();
        }

        public static string GenerateRandomString(int size = 20)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();

            for(int i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }

        public static void GoToConsole(IReadOnlyCollection<IWebElement> resultsOnPageCount)
        {
            foreach (var result in resultsOnPageCount)
            {
                Console.WriteLine(result.Text);
                Console.WriteLine("__________________________________");
            }

        }

        [Test]
        public void Test3_WorkWithSearch()
        {

            var driverOptionsChrome = new ChromeOptions();
            driverOptionsChrome.AddArgument(" --no-sandbox");
            driver = new ChromeDriver(driverOptionsChrome);

            var randomString = GenerateRandomString();

            driver.Navigate().GoToUrl("https://www.rw.by");
            driver.FindElement(By.XPath("//input[@id = 'searchinp']")).SendKeys(randomString);

            driver.FindElement(By.XPath("//button[@type = 'submit']")).Click();

            Assert.IsTrue(driver.Url == string.Concat("https://www.rw.by/search/?s=Y&q=", randomString));

            var warningMessage = driver.FindElement(By.ClassName("notetext")).Text;
            
            Assert.IsTrue(warningMessage == "К сожалению, на ваш поисковый запрос ничего не найдено.");

            var searher = driver.FindElement(By.XPath("//input[@id = 'searchinpm']"));
            searher.Clear();
            searher.SendKeys("Санкт-Петербург");

            driver.FindElement(By.XPath("//input[@type = 'submit']")).Click();

            Thread.Sleep(500);

            var resultsOnPage = driver.FindElement(By.XPath("//ol[@class = 'search-result']")).FindElements(By.TagName("li"));

            Assert.That(resultsOnPage.Count == 15);
            
            GoToConsole(resultsOnPage);
            driver.Quit();

        }

        [Test]
        public void Test4_WorkWithCalendar()
        {

            var driverOptionsChrome = new ChromeOptions();
            driverOptionsChrome.AddArgument(" --no-sandbox");
            driver = new ChromeDriver(driverOptionsChrome);

            driver.Navigate().GoToUrl("https://www.rw.by");

            driver
                .FindElement(By.XPath("//input[@id = 'acFrom']"))
                .SendKeys("Минск-Пассажирский");
            driver
                .FindElement(By.XPath("//input[@id = 'acTo']"))
                .SendKeys("Брест-Центральный");


            var dateTimeForCalendar = DateTime.Today.AddDays(5).ToString("dd.MM.yyyy") + "\n";

            driver.FindElement(By.XPath("//input[@id = 'yDate']")).SendKeys(dateTimeForCalendar);
            driver.FindElement(By.XPath("//input[@type = 'submit']")).Click();

            var trainsCount = driver.FindElements(By.XPath("//div[@data-train-info]")).Count;


            

            var trainsFrom = driver.FindElements(By.XPath("//div[@class = 'sch-table__station train-from-name']"));

            var trainsTo = driver.FindElements(By.XPath("//div[@class = 'sch-table__station train-to-name']"));

            var trainsTime = driver.FindElements(By.XPath("//div[@class = 'sch-table__time train-from-time']"));

            for (int i = 0; i < trainsCount; i++)
            {
                Console.WriteLine($"'{trainsFrom[i].Text} - {trainsTo[i].Text}' - {trainsTime[i].Text}");
            }

            driver.FindElements(By.XPath("//span[@class = 'train-route']")).First().Click();

            var trainTitleVisible = driver.FindElement(By.XPath("//div[@class = 'sch-title__title h2']")).Displayed;

            Assert.IsTrue(trainTitleVisible);

            var cruiseTimetable = driver
                .FindElement(By.XPath("//div[@class = 'sch-title__descr']"))
                .Text[19..];

            Assert.IsFalse(string.IsNullOrWhiteSpace(cruiseTimetable));

            driver.FindElement(By.XPath("//img[@alt = 'БелЖД']")).Click();

            //how to Check the page is load fine?
            driver.Quit();
        }

        //[TearDown]
        //public void TearDown()
        //{
        //    driver.Quit();
        //}
    }
}