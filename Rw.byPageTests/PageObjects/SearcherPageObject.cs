using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Rw.byPageTests.PageObjects
{
    class SearcherPageObject
    {
        private IWebDriver _webDriver;

        public SearcherPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void CheckUrl(string searchingString)
        {
            Assert.AreEqual(string.Concat("https://www.rw.by/search/?s=Y&q=", searchingString), _webDriver.Url);
        }

        public void CheckNoteText(string actualNotetext)
        {
            var warningMessage = _webDriver.FindElement(By.ClassName("notetext")).Text;

            Assert.AreEqual(actualNotetext, warningMessage);

        }

        public ReadOnlyCollection<IWebElement> Search(string searchingString)
        {
            var searher = _webDriver.FindElement(By.XPath("//input[@id = 'searchinpm']"));
            searher.Clear();
            searher.SendKeys(searchingString);

            _webDriver
                .FindElement(By.XPath("//input[@type = 'submit']"))
                .Click();

            Thread.Sleep(500);

            return _webDriver.FindElement(By.XPath("//ol[@class = 'search-result']")).FindElements(By.TagName("li"));
        }

        public void CheckResultsCount(int resultsOnPageCount, int expectedCount)
        {
            Assert.AreEqual(expectedCount, resultsOnPageCount);
        }
    }
}
