using System;
using SSIMS.Models;
using System.Data.Entity;

namespace SSIMS.Database
{
    public class DatabaseContext : DbContext
    {

        public static string connectionString = Data.connectionString;
        public DatabaseContext() : base(connectionString)
        {
            
            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer<DatabaseContext>());
  
        }
 
    }
}