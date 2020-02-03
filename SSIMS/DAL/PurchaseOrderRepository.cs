﻿using System;
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

    }
}