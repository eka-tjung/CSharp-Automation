using ContosoUniversityGrades.Model;
using NUnit.Framework;
using System;

namespace CUTests.UnitTests
{
    [TestFixture]
    public class CreateModelTests
    {
        [Test]
        public void CreateCourseModel()
        {
            // Initialize
            int courseId = 1;
            int credits = 3;
            int departmentID = 0;
            String title = "New Course";

            // Execute
            Course newCourse = new Course()
            {
                CourseID = courseId,
                Credits = credits,
                DepartmentID = departmentID,
                Title = title
            };

            // Validate
            Assert.NotNull(newCourse);
            Assert.AreEqual(courseId, newCourse.CourseID);
            Assert.AreEqual(credits, newCourse.Credits);
            Assert.AreEqual(departmentID, newCourse.DepartmentID);
            Assert.AreEqual(title, newCourse.Title);
        }

        // TODO: Need to create unit tests for all object models
    }
}
