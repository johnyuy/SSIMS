using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSIMS.Database;
using SSIMS.Models;
using SSIMS.ViewModels;
using System.Diagnostics;
using SSIMS.DAL;

namespace SSIMS.Controllers
{
    public class RequisitionAPIController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();

        [HttpGet]
        public List<ApiRequisitionListView> Get(string username)
        {
            Staff currentstaff = uow.StaffRepository.Get(filter: x => x.UserAccountID == username, includeProperties: "Department").FirstOrDefault();
            string department =  currentstaff.Department.ID;

            List<RequisitionOrder> requisitons = new List<RequisitionOrder>();
            List<ApiRequisitionListView> apiRequisitionList = new List<ApiRequisitionListView>();

            requisitons = uow.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.Department.ID == department, includeProperties: "DocumentItems, CreatedByStaff, RepliedByStaff, DocumentItems.Item").ToList();

            foreach (RequisitionOrder req in requisitons)
            {
                apiRequisitionList.Add(new ApiRequisitionListView(req));
            }

            return apiRequisitionList;
        }

        [HttpGet]
        public List<ApiRequisitionListView> GetReqByStaff(string staffusername)
        {
            Staff currentstaff = uow.StaffRepository.Get(filter: x => x.UserAccountID == staffusername, includeProperties: "Department").FirstOrDefault();
            int staffID = currentstaff.ID;

            List<RequisitionOrder> requisitons = new List<RequisitionOrder>();
            List<ApiRequisitionListView> apiRequisitionList = new List<ApiRequisitionListView>();

            requisitons = uow.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaff.ID == staffID, includeProperties: "DocumentItems, CreatedByStaff, RepliedByStaff, DocumentItems.Item").ToList();

            foreach (RequisitionOrder req in requisitons)
            {
                apiRequisitionList.Add(new ApiRequisitionListView(req));
            }

            return apiRequisitionList;
        }


        [HttpGet]
        public ApiRequisitionDetailsView GetByID(int id)
        {

            RequisitionOrder ro = uow.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems, CreatedByStaff, RepliedByStaff,DocumentItems.Item ").FirstOrDefault();
            ApiRequisitionDetailsView apiRequisitionDetails = new ApiRequisitionDetailsView(ro);
            return apiRequisitionDetails;
        }

        [HttpPost]
        public bool ApproveRejectRequisitionOrder(int id, int response, string staffusername)
        {

            bool result = false;
            if (response == 2)//Approve
            {
                try
                {
                    RequisitionOrder ro = uow.RequisitionOrderRepository.GetByID(id);

                    Staff approver = uow.StaffRepository.Get(filter: x => x.UserAccountID == staffusername).FirstOrDefault();
                    ro.Cancelled(approver);
                    uow.RequisitionOrderRepository.Update(ro);
                    uow.Save();
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }
            }
            if (response == 1)//Approve
            {
                try
                {
                    RequisitionOrder ro = uow.RequisitionOrderRepository.GetByID(id);

                    Staff approver = uow.StaffRepository.Get(filter: x => x.UserAccountID == staffusername).FirstOrDefault();
                    ro.Approve(approver);
                    uow.RequisitionOrderRepository.Update(ro);
                    uow.Save();
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }
            }
            if (response == 0)//Reject
            {
                try
                {
                    RequisitionOrder ro = uow.RequisitionOrderRepository.GetByID(id);
                    Staff rejector = uow.StaffRepository.Get(filter: x => x.UserAccountID == staffusername).FirstOrDefault();
                    ro.Rejected(rejector);
                    uow.RequisitionOrderRepository.Update(ro);
                    uow.Save();
                    result = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    result = false;
                }

            }
            return result;
        }
    }
}
