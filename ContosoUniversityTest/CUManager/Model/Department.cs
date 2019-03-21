using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUManager.Model
{
    public class Department
    {
        public int DepartmentID { get; set; }
        public String Name { get; set; }
        public decimal Budget { get; set; }
        public String StartDate { get; set; }
        public int Administrator { get; set; }
    }
}
