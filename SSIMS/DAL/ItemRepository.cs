using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.DAL
{
    public class ItemRepository : GenericRepository<Item>
    {
        public ItemRepository(DatabaseContext context)
            : base(context)
        {
        }

        public int UpdateItemFullStock(int id)
        {
            Console.WriteLine("UpdateItemFullStock");
            return 1;
            //return context.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
        }

    }
}