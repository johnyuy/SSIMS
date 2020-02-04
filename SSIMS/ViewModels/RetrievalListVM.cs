using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.ViewModels
{
    public class RetrievalListVM
    {
        public string Department;
        public List<RetrievalListItemVM> DepartmentRetrievalList;

        public RetrievalListVM(string department, List<RetrievalListItemVM> departmentRetrievalList)
        {
            Department = department;
            DepartmentRetrievalList = departmentRetrievalList;
        }
    }
}