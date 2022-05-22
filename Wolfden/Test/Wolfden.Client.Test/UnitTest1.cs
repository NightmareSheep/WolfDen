using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Threading.Tasks;

namespace Wolfden.Client.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://localhost:7137/");
            Thread.Sleep(10000);
            driver.Quit();
        }
    }
}