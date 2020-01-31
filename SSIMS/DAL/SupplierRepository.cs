using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.DAL
{
    public class SupplierRepository :  GenericRepository<Supplier>
    {

        public SupplierRepository(DatabaseContext context)
    : base(context)
        {
        }
    }
}