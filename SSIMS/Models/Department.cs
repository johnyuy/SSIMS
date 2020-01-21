﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace SSIMS.Models
{
    public class Department
    {
        public int DepartmentID { get; set; }
        
        public int CollectionPointID { get; set; }
        public int DeptHeadAutorizationID { get; set; }
        public string DeptCode { get; set; }
        public string DeptName { get; set; }

        [ForeignKey("DeptRepID")]
        public virtual Staff DeptRep { get; set; }
        public int? DeptRepID { get; set; }
        
        [ForeignKey("DeptHeadID")]
        public virtual Staff DeptHead { get; set; }
        public int? DeptHeadID { get; set; }

        public CollectionPoint CollectionPoint { get; set; }
        public DeptHeadAuthorization GetDeptHeadAuthorization { get; set; }

        public Department()
        {
        }

        public Department(int deptRepID, int deptHeadID, string deptCode, string deptName)
        {
            DeptRepID = deptRepID;
            DeptHeadID = deptHeadID;
            DeptCode = deptCode;
            DeptName = deptName;
        }
    }
}