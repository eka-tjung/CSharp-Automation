using ContosoUniversityGrades.Model;
using CUTests.Utilities;
using NUnit.Framework;
using System;
using System.Data.SqlClient;

namespace CUTests.DBTests
{
    [TestFixture]
    public class GetDataTests
    {
        DBHelper _dbHelper = DBHelper.GetInstance();

        [SetUp]
        public void BeforeEachTest()
        {
            _dbHelper.OpenDBConnection();
        }

        [TearDown]
        public void AfterEachTest()
        {
            _dbHelper.CloseDBConnection();
        }

        [Test]
        public void GetCourseData()
        {
            // Initialize
            string query = @"SELECT TOP 1 * FROM Course";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            Course retrieved = _dbHelper.GetCourseData(reader);

            // Validate
            Assert.IsTrue(reader.HasRows);
            Assert.NotNull(retrieved);
        }

        [Test]
        public void GetCourseInstructorData()
        {
            // Initialize
            string query = @"SELECT TOP 1 * FROM CourseInstructor";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            CourseInstructor retrieved = _dbHelper.GetCourseInstructorData(reader);

            // Validate
            Assert.IsTrue(reader.HasRows);
            Assert.NotNull(retrieved);
        }

        [Test]
        public void GetDepartmentData()
        {
            // Initialize
            string query = @"SELECT TOP 1 * FROM Department";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            Department retrieved = _dbHelper.GetDepartmentData(reader);

            // Validate
            Assert.IsTrue(reader.HasRows);
            Assert.NotNull(retrieved);
        }

        [Test]
        public void GetOfficeAssignmentData()
        {
            // Initialize
            string query = @"SELECT TOP 1 * FROM OfficeAssignment";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            OfficeAssignment retrieved = _dbHelper.GetOfficeAssignmentData(reader);

            // Validate
            Assert.IsTrue(reader.HasRows);
            Assert.NotNull(retrieved);
        }

        [Test]
        public void GetOnlineCourseData()
        {
            // Initialize
            string query = @"SELECT TOP 1 * FROM OnlineCourse";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            OnlineCourse retrieved = _dbHelper.GetOnlineCourseData(reader);

            // Validate
            Assert.IsTrue(reader.HasRows);
            Assert.NotNull(retrieved);
        }

        [Test]
        public void GetOnsiteCourseData()
        {
            // Initialize
            string query = @"SELECT TOP 1 * FROM OnsiteCourse";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            OnsiteCourse retrieved = _dbHelper.GetOnsiteCourseData(reader);

            // Validate
            Assert.IsTrue(reader.HasRows);
            Assert.NotNull(retrieved);
        }

        [Test]
        public void GetPersonData()
        {
            // Initialize
            string query = @"SELECT TOP 1 * FROM Person";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            Person retrieved = _dbHelper.GetPersonData(reader);

            // Validate
            Assert.IsTrue(reader.HasRows);
            Assert.NotNull(retrieved);
        }

        [Test]
        public void GetStudentGradeData()
        {
            // Initialize
            string query = @"SELECT TOP 1 * FROM StudentGrade";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);
            reader.Read();
            StudentGrade retrieved = _dbHelper.GetStudentGradeData(reader);

            // Validate
            Assert.IsTrue(reader.HasRows);
            Assert.NotNull(retrieved);
        }
    }
}
