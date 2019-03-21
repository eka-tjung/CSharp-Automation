using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversityGrades.Model
{
    public class Course
    {
        public int CourseID { get; set; }
        public String Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentID { get; set; }
    }
}
