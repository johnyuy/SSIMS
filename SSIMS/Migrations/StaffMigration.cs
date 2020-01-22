using System;
using System.Data.Entity.Migrations;

namespace SSIMS.Migrations
{
    public partial class Staff : DbMigration
    {
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

        public override void Down()
        {
            DropTable("dbo.Staffs");
        }
    }
}