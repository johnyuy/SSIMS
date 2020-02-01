using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIMS.Service
{
    interface ILoginService
    {
        bool VerifyPassword(string uesrname, string password);
    }
}
