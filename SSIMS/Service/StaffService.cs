using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using SSIMS.Database;

namespace SSIMS.Service
{
    public class StaffService
    {

        private UnitOfWork unitOfWork = new UnitOfWork();

        public ICollection<Staff> GetStaff()
        { 
            var staff = unitOfWork.StaffRepository.Get(filter: x => x.StaffRole == "Staff");

            return staff.ToList();
        }

    }
}