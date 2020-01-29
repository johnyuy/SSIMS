using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;
using System.Diagnostics;

namespace SSIMS.DAL
{
    public class StaffRepository : GenericRepository<Staff>
    {
        public StaffRepository(DatabaseContext context)
            : base(context)
        {
        }


        public List<List<string>> GetStaffAccountNames()
        {
            List<Staff> staffs = (List<Staff>) Get();
            //Debug.WriteLine( "number of staff = " + staffs.Count);
            if (staffs.Count == 0)
                return null;

            List<List<string>> namelists = new List<List<string>>();
            List<string> stafflist = new List<string>();
            List<string> replist = new List<string>();
            List<string> headlist = new List<string>();
            List<string> clerklist = new List<string>();
            List<string> supervisorlist = new List<string>();
            List<string> managerlist = new List<string>();

            foreach(Staff staff in staffs)
            {
                switch (staff.StaffRole)
                {
                    case "DeptHead":
                        headlist.Add(staff.Email.Split('@')[0]); break;
                    case "DeptRep":
                        replist.Add(staff.Email.Split('@')[0]); break;
                    case "Manager":
                        managerlist.Add(staff.Email.Split('@')[0]); break;                    
                    case "Supervisor":
                        supervisorlist.Add(staff.Email.Split('@')[0]); break;
                    case "Clerk":
                        clerklist.Add(staff.Email.Split('@')[0]); break;
                    case "Staff":
                        stafflist.Add(staff.Email.Split('@')[0]); break;
                }
                
            }
            namelists.Add(stafflist);
            namelists.Add(replist);
            namelists.Add(headlist);
            namelists.Add(clerklist);
            namelists.Add(supervisorlist);
            namelists.Add(managerlist);

            return namelists;
        }
    }
}