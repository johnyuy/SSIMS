using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class UserAccount
    {
        [Key]
        public string ID { get; set; }
        public string Password { get; set; }
        public int StoreAccess { get; set; }
        public int DeptAccess { get; set; }

        public UserAccount()
        {
        }

        public UserAccount(string userName, string password, int storeAccess, int deptAccess)
        {
            ID = userName;
            Password = password;
            StoreAccess = storeAccess;
            DeptAccess = deptAccess;
        }
    }
}