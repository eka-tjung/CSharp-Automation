using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContosoUniversityGrades.Model
{
    public class OfficeAssignment
    {
        public int InstructorID { get; set; }
        public String Location { get; set; }
        public byte[] Timestamp { get; set; }
    }
}
