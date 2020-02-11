using SSIMS.Models;
using System.Web;
using SSIMS.DAL;

namespace SSIMS.Service
{
    interface ILoginService
    {
        bool VerifyPassword(string username, string password);
        bool VerifyPasswordApi(string username, string password);
        string UpdateSession(string username, string sessionId);
        bool AuthenticateSession(string username, string sessionId);
        bool IsStoredSession(HttpCookie AuthCookie);
        void CancelSession(string username);
        bool StaffToAuthorizedHead(string staffname, bool isAuth);
        void UpdateDeptAccessByRole(string staffname, UnitOfWork uow);
        Staff StaffFromSession { get; }
    }
}
