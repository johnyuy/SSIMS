using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations;

namespace SSIMS.Models
{
    public class Staff : DbMigration
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

        public override void Up()
        {
            CreateTable(
               "dbo.Staffs",
               c => new
               {
                   StaffID = c.Int(nullable: false, identity: true),
                   DepartmentID = c.Int(nullable: true, identity: true),
                   UserAccountID = c.Int(nullable: true, identity: true),
                   StaffRole = c.String(nullable: false),

               })
               .PrimaryKey(t => t.StaffID);
            Sql("DBCC CHECKIDENT ('Staffs', RESEED, 10000);");
        }
    }
}