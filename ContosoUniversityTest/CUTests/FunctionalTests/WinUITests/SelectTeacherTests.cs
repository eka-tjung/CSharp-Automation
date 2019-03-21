using CUTests.Utilities;
using NUnit.Framework;
using System;
using TestStack.White;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace CUTests.FunctionalTests.WinUITests
{
    [TestFixture]
    public class SelectTeacherTests
    {
        GradesManagerHelper _gradesHelper = new GradesManagerHelper();

        private Application _testApp;
        private Window _testWindow;

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
        }

        [TearDown]
        public void AfterEachTest()
        {
        }

        [Test]
        public void SelectRandomTeacher()
        {
            // Initialize
            ComboBox teacherList = _testWindow.Get<ComboBox>("teacherList");
            int teacherCount = teacherList.Items.Count;
            Random rnd = new Random();
            int selectedIndex = rnd.Next(teacherCount);

            // Execute
            teacherList.Select(selectedIndex);

            // Validate
            ComboBox courseList = _testWindow.Get<ComboBox>("courseList");
            Assert.IsTrue(courseList.Items.Count > 0);
        }
    }
}
