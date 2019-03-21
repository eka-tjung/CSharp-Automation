using CUManager.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUTests.Utilities
{
    public class DBController
    {
        private static DBController _instance = null;
        private static SqlConnection _sqlConnection = null;
        public readonly String _connectionString;
        public readonly String _currentPath;

        /// <summary>
        /// Private constructor with a GetInstance makes this a singleton
        /// </summary>
        private DBController()
        {
            // Open connection to DB
            String dataSource = @"(LocalDb)\v11.0";
            _currentPath = Environment.CurrentDirectory;
            String pathToDbFolder = _currentPath.Remove(_currentPath.Length - (@"CUManager\Controller").Length) + @"\ContosoUniversity\App_Data\";
            String attachDbFilename = String.Format("{0}School.mdf", pathToDbFolder.ToString());
            _connectionString = String.Format("Data Source={0};AttachDbFilename={1};Integrated Security={2};MultipleActiveResultSets={3}", dataSource, attachDbFilename, "True", "True");
            _sqlConnection = new SqlConnection(_connectionString);
        }

        /// <summary>
        /// Gets an instance of the DB Helper class
        /// </summary>
        public static DBController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DBController();
            }

            return _instance;
        }

        public void OpenDBConnection()
        {
            _sqlConnection.Open();
        }

        public void CloseDBConnection()
        {
            _sqlConnection.Close();
        }

        /// <summary>
        /// Query the database given the provided query string.
        /// 
        /// Requires DB connection to be opened first - will return null otherwise
        /// </summary>
        /// <param name="queryString">The query string to be executed on the database</param>
        /// <returns>The SqlDataReader returned from executing the query on the database</returns>
        public SqlDataReader QueryDB(String queryString)
        {
            // Execute query
            SqlCommand command = new SqlCommand(queryString, _sqlConnection);
            SqlDataReader reader = null;

            try
            {
                reader = command.ExecuteReader();
            }
            catch
            {
                reader = null;
            }

            return reader;
        }

        /// <summary>
        /// Converts row data retrieved from the DB into Course
        /// </summary>
        /// <param name="row">DB row data</param>
        /// <returns>Course data; null if conversion not possible</returns>
        public Course GetCourseData(SqlDataReader row)
        {
            Course retrieved = null;

            try
            {
                retrieved = new Course
                {
                    CourseID = row.GetInt32(0),
                    Title = row.GetString(1),
                    Credits = row.GetInt32(2),
                    DepartmentID = row.GetInt32(3)
                };
            }
            catch
            {
                retrieved = null;
            }

            return retrieved;
        }

        /// <summary>
        /// Converts row data retrieved from the DB into CourseInstructor
        /// </summary>
        /// <param name="row">DB row data</param>
        /// <returns>CourseInstructor data; null if conversion not possible</returns>
        public CourseInstructor GetCourseInstructorData(SqlDataReader row)
        {
            CourseInstructor retrieved = null;

            try
            {
                retrieved = new CourseInstructor
                {
                    CourseID = row.GetInt32(0),
                    PersonID = row.GetInt32(1)
                };
            }
            catch
            {
                retrieved = null;
            }

            return retrieved;
        }

        /// <summary>
        /// Converts row data retrieved from the DB into Department
        /// </summary>
        /// <param name="row">DB row data</param>
        /// <returns>Department data; null if conversion not possible</returns>
        public Department GetDepartmentData(SqlDataReader row)
        {
            Department retrieved = null;

            try
            {
                retrieved = new Department
                {
                    DepartmentID = row.GetInt32(0),
                    Name = row.GetString(1),
                    Budget = row.GetDecimal(2),
                    StartDate = row.GetDateTime(3).ToString(),
                    Administrator = row.GetInt32(4)
                };
            }
            catch
            {
                retrieved = null;
            }

            return retrieved;
        }

        /// <summary>
        /// Converts row data retrieved from the DB into OfficeAssignment
        /// </summary>
        /// <param name="row">DB row data</param>
        /// <returns>OfficeAssignment data; null if conversion not possible</returns>
        public OfficeAssignment GetOfficeAssignmentData(SqlDataReader row)
        {
            OfficeAssignment retrieved = null;

            try
            {
                retrieved = new OfficeAssignment
                {
                    InstructorID = row.GetInt32(0),
                    Location = row.GetString(1),
                    Timestamp = (byte[])row["Timestamp"]
                };
            }
            catch
            {
                retrieved = null;
            }

            return retrieved;
        }

        /// <summary>
        /// Converts row data retrieved from the DB into OnlineCourse
        /// </summary>
        /// <param name="row">DB row data</param>
        /// <returns>OnlineCourse data; null if conversion not possible</returns>
        public OnlineCourse GetOnlineCourseData(SqlDataReader row)
        {
            OnlineCourse retrieved = null;

            try
            {
                retrieved = new OnlineCourse
                {
                    CourseID = row.GetInt32(0),
                    URL = row.GetString(1)
                };
            }
            catch
            {
                retrieved = null;
            }

            return retrieved;
        }

        /// <summary>
        /// Converts row data retrieved from the DB into OnsiteCourse
        /// </summary>
        /// <param name="row">DB row data</param>
        /// <returns>OnsiteCourse data; null if conversion not possible</returns>
        public OnsiteCourse GetOnsiteCourseData(SqlDataReader row)
        {
            OnsiteCourse retrieved = null;

            try
            {
                retrieved = new OnsiteCourse
                {
                    CourseID = row.GetInt32(0),
                    Location = row.GetString(1),
                    Days = row.GetString(2),
                    Time = row.GetDateTime(3).ToString()
                };
            }
            catch
            {
                retrieved = null;
            }

            return retrieved;
        }

        /// <summary>
        /// Converts row data retrieved from the DB into Person
        /// </summary>
        /// <param name="row">DB row data</param>
        /// <returns>Person data; null if conversion not possible</returns>
        public Person GetPersonData(SqlDataReader row)
        {
            Person retrieved = null;

            try
            {
                retrieved = new Person
                {
                    PersonID = row.GetInt32(0),
                    LastName = row.GetString(1),
                    FirstName = row.GetString(2)
                };
            }
            catch
            {
                retrieved = null;
            }

            // HireDate and EnrollmentDate are not always available it seems - handle it.
            if (retrieved != null)
            {
                try
                {
                    retrieved.HireDate = row.GetDateTime(3);
                }
                catch
                {
                    // For some reason hired date is not available - simply do nothing
                }

                try
                {
                    retrieved.EnrollmentDate = row.GetDateTime(4);
                }
                catch
                {
                    // For some reason enrollment date is not available - simply do nothing
                }

            }

            return retrieved;
        }

        /// <summary>
        /// Converts row data retrieved from the DB into StudentGrade
        /// </summary>
        /// <param name="row">DB row data</param>
        /// <returns>StudentGrade data; null if conversion not possible</returns>
        public StudentGrade GetStudentGradeData(SqlDataReader row)
        {
            StudentGrade retrieved = null;

            try
            {
                retrieved = new StudentGrade
                {
                    EnrollmentID = row.GetInt32(0),
                    CourseID = row.GetInt32(1),
                    StudentID = row.GetInt32(2),
                    Grade = row.GetDecimal(3)
                };
            }
            catch
            {
                retrieved = null;
            }

            return retrieved;
        }
    }
}
