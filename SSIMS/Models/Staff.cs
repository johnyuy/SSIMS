using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations;
using System.ComponentModel.DataAnnotations;

namespace SSIMS.Models
{
    public class Staff
    {
        [Display(Name = "Staff ID")]
        public int ID { get; set; }
        //public string DepartmentID { get; set; }
        public string UserAccountID { get; set; }
        [Display(Name = "Staff Name")]
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public string StaffRole { get; set; }

        public Department Department { get; set; }

        public Staff()
        {
        }

        public Staff(string name, string phoneNumber, 
            string email, string staffRole)     
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            UserAccountID = Email.Split('@')[0];
            StaffRole = staffRole;
        }

        public Staff(string name, string phoneNumber,
            string email, Department department, string staffRole)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            UserAccountID = Email.Split('@')[0];
            Department = department;
            StaffRole = staffRole;
        }

    }
}