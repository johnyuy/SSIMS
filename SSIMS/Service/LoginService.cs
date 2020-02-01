using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;
using System.Diagnostics;

namespace SSIMS.Service
{
    public class LoginService : ILoginService
    {
        UnitOfWork uow = new UnitOfWork();
        public bool VerifyPassword(string username, string password)
        {
            Debug.WriteLine("Verifying Login");
            Debug.WriteLine("\tUsername = " + username);
            Debug.WriteLine("\tPassword = " + password);
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if (account != null && password == account.Password) {
                Debug.WriteLine("\tAuthenthication successful!");
                return true;
            }
            Debug.WriteLine("\tAuthenthication Unsuccessful!");
            return false;
        }
    }
}