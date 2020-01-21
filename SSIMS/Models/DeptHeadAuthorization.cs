using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class DeptHeadAuthorization
    {
        public string AuthorizationID { get; set; }
        public string StaffID { get; set; }

        public Staff ActingDeptHead { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}