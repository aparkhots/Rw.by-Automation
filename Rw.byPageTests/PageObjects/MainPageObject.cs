using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Rw.byPageTests.PageObjects
{
    class MainPageObject
    {
        private IWebDriver _webDriver;

        public MainPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _webDriver.Navigate().GoToUrl("https://www.rw.by/");
        }

        public MainPageObject ChooseLaguage(string lang)
        {
            _webDriver.FindElement(By.XPath($"//a[@href='/{lang}/']")).Click();
            Thread.Sleep(200);
            return new MainPageObject(_webDriver);
        }

        public void CheckNewsCount(int minNewsCount)
        {
            var newsCount = _webDriver
                .FindElement(By.ClassName("index-news-list"))
                .FindElements(By.TagName("dt"))
                .Count;

            Assert.IsTrue(newsCount >= minNewsCount);
        }

        public void CheckCopyright(string copyrightString)
        {
            var copyright = _webDriver
                .FindElement(By.ClassName("copyright"))
                .Text
                .Contains(copyrightString);

            Assert.IsTrue(copyright);
        }


        public void CheckButtonsPresent()
        {
            var menuItemsButtons = _webDriver
                .FindElement(By.ClassName("menu-items"))
                .Text;

            Assert.IsTrue(menuItemsButtons.Contains("PRESS CENTER"));
            Assert.IsFalse(menuItemsButtons.Contains("Timetable"));
            Assert.IsTrue(menuItemsButtons.ToLower().Contains("passenger services"));
            Assert.IsTrue(menuItemsButtons.ToLower().Contains("freight"));
            Assert.IsTrue(menuItemsButtons.ToLower().Contains("corporate"));
            Assert.IsTrue(menuItemsButtons.ToLower().Contains("tickets"));
        }

        public SearcherPageObject SearchInput(string searchingString)
        {
            _webDriver.FindElement(By.Id("searchinp")).SendKeys(searchingString);
            _webDriver.FindElement(By.XPath("//button[@type = 'submit']")).Click();
            return new SearcherPageObject(_webDriver);
        }

        public SchedulePageObject SearchTrain(string trainFrom, string trainTo, int days)
        {
            _webDriver
                .FindElement(By.Id("acFrom"))
                .SendKeys(trainFrom);
            _webDriver
                .FindElement(By.Id("acTo"))
                .SendKeys(trainTo);


            var dateTimeForCalendar = DateTime.Today.AddDays(days).ToString("dd.MM.yyyy") + "\n";

            _webDriver.FindElement(By.Id("yDate")).SendKeys(dateTimeForCalendar);
            _webDriver.FindElement(By.XPath("//input[@type = 'submit']")).Click();

            return new SchedulePageObject(_webDriver);
        }

    }
}
