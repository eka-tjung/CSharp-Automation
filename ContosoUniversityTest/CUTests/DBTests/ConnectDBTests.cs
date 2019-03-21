using CUTests.Utilities;
using NUnit.Framework;
using System.Data.SqlClient;

namespace CUTests.DBTests
{
    /// <summary>
    /// Basic tests connecting to the DB
    /// </summary>
    [TestFixture]
    public class ConnectDBTests
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
        public void SelectAllFromCourse()
        {
            // Initialize
            string query = @"SELECT * FROM Course";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);

            // Validate
            Assert.NotNull(reader);
        }

        [Test]
        public void SelectAllFromCourseInstructor()
        {
            // Initialize
            string query = @"SELECT * FROM CourseInstructor";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);

            // Validate
            Assert.NotNull(reader);
        }

        [Test]
        public void SelectAllFromDepartment()
        {
            // Initialize
            string query = @"SELECT * FROM Department";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);

            // Validate
            Assert.NotNull(reader);
        }

        [Test]
        public void SelectAllFromOfficeAssignment()
        {
            // Initialize
            string query = @"SELECT * FROM OfficeAssignment";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);

            // Validate
            Assert.NotNull(reader);
        }

        [Test]
        public void SelectAllFromOnlineCourse()
        {
            // Initialize
            string query = @"SELECT * FROM OnlineCourse";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);

            // Validate
            Assert.NotNull(reader);
        }

        [Test]
        public void SelectAllFromOnsiteCourse()
        {
            // Initialize
            string query = @"SELECT * FROM OnsiteCourse";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);

            // Validate
            Assert.NotNull(reader);
        }

        [Test]
        public void SelectAllFromPerson()
        {
            // Initialize
            string query = @"SELECT * FROM Person";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);

            // Validate
            Assert.NotNull(reader);
        }

        [Test]
        public void SelectAllFromStudentGrade()
        {
            // Initialize
            string query = @"SELECT * FROM StudentGrade";

            // Execute
            SqlDataReader reader = _dbHelper.QueryDB(query);

            // Validate
            Assert.NotNull(reader);
        }
    }
}
