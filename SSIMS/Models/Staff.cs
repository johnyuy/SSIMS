using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Staff:Person
    {
        public string StaffId { get; set; }
        public virtual Department Department { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public string StaffRole { get; set; }

        public Staff()
        {
        }

        public Staff(string StaffId, Department Department,UserAccount UserAccount,string StaffRole)
        {
            staffId = StaffId;
            department = Department;
            userAccount = UserAccount;
            staffRole = StaffRole;
        }
    }
}