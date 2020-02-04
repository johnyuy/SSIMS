using SSIMS.Database;
using SSIMS.Models;
using SSIMS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.DAL
{
    public class RequisitionOrderRepository:GenericRepository<RequisitionOrder>
    {

        public RequisitionOrderRepository(DatabaseContext context)
            : base(context)
        {
        }

        //delete requisitionOrder by id
        public void DeleteRObyID(int ID)
        {
            var requisitionOrder = Get(filter: x => x.ID == ID).First();
            //context.RequisitionOrders.Remove(requisitionOrder);
            dbSet.Remove(requisitionOrder);
        }
      



    }
}