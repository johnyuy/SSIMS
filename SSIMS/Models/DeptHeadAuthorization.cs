using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class DeptHeadAuthorization
    {
        [DisplayName("Dept Head Authorized")]
        public int ID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int DepartmentID { get; set; }
        public Staff Staff { get; set; }
        public DeptHeadAuthorization()
        {
        }

        public DeptHeadAuthorization(Staff staff,int departmentID, DateTime startDate, DateTime endDate)
        {
            Staff = staff;
            DepartmentID = departmentID;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}