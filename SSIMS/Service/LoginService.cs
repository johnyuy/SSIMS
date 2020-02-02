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
            Debug.WriteLine("\n[Login Attempt]");
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if (account != null && password == account.Password) {
                Debug.WriteLine("ACCESS GRANTED: " + account.ID);
                GenerateIdentity(account);
                return true;
            }
            Debug.WriteLine("\tACCESS DENIED!\n");
            return false;
        }

        public void CreateNewSession(string username, string sessionId)
        {
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if(account != null)
            {
                account.SessionID = sessionId;
                uow.UserAccountRepository.Update(account);
                uow.Save();
                Debug.Print("\tUpdated SessionID : " + account.SessionID);
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

        public void CancelSession(string username)
        {
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if (account != null)
            {
                account.SessionID = null;
                uow.UserAccountRepository.Update(account);
                uow.Save();
                Debug.Print("Session for " + account.ID + " ended");
            }
        }
        private static void GenerateIdentity(UserAccount userAccount)
        {
            List<string> roles = new List<string>();
            int storeaccess = userAccount.StoreAccess;
            int deptaccess = userAccount.DeptAccess;
            if(storeaccess + deptaccess == 6)
            {
                roles.Add("manager");
                roles.Add("supervisor");
                roles.Add("clerk");
                roles.Add("head");
                roles.Add("rep");
                roles.Add("staff");
            }
            else if (storeaccess == 3 && deptaccess ==0)
            {
                roles.Add("manager");
                roles.Add("supervisor");
                roles.Add("clerk");
            }
            else if (storeaccess == 2 && deptaccess == 0)
            {
                roles.Add("supervisor");
                roles.Add("clerk");
            }
            else if (storeaccess == 1 && deptaccess == 0)
            {
                roles.Add("clerk");

            }
            else if (storeaccess == 0 && deptaccess == 3)
            {
                roles.Add("head");
                roles.Add("rep");
                roles.Add("staff");
            }
            else if (storeaccess == 0 && deptaccess == 2)
            {
                roles.Add("rep");
                roles.Add("staff");
            }
            else if (storeaccess == 0 && deptaccess == 1)
            {
                roles.Add("staff");
            }

            if (roles.Count>0)
            {
                HttpContext.Current.Session["username"] = userAccount.ID;
                HttpContext.Current.Session["roles"] = roles;
            }
        }
    }
}