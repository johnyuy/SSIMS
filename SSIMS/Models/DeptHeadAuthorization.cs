using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SSIMS.ViewModels;
using SSIMS.DAL;

namespace SSIMS.Models
{
    public class DeptHeadAuthorization
    {
        [Key]
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string DepartmentID { get; set; }
        public Staff Staff { get; set; }

        public DeptHeadAuthorization()
        {
        }

        public DeptHeadAuthorization(Staff staff,string departmentID, DateTime startDate, DateTime endDate)
        {
            Staff = staff;
            DepartmentID = departmentID;
            StartDate = startDate;
            EndDate = endDate;
        }

        public DeptHeadAuthorization(DeptHeadAuthVM vm, UnitOfWork uow)
        {
            StartDate = DateTime.Parse(vm.StartDate);
            EndDate = DateTime.Parse(vm.EndDate);
            DepartmentID = vm.DeptID;
            Staff = uow.StaffRepository.Get(filter: x => x.Name == vm.StaffName).FirstOrDefault();

        }
    }
}