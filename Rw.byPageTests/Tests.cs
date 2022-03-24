using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System.Collections.Generic;
using System.IO.Compression;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Rw.byPageTests.PageObjects;

namespace Rw.byPageTests
{
    public class Tests
    {
        private IWebDriver _webDriver;

        [TestCase("chrome")]
        [TestCase("edge")]
        public void Test1_OpeningSiteFromGoogle(string browser)
        {
            _webDriver = Handlers.ChooseBrowser(browser);

            var googleSearcher = new GoogleSearcherPageObject(_webDriver);
            googleSearcher.Searching("белорусская железная дорога");

            Handlers.CheckLoading(_webDriver);
        }

        [TestCase("chrome")]
        [TestCase("edge")]
        public void Test2_WorkWithTheMainPage(string browser)
        {
            _webDriver = Handlers.ChooseBrowser(browser);

            var mainPage = new MainPageObject(_webDriver);

            mainPage.ChooseLaguage("en");
            mainPage.CheckNewsCount(4);
            mainPage.CheckCopyright("© 2022 Belarusian Railway");
            mainPage.CheckButtonsPresent();
        }


        [TestCase("chrome")]
        [TestCase("edge")]
        public void Test3_WorkWithSearch(string browser)
        {
            _webDriver = Handlers.ChooseBrowser(browser);

            var mainPage = new MainPageObject(_webDriver);

            var randomString = Handlers.GenerateRandomString();

            var searcherPage = mainPage.SearchInput(randomString);

            searcherPage.CheckUrl(randomString);
            searcherPage.CheckNoteText("К сожалению, на ваш поисковый запрос ничего не найдено.");

            var results = searcherPage.Search("Санкт-Петербург");

            Handlers.GoToConsole(results);

            searcherPage.CheckResultsCount(results.Count, 15);
        }

        [TestCase("chrome")]
        [TestCase("edge")]
        public void Test4_WorkWithCalendar(string browser)
        {
            _webDriver = Handlers.ChooseBrowser(browser);

            var mainPage = new MainPageObject(_webDriver);


            var schedulePage = mainPage.SearchTrain("Минск-Пассажирский","Брест-Центральный",5);
            schedulePage.TrainsScheduleConsoleResult();

            var trainInformationPage = schedulePage.FirstResultTrain();
            trainInformationPage.CheckTrainTitleVisibility();
            trainInformationPage.CheckCruiseTimeTable();

            _webDriver.FindElement(By.XPath("//img[@alt = 'БелЖД']")).Click();

            Handlers.CheckLoading(_webDriver);
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }
    }
}