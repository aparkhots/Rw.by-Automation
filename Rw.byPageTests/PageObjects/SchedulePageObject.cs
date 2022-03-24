using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace Rw.byPageTests.PageObjects
{
    class SchedulePageObject
    {
        private IWebDriver _webDriver;

        public SchedulePageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void TrainsScheduleConsoleResult()
        {
            var trainsCount = _webDriver
                .FindElements(By.XPath("//div[@data-train-info]"))
                .Count;

            var trainsFrom = _webDriver.FindElements(By.XPath("//div[@class = 'sch-table__station train-from-name']"));

            var trainsTo = _webDriver.FindElements(By.XPath("//div[@class = 'sch-table__station train-to-name']"));

            var trainsTime = _webDriver.FindElements(By.XPath("//div[@class = 'sch-table__time train-from-time']"));

            for (int i = 0; i < trainsCount; i++)
            {
                Console.WriteLine($"'{trainsFrom[i].Text} - {trainsTo[i].Text}' - {trainsTime[i].Text}");
            }
        }

        public TrainInformationPageObject FirstResultTrain()
        {
            _webDriver
                .FindElements(By.XPath("//span[@class = 'train-route']"))
                .First()
                .Click();
            return new TrainInformationPageObject(_webDriver);
        }
    }
}
