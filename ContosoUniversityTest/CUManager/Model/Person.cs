using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUManager.Model
{
    public class Person
    {
        public int PersonID { get; set; }
        public String LastName { get; set; }
        public String FirstName { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
