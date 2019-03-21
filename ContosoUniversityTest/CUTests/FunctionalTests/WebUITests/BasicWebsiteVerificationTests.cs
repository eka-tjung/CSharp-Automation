using System;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;

namespace CUTests.FunctionalTests.WebUITests
{
    [TestFixture]
    class BasicWebsiteVerificationTests
    {
        ChromeDriver _driver;

        [SetUp]
        public void BeforeEachTest()
        {
            _driver = new ChromeDriver();
        }

        [TearDown]
        public void AfterEachTest()
        {
            //_driver.Close();
        }

        [Test]
        public void LaunchDefaultPage()
        {
            // Initialize
            String defaultPageUrl = "http://localhost:1701/Default.aspx";

            // Execute
            _driver.Navigate().GoToUrl(defaultPageUrl);

            // Validate
            Assert.AreEqual(defaultPageUrl, _driver.Url);
        }

        [Test]
        public void SelectNameSort()
        {
            // Initialize
            String defaultPageUrl = "http://localhost:1701/Students.aspx";
            _driver.Navigate().GoToUrl(defaultPageUrl);

            // Execute
            // //div[@class='main']/div[1]/table[1]/tbody[1]/tr[1]/th[2]
            String searchString = "//div[@class='main']/div[1]/table[1]/tbody[1]/tr[1]/th[2]";
            _driver.FindElementByXPath(searchString).Click();
        }
    }
}
