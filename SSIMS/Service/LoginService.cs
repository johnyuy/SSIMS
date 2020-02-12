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

        public bool VerifyPasswordApi(string username, string password)
        {
            Debug.WriteLine("\n[Login Attempt]");
            UserAccount account = uow.UserAccountRepository.GetByID(username);
            if (account != null && password == account.Password)
            {
                Debug.WriteLine("\tACCESS GRANTED: " + account.ID);

                //GenerateIdentity(account);
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

        public static bool IsAuthorizedRoles(string authorizedRole)
        {
            string actualrole = HttpContext.Current.Session["role"].ToString();
            if (actualrole.Equals(authorizedRole)
                || actualrole.Equals("admin")
                || authorizedRole.Equals("all"))
                return true;

            return false;
        }

        public static bool IsAuthorizedRoles(string authorizedRole1, string authorizedRole2)
        {
            string actualrole = HttpContext.Current.Session["role"].ToString();
            if (actualrole.Equals(authorizedRole1)
                || actualrole.Equals(authorizedRole2)
                || actualrole.Equals("admin"))
                return true;
            return false;
        }

        public static bool IsAuthorizedRoles(string authorizedRole1, string authorizedRole2, string authorizedRole3)
        {
            string actualrole = HttpContext.Current.Session["role"].ToString();
            if (actualrole.Equals(authorizedRole1)
                || actualrole.Equals(authorizedRole2)
                || actualrole.Equals(authorizedRole3)
                || actualrole.Equals("admin"))
                return true;
            return false;
        }

        public string UserDepartment
        {
            get
            {
                if (HttpContext.Current.Session["username"] == null)
                    return null;
                string username = HttpContext.Current.Session["username"].ToString();
                Staff staff = uow.StaffRepository.Get(filter: x => x.UserAccountID == username, includeProperties:"Department").FirstOrDefault();

                return staff.DepartmentID;
            }
        }


        public bool StaffToAuthorizedHead(string staffname, bool isAuth)
        {
            UnitOfWork uow = new UnitOfWork();
            Staff staff = uow.StaffRepository.Get(filter: x => x.Name == staffname).FirstOrDefault();
            if (staff == null) return false;
            if (staff.StaffRole != "DeptRep" && staff.StaffRole != "Staff")
                return false;
            string acc = staff.UserAccountID;
            UserAccount account = uow.UserAccountRepository.GetByID(acc);
            if(account == null) return false;
            if(isAuth)
            {
                account.DeptAccess = 3;
            }
            else
            {
                if (staff.StaffRole == "DeptRep")
                    account.DeptAccess = 2;
                else
                    account.DeptAccess = 1;
            }
            uow.UserAccountRepository.Update(account);
            uow.Save();
            return true;
        }

        public void UpdateDeptAccessByRole(string staffname, UnitOfWork uow)
        {
            Staff staff = uow.StaffRepository.Get(filter: x => x.Name == staffname).FirstOrDefault();
            if (staff == null) return;
            Debug.WriteLine("Updating by role for " + staff.Name);
            string acc = staff.UserAccountID;
            UserAccount account = uow.UserAccountRepository.GetByID(acc);
            if (account == null) return;
            if (staff.StaffRole == "DeptRep")
            {
                account.DeptAccess = 2;
            }
            else if (staff.StaffRole == "Staff")
            {
                account.DeptAccess = 1;
            }
            uow.UserAccountRepository.Update(account);
            uow.Save();
        }
    }
}