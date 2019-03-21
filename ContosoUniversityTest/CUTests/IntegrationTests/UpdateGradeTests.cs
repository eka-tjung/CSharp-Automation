using ContosoUniversityGrades.Model;
using CUTests.Utilities;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data.SqlClient;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace CUTests.IntegrationTests
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
        public void VerifyUpdateRandomStudentGradeWithDB()
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
            // Get student information
            String selectedStudent = studentList.SelectedItemText;
            String[] selectedStudentName = selectedStudent.Split(',');
            string query = String.Format("SELECT * FROM Person WHERE FirstName='{0}' AND LastName='{1}'", selectedStudentName[1].Trim(), selectedStudentName[0].Trim());
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            Person student = _dbHelper.GetPersonData(reader);

            // Get course information
            String selectedCourse = courseList.SelectedItemText;
            query = String.Format("SELECT * FROM Course WHERE Title='{0}'", selectedCourse);
            reader = _dbHelper.QueryDB(query);
            reader.Read();
            Course course = _dbHelper.GetCourseData(reader);

            // Get student grade information
            query = String.Format("SELECT * FROM StudentGrade WHERE StudentID='{0}' AND CourseID='{1}'", student.PersonID, course.CourseID);
            reader = _dbHelper.QueryDB(query);
            reader.Read();
            StudentGrade grade = _dbHelper.GetStudentGradeData(reader);

            // Compare the grade we set with the grade from DB
            Assert.AreEqual(newGrade, grade.Grade);
        }
    }
}
