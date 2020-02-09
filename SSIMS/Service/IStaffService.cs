using SSIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIMS.Service
{
    interface IStaffService
    {
        ICollection<Staff> GetStaff();
        Staff GetStaffByUsername(string username);
        List<Staff> GetStaffByDeptID(string deptID);
    }
}
