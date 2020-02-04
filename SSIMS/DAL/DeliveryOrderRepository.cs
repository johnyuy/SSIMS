using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.DAL
{
    public class DeliveryOrderRepository : GenericRepository<DeliveryOrder>
    {
        public DeliveryOrderRepository(DatabaseContext context) : base(context)
        {
        }

        // you can add methods specific to the class here 

        public int AdditionalMethod(int id)
        {
            Console.WriteLine("AdditionalMethod");
            return 1;
            //return context.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
        }

    }
}