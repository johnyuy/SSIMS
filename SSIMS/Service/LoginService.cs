using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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
                Debug.WriteLine("\tACCESS GRANTED: " + account.ID);

                GenerateIdentity(account);
                return true;
            }
            Debug.WriteLine("\tACCESS DENIED!\n");
            return false;
        }

        public string UpdateSession(string username, string sessionId)
        {
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if(account != null)
            {
                account.SessionID = sessionId;
                uow.UserAccountRepository.Update(account);
                uow.Save();
                Debug.Print("\tUpdated SessionID : " + account.SessionID);
            }
            return sessionId;
        }

        public bool AuthenticateSession(string username, string sessionId)
        {
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if(account != null)
            {
                if (sessionId == account.SessionID)
                {
                    
                    return true;
                }
                    
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

        public bool IsStoredSession(HttpCookie AuthCookie)
        {
            string username = AuthCookie.Value.Split('#')[0];
            string sessionid = AuthCookie.Value.Split('#')[1];
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if (account != null)
            {
                if (sessionid == account.SessionID)
                {
                    Debug.WriteLine("\n[Resume Session : " + username + "/" + sessionid+"]");
                    UpdateSession(username, HttpContext.Current.Session.SessionID);
                    GenerateIdentity(account);
                    return true;
                }
            }
            return false;
        }

       
        private static void GenerateIdentity(UserAccount userAccount)
        {
            string role = "";
            int storeaccess = userAccount.StoreAccess;
            int deptaccess = userAccount.DeptAccess;
            if(storeaccess + deptaccess == 6)
            {
                role="admin";
            }
            else if (storeaccess == 3 && deptaccess ==0)
            {
                role="manager";
            }
            else if (storeaccess == 2 && deptaccess == 0)
            {
                role="supervisor";
            }
            else if (storeaccess == 1 && deptaccess == 0)
            {
                role="clerk";

            }
            else if (storeaccess == 0 && deptaccess == 3)
            {
                role="head";
            }
            else if (storeaccess == 0 && deptaccess == 2)
            {
                role="rep";
            }
            else if (storeaccess == 0 && deptaccess == 1)
            {
                role="staff";
            }

            if (role!="")
            {
                HttpContext.Current.Session["username"] = userAccount.ID;
                HttpContext.Current.Session["role"] = role;
            }
        }

        public Staff StaffFromSession
        {
            get
            {
                //this method get up to Staff's Department property primitives
                if (HttpContext.Current.Session["username"] == null)
                    return null;
                string username = HttpContext.Current.Session["username"].ToString();
                Staff staff = uow.StaffRepository.Get(filter: x => x.UserAccountID == username).FirstOrDefault();

                return staff;
            }
        }

        public bool  AuthorizeRole(string role)
        {
            return false;
        }

        public bool AuthorizeRole(string role1, string role2)
        {
            return false;
        }

        public bool AuthorizeRole(string role1, string role2, string role3)
        {
            return false;
        }
    }
}