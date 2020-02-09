using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using SSIMS.DAL;
using SSIMS.Models;
using SSIMS.ViewModels;

namespace SSIMS.Service
{
    public class DepartmentService
    {
        public List<DeptHeadAuthVM> GetDeptHeadAuthorizationVMs(string DeptID, UnitOfWork uow)
        {
            IEnumerable<DeptHeadAuthorization> auths = uow.DeptHeadAuthorizationRepository.Get(filter: x => x.DepartmentID == DeptID, includeProperties:"Staff");
            if (auths == null)
                return null;

            List<DeptHeadAuthVM> vmlist = new List<DeptHeadAuthVM>();
            foreach(DeptHeadAuthorization auth in auths)
            {
                vmlist.Add(new DeptHeadAuthVM(auth));
            }
            return vmlist;
        }

        public bool IsActiveAuthExist(string deptID, out DeptHeadAuthorization activedept, UnitOfWork uow)
        {
            activedept = uow.DeptHeadAuthorizationRepository.Get(filter: x => x.EndDate.CompareTo(DateTime.Now)==1 && x.DepartmentID==deptID).FirstOrDefault();
            if (activedept == null)
                return false;
            else
                return true;
        }

        public bool SubmitNewAuth(string name, string startDate, string endDate, string deptID)
        {
            UnitOfWork uow = new UnitOfWork();
            //check start is today or future
            if (DateTime.Parse(startDate).CompareTo(DateTime.Now) < 0)
                return false;

            //check enddate is after startdate
            if (DateTime.Parse(startDate).CompareTo(DateTime.Parse(endDate)) >= 0)
                return false;
            //check current auth exist
            if(IsActiveAuthExist(deptID, out _, uow))
                return false;


            //check if there is a staff selected
            if (name == "")
                return false;
            
            uow.DeptHeadAuthorizationRepository.Insert(new DeptHeadAuthorization(name, startDate, endDate, deptID, uow));
            uow.Save();
            Debug.WriteLine("Dep Head Auth for " + deptID + " : " + name + " was inserted into db");

            return true;
        }

        public bool CancelAuth(string deptID)
        {
            UnitOfWork uow = new UnitOfWork();
            if (!IsActiveAuthExist(deptID, out DeptHeadAuthorization auth, uow))
                return false;
            auth.EndDate = DateTime.Now;
            uow.DeptHeadAuthorizationRepository.Update(auth);
            uow.Save();

            return true;
        }
    }
}