using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework.Constraints;
using Rw.byPageTests.PageObjects;

namespace Rw.byPageTests
{
    public class Tests
    {
        private IWebDriver _webDriver;

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
            _webDriver = driver;
            var googleSearcher = new GoogleSearcherPageObject(_webDriver);
            googleSearcher.Searching();

            //how to Check the page is load fine?
        }

        [TestCaseSource(nameof(SetBrowsers))]
        public void Test2_WorkWithTheMainPage(IWebDriver driver)
        {
            _webDriver = driver;
            var mainPage = new MainPageObject(_webDriver);

            mainPage.ChooseLaguage("en");
            mainPage.CheckNewsCount();
            mainPage.CheckCopyright();
            mainPage.CheckButtonsPresent();
        }



        [TestCaseSource(nameof(SetBrowsers))]
        public void Test3_WorkWithSearch(IWebDriver driver)
        {
            _webDriver = driver;
            var mainPage = new MainPageObject(_webDriver);

            var randomString = Handlers.GenerateRandomString();

            var searcherPage = mainPage.SearchInput(randomString);

            searcherPage.CheckUrl(randomString);
            searcherPage.CheckNoteText("К сожалению, на ваш поисковый запрос ничего не найдено.");

            var results = searcherPage.Search("Санкт-Петербург");

            Handlers.GoToConsole(results);

            searcherPage.CheckResultsCount(results.Count, 15);
        }

        [TestCaseSource(nameof(SetBrowsers))]
        public void Test4_WorkWithCalendar(IWebDriver driver)
        {

            _webDriver = driver;
            var mainPage = new MainPageObject(_webDriver);


            var schedulePage = mainPage.SearchTrain("Минск-Пассажирский","Брест-Центральный",5);
            schedulePage.TrainsScheduleConsoleResult();

            var trainInformationPage = schedulePage.FirstResultTrain();
            trainInformationPage.CheckTrainTitleVisibility();
            trainInformationPage.CheckCruiseTimeTable();

            _webDriver.FindElement(By.XPath("//img[@alt = 'БелЖД']")).Click();

            //how to Check the page is load fine?
        }

        [TearDown]
        public void TearDown()
        {
            _webDriver.Quit();
        }
    }
}