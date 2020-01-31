using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;
using System.Diagnostics;

namespace SSIMS.DAL
{
    public class TenderRepository : GenericRepository<Tender>
    {

        public TenderRepository(DatabaseContext context)
    : base(context)
        {
        }
    }
}