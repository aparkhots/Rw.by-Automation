using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using OpenQA.Selenium;

namespace Rw.byPageTests.PageObjects
{
    class GoogleSearcherPageObject
    {
        private readonly IWebDriver _webDriver;

        private readonly By googleSearchInput = By.XPath("//input[@name = 'q']");
        private readonly By googleSearchButton = By.XPath("//input[@name= 'btnK']");
       
        
        public GoogleSearcherPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _webDriver.Navigate().GoToUrl("https://www.google.com");

        }

        public void Searching(string searchingString)
        {
            
            _webDriver.FindElement(googleSearchInput).SendKeys(searchingString);

            Thread.Sleep(300);

            _webDriver.FindElement(googleSearchButton).Click();

            _webDriver.FindElement(By.PartialLinkText("www.rw.by")).Click();

        }
    }
}
