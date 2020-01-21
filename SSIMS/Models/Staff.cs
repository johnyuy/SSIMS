using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Staff: Person
    {
        public string StaffID { get; set; }
        public virtual Department Department { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public string StaffRole { get; set; }

        public Staff()
        {
        }

        public Staff(string StaffID, Department Department, UserAccount UserAccount, string StaffRole)
        {
            staffID = StaffID;
            department = Department;
            userAccount = UserAccount;
            staffRole = StaffRole;
        }
    }
}