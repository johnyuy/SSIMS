using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations;

namespace SSIMS.Models
{
    public class Staff
    {
        public int StaffID { get; set; }
        public int? DepartmentID { get; set; }
        public int? UserAccountID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string StaffRole { get; set; }

        public virtual Department Department { get; set; }
        public virtual UserAccount UserAccount { get; set; }

        public Staff()
        {
        }

        public Staff(string name, string phoneNumber, 
            string email, int departmentID, string staffRole)     
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            DepartmentID = departmentID;
            StaffRole = staffRole;
        }

    }
}