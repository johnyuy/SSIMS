using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace SSIMS.Models
{
    public class Department
    {
        [Display(Name = "Department ID")]
        public string ID { get; set; }
        [Display(Name = "Department Name")]
        public string DeptName { get; set; }
        public Staff DeptRep { get; set; }
        public Staff DeptHead { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }

        public CollectionPoint CollectionPoint { get; set; }
        public DeptHeadAuthorization DeptHeadAuthorization { get; set; }

        public Department()
        {
        }

        public Department(string deptId, string deptName, string phoneNumber, string faxNumber, CollectionPoint collectionPoint,DeptHeadAuthorization deptHeadAuthorization,Staff deptHead,Staff deptRep)
        {
            ID = deptId;
            DeptName = deptName;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
            CollectionPoint = collectionPoint;
            DeptHeadAuthorization = deptHeadAuthorization;
            DeptHead = deptHead;
            DeptRep = deptRep;
        }
    }
}