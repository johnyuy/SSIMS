using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using System.Diagnostics;

namespace SSIMS.Service
{
    public class LoginService : ILoginService
    {
        UnitOfWork uow = new UnitOfWork();
        public bool VerifyPassword(string username, string password)
        {
            Debug.WriteLine("Verifying Login username and password");
            Debug.WriteLine("Username = " + username);
            Debug.WriteLine("Password = " + password);

            return false;
        }
    }
}