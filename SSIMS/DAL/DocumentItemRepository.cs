using SSIMS.Database;
using SSIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.DAL
{
    public class DocumentItemRepository : GenericRepository<DocumentItem>
    {

        public DocumentItemRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}