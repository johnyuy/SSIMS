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
        public DbSet<DeptHeadAuthorization> DeptHeadAuthorizations { get; set; }
        public DbSet<DisbursementList> DisbursementLists { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentItem> DocumentItems { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<PurchaseItem> PurchaseItems { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<RequisitionForm> RequisitionForms { get; set; }
        public DbSet<RetrievalList> RetrievalLists { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<StockCardEntry> StockCardEntries { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Tender> Tenders { get; set; }
        public DbSet<TransactionItem> TransactionItems { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

    }
}