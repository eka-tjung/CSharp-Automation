using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using CUTests.Utilities;
using System.Data.SqlClient;
using ContosoUniversityGrades.Model;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;

namespace CUTests.IntegrationTests
{
    [TestFixture]
    public class AddStudentsTests
    {
        DBHelper _dbHelper = DBHelper.GetInstance();
        ChromeDriver _driver;
        WebDriverWait _wait;
        Actions _actionBuilder;

        [SetUp]
        public void BeforeEachTest()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
            _actionBuilder = new Actions(_driver);

            _dbHelper.OpenDBConnection();
        }

        [TearDown]
        public void AfterEachTest()
        {
            _dbHelper.CloseDBConnection();
            _driver.Close();
        }

        [Test]
        public void VerifyAddNewStudentNavigateWebFromDefault()
        {
            // Initialize
            DateTime today = DateTime.Now;
            String firstName = "Student";
            String lastName = today.Ticks.ToString();
            String enrollmentDate = today.ToShortDateString();

            /// Execute
            // Click through to add student page
            ClickThroughToAddStudentPage();

            // Submit student data in add student page
            SubmitStudentData(firstName, lastName, enrollmentDate);
            _driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0, 0, 1);

            /// Validate
            Person retrieved = GetStudentFromDb(firstName, lastName, enrollmentDate);
            Assert.NotNull(retrieved);
            Assert.AreEqual(firstName, retrieved.FirstName);
            Assert.AreEqual(lastName, retrieved.LastName);
            Assert.AreEqual(enrollmentDate, retrieved.EnrollmentDate.ToShortDateString());
        }

        /// <summary>
        /// Attempts to get a "Person" object from the DB corresponding to the given parameters
        /// </summary>
        /// <returns>A person object conforming to the input parameters; null if not found</returns>
        private Person GetStudentFromDb(String firstName, String lastName, String enrollmentDate)
        {
            string query = String.Format("SELECT * FROM Person WHERE FirstName='{0}' AND LastName='{1}' AND EnrollmentDate='{2}'", firstName, lastName, enrollmentDate);
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            return _dbHelper.GetPersonData(reader);
        }

        /// <summary>
        /// This method actually navigates to the Add Student page
        /// starting by launching the default page and clicking through,
        /// simulating a user on the webpage
        /// </summary>
        private void ClickThroughToAddStudentPage()
        {
            String defaultPageUrl = "http://localhost:1701/Default.aspx";

            // Launch default page
            _driver.Navigate().GoToUrl(defaultPageUrl);
            _wait.Until(d => d.FindElement(By.LinkText("Students")));

            // Hover over the "Students" button
            // See https://code.google.com/p/selenium/wiki/AdvancedUserInteractions
            var StudentButton = _driver.FindElementByLinkText("Students");
            _actionBuilder.MoveToElement(StudentButton).Build().Perform();

            // Click on the "Add Students" button
            _driver.FindElementByLinkText("Add Students").Click();
            _wait.Until(d => d.FindElement(By.Name("ctl00$MainContent$StudentsDetailsView$ctl01")));
        }

        /// <summary>
        /// This method assumes user is in the add student page
        /// Subsequently, it will both enter and submit the student information as provided
        /// </summary>
        private void SubmitStudentData(String firstName, String lastName, String enrollmentDate)
        {
            // First Name input
            _driver.FindElementByName("ctl00$MainContent$StudentsDetailsView$ctl01").SendKeys(firstName);

            // Last Name input
            _driver.FindElementByName("ctl00$MainContent$StudentsDetailsView$ctl02").SendKeys(lastName);

            // Enrollment Date input
            _driver.FindElementByName("ctl00$MainContent$StudentsDetailsView$ctl03").SendKeys(enrollmentDate);

            // Click "insert" link
            _driver.FindElementByLinkText("Insert").Click();
        }
    }
}
