using ContosoUniversityGrades.Model;
using CUTests.Utilities;
using NUnit.Framework;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data.SqlClient;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace CUTests.E2ETests
{
    [TestFixture]
    public class UpdateGradeTests
    {
        DBHelper _dbHelper = DBHelper.GetInstance();
        GradesManagerHelper _gradesHelper = new GradesManagerHelper();
        private Application _testApp;
        private Window _testWindow;
        ChromeDriver _driver;
        WebDriverWait _wait;
        Actions _actionBuilder;

        [OneTimeSetUp]
        public void BeforeAllTests()
        {
            _gradesHelper.LaunchGradesManager();
            _testApp = _gradesHelper.GetTestApp();
            _testWindow = _gradesHelper.GetTestWindow();
        }

        [OneTimeTearDown]
        public void AfterAllTests()
        {
            _testWindow.Close();
            _testApp.Close();
        }

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
        public void VerifyUpdateRandomStudentGradeWithWebsite()
        {
            // Initialize
            Random rnd = new Random();
            int selectedIndex = 0;

            /// Execution
            // Get random teacher
            ComboBox teacherList = _testWindow.Get<ComboBox>("teacherList");
            int teacherCount = teacherList.Items.Count;
            selectedIndex = rnd.Next(teacherCount);
            teacherList.Select(selectedIndex);

            // Get random course
            ComboBox courseList = _testWindow.Get<ComboBox>("courseList");
            int courseCount = courseList.Items.Count;
            selectedIndex = rnd.Next(courseCount);
            courseList.Select(selectedIndex);

            // Get random student
            ComboBox studentList = _testWindow.Get<ComboBox>("studentList");
            int studentCount = studentList.Items.Count;
            selectedIndex = rnd.Next(studentCount);
            studentList.Select(selectedIndex);

            // Modify the student's grade
            // Working with decimal variable types, see http://msdn.microsoft.com/en-us/library/364x0z75.aspx
            TextBox studentGrade = _testWindow.Get<TextBox>("studentGrade");
            decimal currentGrade = Convert.ToDecimal(studentGrade.Text);
            decimal newGrade = currentGrade - 0.1m;
            studentGrade.Text = newGrade.ToString();

            // Update the grade
            Button updateButton = _testWindow.Get<Button>("btnUpdateGrade");
            updateButton.Click();

            /// Validation
            // Get teacher name
            String selectedTeacher = teacherList.SelectedItemText;
            String[] selectedTeacherName = selectedTeacher.Split(',');

            // Get student name
            String selectedStudent = studentList.SelectedItemText;
            String[] selectedStudentName = selectedStudent.Split(',');

            // Get course title
            String selectedCourse = courseList.SelectedItemText;

            // Launch the instructors page
            _driver.Navigate().GoToUrl("http://localhost:1701/Instructors.aspx");

            // Find instructor
            String xPathQuery = String.Format("//span[text()='{0}']", selectedTeacherName[0].Trim());
            var teacherSpan = _driver.FindElementByXPath(xPathQuery);

            // Select instructor
            String teacherSpanId = teacherSpan.GetAttribute("id");
            String[] teacherSpanIdComponents = teacherSpanId.Split('_');
            String teacherIndex = teacherSpanIdComponents[teacherSpanIdComponents.Length - 1];
            xPathQuery = String.Format("//a[@href='javascript:__doPostBack('ctl00$MainContent$InstructorsGridView','Select${0}')']", teacherIndex);
            _driver.FindElementByXPath(xPathQuery).Click();

            // Select course
            xPathQuery = "//a[@href='javascript:__doPostBack('ctl00$MainContent$CoursesGridView','Select$0')'";
            _driver.FindElementByXPath(xPathQuery).Click();

            // Find student
            xPathQuery = String.Format("//span[@InnerHtml='{0}']", selectedStudentName[0].Trim());
            var studentSpan = _driver.FindElementByXPath(xPathQuery);

            // Get student grade
            String studentSpanId = studentSpan.GetAttribute("id");
            String[] studentSpanIdComponents = studentSpanId.Split('_');
            String studentIndex = studentSpanIdComponents[studentSpanIdComponents.Length];
            String observedGrade = _driver.FindElementById(String.Format("MainContent_GradesListView_StudentGradeLabel_{0}", studentIndex)).Text;

            // Compare the grade we set with the grade on website
            Assert.AreEqual(newGrade, Convert.ToDecimal(observedGrade));
        }
    }
}
