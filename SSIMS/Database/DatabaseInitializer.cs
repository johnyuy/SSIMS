using System;
using System.Collections.Generic;

using System.Data.Entity;
using SSIMS.Models;

namespace SSIMS.Database
{
    public class DatabaseInitializer<T> : DropCreateDatabaseAlways<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {

            //Seed data
            
            context.SaveChanges();
            base.Seed(context);
        }
    }
}