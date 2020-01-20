using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SSIMS.Database
{
    public class SSIMSDbInitializer<T> : DropCreateDatabaseAlways<SSIMSDbContext>
    {
        protected override void Seed(SSIMSDbContext context)
        {
            //seed data
            base.Seed(context);
        }
    }
}