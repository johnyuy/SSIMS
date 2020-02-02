using System.Web;

namespace SSIMS.Service
{
    interface ILoginService
    {
        bool VerifyPassword(string username, string password);
        string CreateNewSession(string username, string sessionId);
        bool AuthenticateSession(string username, string sessionId);
        bool IsStoredSession(HttpCookie AuthCookie);
        void CancelSession(string username);
    }
}
