using System;
using SSIMS.Models;
using System.Data.Entity;

namespace SSIMS.Database
{
    public class DatabaseContext : DbContext
    {

        public static string connectionString = ServerConnection.ConnectionString;
        public DatabaseContext() : base(connectionString)
        {

            System.Data.Entity.Database.SetInitializer(new DatabaseInitializer<DatabaseContext>());

        }


        public DbSet<AdjustmentVoucher> AdjustmentVouchers { get; set; }
        public DbSet<CollectionPoint> CollectionPoints { get; set; }
        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }
        public DbSet<Department> Departments { get; set; }
    

    }
}