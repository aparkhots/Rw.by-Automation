using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
