using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class UserAccount
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int AccessType { get; set; }

        public UserAccount()
        {
        }

        public UserAccount(int UserId,string UserName, string Password,int AccessType)
        {
            userId = UserId;
            userName = UserName;
            password = Password;
            accessType = AccessType;
        }
    }
}