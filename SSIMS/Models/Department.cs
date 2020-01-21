using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Department
    {
        public int DeptID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public virtual Staff DeptRep { get; set; }
        public virtual Staff DeptHead { get; set; }
        public virtual CollectionPoint CollectionPoint { get; set; }

        public Department()
        {
        }

        public Department(string DeptId, string DeptName, Staff DeptRep, Staff DeptHead)
        {
            deptId = DeptId;
            deptName = DeptName;
            deptRep = DeptRep;
            deptHead = DeptHead;
            collectionPoint = CollectionPoint;
        }
    }
}