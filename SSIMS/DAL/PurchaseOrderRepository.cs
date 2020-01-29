using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.DAL
{
    public class PurchaseOrderRepository : GenericRepository<PurchaseOrder>
    {
        public PurchaseOrderRepository(DatabaseContext context)
    : base(context)
        {
        }

        public int extraMethod(int id)
        {
            Console.WriteLine("UpdateItemFullStock");

            var categories = context.Database.ExecuteSqlCommand("SELECT DISTINCT Category FROM Items");

            return context.Database.ExecuteSqlCommand("SELECT DISTINCT Category FROM Items");

            //return 1;
            //return context.Database.ExecuteSqlCommand("SELECT Course SET Credits = Credits * {0}", multiplier);


        }
    }
}