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

        
        


    }
}