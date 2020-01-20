using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using SSIMS.Models;


namespace SSIMS.Database
{
    public class SSIMSDbContext : DbContext
    {
        public SSIMSDbContext() : base("Server=DRAKELIND20E;Database=NETLib;Integrated Security = True")
        {
            Database.SetInitializer(new SSIMSDbInitializer<SSIMSDbContext>());
        }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Requsition> Requisitions { get; set; }

        public DbSet<UserAccount> UserAccounts { get; set; }

        public DbSet<InventoryItem> InventoryItems { get; set; }

        public DbSet<TrasactionalItem> TransactionalItems { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Department> Departments { get; set; }

        public DbSet<DeptHeadAuthorization> DeptHeadAuthorizations { get; set; }

        public DbSet<CollectionPoint> CollectionPoints { get; set; }

        public DbSet<RestockItem> RestockItems { get; set; }

        public DbSet<TenderItem> TenderItems { get; set; }

        public DbSet<RequestForm> RequestForms { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        public DbSet<DeliveryOrder> DeliveryOrders { get; set; }

        public DbSet<RetrievalList> RetrievalLists { get; set; }

        public DbSet<DisbursementList> DisbursementLists { get; set; }

        public DbSet<StockCardEntry> StockCardEntries { get; set; }
    }
}