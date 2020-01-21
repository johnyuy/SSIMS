using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Staff: Person
    {
        public int StaffID { get; set; }
        public int DepartmentID { get; set; }
        public int UserAccountID { get; set; }
        public string StaffRole { get; set; }

        public virtual Department Department { get; set; }
        public virtual UserAccount UserAccount { get; set; }

        public Staff()
        {
        }

        public Staff(string name, string phoneNumber, string faxNumber, 
            string email, int departmentID, int userAccountID, string staffRole)     
                    : base(name, phoneNumber, faxNumber, email)
        {
            DepartmentID = departmentID;
            UserAccountID = userAccountID;
            StaffRole = staffRole;
        }
    }
}