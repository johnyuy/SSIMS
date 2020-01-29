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
        public int StaffID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Staff Staff { get; set; }

        public DeptHeadAuthorization()
        {
        }

        public DeptHeadAuthorization(int staffID, DateTime startDate, DateTime endDate)
        {
            StaffID = staffID;
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}