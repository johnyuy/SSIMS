using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.DAL
{
    public class AdjustmentVoucherRepository : GenericRepository<AdjustmentVoucher>
    {
        public AdjustmentVoucherRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}