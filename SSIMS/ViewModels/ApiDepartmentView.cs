using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiDepartmentView
    {

        public string ID;
        public string DepartmentName;
        public string DeptHead;

        public ApiDepartmentView(string iD, string departmentName, string deptHead)
        {
            ID = iD;
            DepartmentName = departmentName;
            DeptHead = deptHead;
        }

        public ApiDepartmentView(Department department)
        {
            ID = department.ID;
            DepartmentName = department.DeptName;
            if(department.DeptHead != null)
            {
                DeptHead = department.DeptHead.Name;
            }
            else
            {
                DeptHead = "";
            }

        }

        public ApiDepartmentView()
        {
        }
    }
}