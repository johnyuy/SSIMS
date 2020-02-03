using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using SSIMS.Database;

namespace SSIMS.Service
{
    public class StaffService: IStaffService
    {

        private UnitOfWork unitOfWork = new UnitOfWork();

        public ICollection<Staff> GetStaff()
        { 
            var staff = unitOfWork.StaffRepository.Get(filter: x => x.StaffRole == "Staff");

            return staff.ToList();
        }

        public Staff GetStaffByUsername(string username)
        {
            if (!String.IsNullOrEmpty(username))
            {
                var staff = unitOfWork.StaffRepository.Get(filter: x => x.UserAccountID == username).First();
                if (staff != null)
                    return staff;
            }
            return null;
        }
    }
}