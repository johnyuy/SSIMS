using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using SSIMS.Models;


namespace SSIMS.Database
{
    public static class DatabaseCustomizer
    {
        public static void SetDefaultID(DatabaseContext context)
        {

            context.Database.Connection.Open();
            try
            {
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Staffs ON");
                context.Database.ExecuteSqlCommand("INSERT INTO STAFFS (StaffID, Name, DepartmentID) VALUES (100001,'John Yu',101)");
                context.SaveChanges();
                context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Staffs OFF");
            }
            finally
            {
                context.Database.Connection.Close();
            }


            foreach (var staff in context.Staffs)
            {
                Debug.WriteLine(staff.StaffID + ": " + staff.Name);
            }
            
        }
    }
}