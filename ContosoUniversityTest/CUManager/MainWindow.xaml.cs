using CUManager.Model;
using CUTests.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CUManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DBController _db = DBController.GetInstance();
        List<Person> _teachers = new List<Person>();
        List<Person> _students = new List<Person>();
        List<Course> _courses = new List<Course>();
        List<CourseInstructor> _courseInstructorList = new List<CourseInstructor>();
        List<StudentGrade> _studentGradeList = new List<StudentGrade>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LogMessage(_db._currentPath);
            LogMessage(_db._connectionString);

            // Retrieve all relevant data from DB
            //GetDBData();

            // When page is loaded, populate the teacher combobox
            teacherList.Items.Clear();
            foreach (Person teacher in _teachers)
            {
                teacherList.Items.Add(FormatName(teacher));
            }
        }

        /// <summary>
        /// Called whenever user changes the selected teacher
        /// </summary>
        private void teacherList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = teacherList.SelectedIndex;
            LogMessage("Teacher selected: " + teacherList.SelectedValue.ToString());

            courseList.Items.Clear();
            int selectedTeacherId = _teachers[selectedIndex].PersonID;
            foreach (CourseInstructor courseInstructor in _courseInstructorList)
            {
                if (courseInstructor.PersonID == selectedTeacherId)
                {
                    Course relevantCourse = _courses.Find(c => c.CourseID == courseInstructor.CourseID);
                    courseList.Items.Add(relevantCourse.Title);
                }
            }
        }

        /// <summary>
        /// Called whenever user changes the selected course
        /// </summary>
        private void courseList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LogMessage("Course selected: " + courseList.SelectedValue.ToString());

            studentList.Items.Clear();
            if (courseList.Items.Count > 0)
            {
                Course selectedCourse = _courses.Find(c => c.Title == courseList.SelectedValue.ToString());
                foreach (StudentGrade studentGrade in _studentGradeList)
                {
                    if (studentGrade.CourseID == selectedCourse.CourseID)
                    {
                        Person registeredStudent = _students.Find(s => s.PersonID == studentGrade.StudentID);
                        if (registeredStudent != null)
                        {
                            studentList.Items.Add(FormatName(registeredStudent));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called whenever user changes the selected student
        /// </summary>
        private void studentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            studentGrade.Text = String.Empty;
            int selectedIndex = studentList.SelectedIndex;
            LogMessage("Student selected: " + studentList.SelectedValue.ToString());

            if (studentList.Items.Count > 0)
            {
                Person selectedStudent = _students.Find(s => FormatName(s) == studentList.SelectedValue.ToString());
                studentGrade.Text = _studentGradeList.Find(g => g.StudentID == selectedStudent.PersonID).Grade.ToString();
            }
        }

        /// <summary>
        /// Called whenever the update grade button is clicked
        /// </summary>
        private void btnUpdateGrade_Click(object sender, RoutedEventArgs e)
        {
            Decimal newGrade = Convert.ToDecimal(studentGrade.Text);
            Person selectedStudent = _students.Find(s => FormatName(s) == studentList.SelectedValue.ToString());
            Course selectedCourse = _courses.Find(c => c.Title == courseList.SelectedValue.ToString());
            LogMessage(String.Format("Updating {0}'s grade to {1} for {2}", selectedStudent.FirstName, newGrade, selectedCourse.Title));
            UpdateGrade(newGrade, selectedStudent.PersonID, selectedCourse.CourseID);
        }

        /// <summary>
        /// Attempts to update the grade in the DB
        /// 
        /// NOTE: This is actually very UNSAFE and likely to cause bugs!
        /// </summary>
        private void UpdateGrade(Decimal newGrade, int studentId, int courseId)
        {
            // Open DB connection
            OpenDBConnection();

            // Update it in the DB
            string query = String.Format("UPDATE StudentGrade SET Grade={0} WHERE StudentID={1} AND CourseID={2}", newGrade, studentId, courseId);
            _db.QueryDB(query);

            // Update it locally
            StudentGrade localGrade = _studentGradeList.Find(g => (g.StudentID == studentId && g.CourseID == courseId));
            localGrade.Grade = newGrade;

            // Close the DB
            CloseDBConnection();
        }

        /// <summary>
        /// A consistent way to format names
        /// </summary>
        private String FormatName(Person person)
        {
            return String.Format("{0}, {1}", person.LastName, person.FirstName);
        }

        /// <summary>
        /// Gets all the relevant DB data
        /// </summary>
        private void GetDBData()
        {
            // Open DB connection
            OpenDBConnection();

            // Get teachers
            string query = @"SELECT * FROM Person WHERE HireDate IS NOT NULL";
            SqlDataReader reader = _db.QueryDB(query);
            while (reader.Read())
            {
                _teachers.Add(_db.GetPersonData(reader));
            }

            // Get students
            query = @"SELECT * FROM Person WHERE HireDate IS NULL";
            reader = _db.QueryDB(query);
            while (reader.Read())
            {
                Person student = _db.GetPersonData(reader);
                if (student != null)
                {
                    _students.Add(student);
                }
            }

            // Get courses
            query = @"SELECT * FROM Course";
            reader = _db.QueryDB(query);
            while (reader.Read())
            {
                _courses.Add(_db.GetCourseData(reader));
            }

            // Get course instructor list
            query = @"SELECT * FROM CourseInstructor";
            reader = _db.QueryDB(query);
            while (reader.Read())
            {
                _courseInstructorList.Add(_db.GetCourseInstructorData(reader));
            }

            // Get student grade list
            query = @"SELECT * FROM StudentGrade";
            reader = _db.QueryDB(query);
            while (reader.Read())
            {
                StudentGrade grade = _db.GetStudentGradeData(reader);
                if (grade != null)
                {
                    _studentGradeList.Add(grade);
                }
            }

            // Close DB connection
            CloseDBConnection();

            // Sort everything
            _teachers.Sort((a, b) => a.LastName.CompareTo(b.LastName));
            _students.Sort((a, b) => a.LastName.CompareTo(b.LastName));
            _courses.Sort((a, b) => a.Title.CompareTo(b.Title));
        }

        /// <summary>
        /// Attempt to open the DB connection
        /// </summary>
        private void OpenDBConnection()
        {
            try
            {
                _db.OpenDBConnection();
                LogMessage("DB opened successfully!");
            }
            catch (Exception ex)
            {
                LogMessage("DB failed to open: " + ex.Message);
            }
        }

        /// <summary>
        /// Attempt to close the DB connection
        /// </summary>
        private void CloseDBConnection()
        {
            try
            {
                _db.CloseDBConnection();
                LogMessage("DB closed successfully!");
            }
            catch (Exception ex)
            {
                LogMessage("DB failed to close: " + ex.Message);
            }
        }

        /// <summary>
        /// Logs the provided message into the UI
        /// </summary>
        private void LogMessage(String msg)
        {
            logBox.Content += String.Format("\n{0}", msg);
        }
    }
}
