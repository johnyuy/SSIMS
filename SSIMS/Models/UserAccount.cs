using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class UserAccount
    {
        public int UserAccountID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int StoreAccess { get; set; }
        public int DeptAccess { get; set; }

        public UserAccount()
        {
        }

        public UserAccount(string userName, string password, int storeAccess, int deptAccess)
        {
            Username = userName;
            Password = password;
            StoreAccess = storeAccess;
            DeptAccess = deptAccess;
        }
    }
}