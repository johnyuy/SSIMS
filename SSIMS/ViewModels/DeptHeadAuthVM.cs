using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class DeptHeadAuthVM
    {
        public int ID { get; set; }
        [Display(Name="Start Date")]
        public string StartDate { get; set; }
        [Display(Name = "End Date")]
        public string EndDate { get; set; }
        public string DeptID { get; set; }
        [Display(Name = "Staff")]
        public string StaffName { get; set; }

        public DeptHeadAuthVM(DeptHeadAuthorization auth)
        {
            ID = auth.ID;
            StartDate = auth.StartDate.ToString("dd/MM/yyyy");
            EndDate = auth.EndDate.ToString("dd/MM/yyyy");
            DeptID = auth.DepartmentID;
            StaffName = auth.Staff.Name;
        }

        public DeptHeadAuthVM() { }
    }
}