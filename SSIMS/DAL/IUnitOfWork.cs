using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIMS.DAL
{
    interface IUnitOfWork : IDisposable
    {
        
        void Save();

    }
}
