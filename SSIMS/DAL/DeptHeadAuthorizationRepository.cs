using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Database;
using SSIMS.Models;

namespace SSIMS.DAL
{
    public class DeptHeadAuthorizationRepository : GenericRepository<DeptHeadAuthorization>
    {

        public DeptHeadAuthorizationRepository(DatabaseContext context)
            : base(context)
        {
        }

        // you can add methods specific to the class here 

        public int AdditionalMethod(int id)
        {
            Console.WriteLine("AdditionalMethod");
            return 1;
            //return context.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
        }

    }
}
