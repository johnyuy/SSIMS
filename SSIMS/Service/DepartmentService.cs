using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;
using SSIMS.ViewModels;

namespace SSIMS.Service
{
    public class DepartmentService
    {
        UnitOfWork uow = new UnitOfWork();
        public List<DeptHeadAuthVM> GetDeptHeadAuthorizationVMs(string DeptID)
        {
            IEnumerable<DeptHeadAuthorization> auths = uow.DeptHeadAuthorizationRepository.Get(filter: x => x.DepartmentID == DeptID);
            if (auths == null)
                return null;

            List<DeptHeadAuthVM> vmlist = new List<DeptHeadAuthVM>();
            foreach(DeptHeadAuthorization auth in auths)
            {
                vmlist.Add(new DeptHeadAuthVM(auth));
            }
            return vmlist;
        }

        public bool IsActiveAuthExist(string deptID, out DeptHeadAuthorization activedept)
        {
            activedept = uow.DeptHeadAuthorizationRepository.Get(filter: x => x.EndDate.CompareTo(DateTime.Now)==1 && x.DepartmentID==deptID).FirstOrDefault();
            if (activedept == null)
                return false;
            else
                return true;
        }
    }
}