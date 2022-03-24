using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace Rw.byPageTests
{
    class Handlers
    {
        

        public static string GenerateRandomString(int size = 20)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < size; i++)
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

        public static IWebDriver Chrome()
        {
            var driverOptionsChrome = new ChromeOptions();
            driverOptionsChrome.AddArgument(" --no-sandbox");
            return new ChromeDriver(driverOptionsChrome);
        }

        public static IWebDriver Edge()
        {
            var driverOptionsEdge = new EdgeOptions();
            driverOptionsEdge.AddArgument(" --no-sandbox");
            return new EdgeDriver(driverOptionsEdge);
        }
        public static IWebDriver ChooseBrowser(string browser)
        {
            IWebDriver choosenDriver = null;
            switch (browser)
            {
                case "chrome":
                    choosenDriver = Chrome();
                    break;
                case "edge":
                    choosenDriver = Edge();
                    break;
            }

            return choosenDriver;
        }

        public static void CheckLoading(IWebDriver driver)
        {
            Thread.Sleep(500);
            int firstCountElements = driver.FindElements(By.TagName("div")).Count;
            Thread.Sleep(500);
            int secondCountElements = driver.FindElements(By.TagName("div")).Count;
            Assert.AreEqual(firstCountElements,secondCountElements);
        }
    }
}
