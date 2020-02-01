using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Database;
using SSIMS.Models;

namespace SSIMS.DAL
{
    public class UserAccountRepository : GenericRepository<UserAccount>
    {
        public UserAccountRepository(DatabaseContext context)
            : base(context)
        {
        }
    }
}