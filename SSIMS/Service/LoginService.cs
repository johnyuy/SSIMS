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
                Debug.WriteLine("\tLogin successful!");
                return true;
            }
            Debug.WriteLine("\tLogin Unsuccessful!");
            return false;
        }

        public void CreateNewSession(string username, string sessionId)
        {
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if(account != null)
            {
                Debug.Print("\tOld SessionID for " + account.ID + " = " + account.SessionID);
                account.SessionID = sessionId;
                uow.UserAccountRepository.Update(account);
                uow.Save();
                Debug.Print("\tNew SessionID for " + account.ID + " = " + account.SessionID);
            }
        }

        public bool AuthenticateSession(string username, string sessionId)
        {
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if(account != null)
            {
                if (sessionId == account.SessionID)
                    return true;
            }
            return false;
        }
    }
}