using NUnit.Framework;
using OpenQA.Selenium;

namespace Rw.byPageTests.PageObjects
{
    class TrainInformationPageObject
    {
        private IWebDriver _webDriver;

        public TrainInformationPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public void CheckTrainTitleVisibility()
        {
            var trainTitleVisible = _webDriver
                .FindElement(By.XPath("//div[@class = 'sch-title__title h2']"))
                .Displayed;

            Assert.IsTrue(trainTitleVisible);
        }
        public void CheckCruiseTimeTable()
        {
            var cruiseTimetable = _webDriver
                .FindElement(By.XPath("//div[@class = 'sch-title__descr']"))
                .Text[19..];

            Assert.IsFalse(string.IsNullOrWhiteSpace(cruiseTimetable));
        }
    }
}
