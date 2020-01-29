using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using SSIMS.Models;
using SSIMS.Database;
using SSIMS.DAL;

namespace SSIMS.Service
{
    public class PurchaseOrderService : IDisposable
    {
        private DatabaseContext context = new DatabaseContext();
        private PurchaseOrderRepository purchaseOrderRepository;


        public PurchaseOrderRepository ItemRepository
        {
            get
            {

                if (this.purchaseOrderRepository == null)
                {
                    this.purchaseOrderRepository = new PurchaseOrderRepository(context);
                }
                return purchaseOrderRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}