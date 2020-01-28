using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSIMS.Models
{
    public class Department
    {
        public string ID { get; set; }
        public int? DeptRepID { get; set; }
        public int? DeptHeadID { get; set; }
        public int? CollectionPointID { get; set; }
        public int? DeptHeadAutorizationID { get; set; }
        public string DeptName { get; set; }

        [ForeignKey("DeptRepID")]
        public Staff DeptRep { get; set; }

        [ForeignKey("DeptHeadID")]
        public Staff DeptHead { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }

        public CollectionPoint CollectionPoint { get; set; }
        public DeptHeadAuthorization DeptHeadAuthorization { get; set; }

        public Department()
        {
        }

        public Department(string deptId, string deptName, string phoneNumber, string faxNumber)
        {
            ID = deptId;
            DeptName = deptName;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
        }
    }
}