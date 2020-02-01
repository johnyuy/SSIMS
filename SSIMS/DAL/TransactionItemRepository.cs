using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.DAL
{
    public class TransactionItemRepository : GenericRepository<TransactionItem>
    {
        public TransactionItemRepository(DatabaseContext context)
   : base(context)
        {
        }
    }
}