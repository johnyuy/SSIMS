﻿using System;
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
            foreach(Staff staff in staffs)
            {
                switch (staff.StaffRole)
                {
                    case "DeptHead":
                        headlist.Add(staff.Email.Split('@')[0]); break;
                    case "DeptRep":
                        replist.Add(staff.Email.Split('@')[0]); break;
                    default:
                        stafflist.Add(staff.Email.Split('@')[0]); break;
                }
                
            }
            namelists.Add(stafflist);
            namelists.Add(replist);
            namelists.Add(headlist);
            return namelists;
        }
    }
}