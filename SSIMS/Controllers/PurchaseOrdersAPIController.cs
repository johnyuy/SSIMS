using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSIMS.ViewModels;
using SSIMS.Models;
using SSIMS.DAL;

namespace SSIMS.Controllers
{
    public class PurchaseOrdersAPIController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();

        [HttpGet]
        public List<ApiPurchaseOrdersListView> Get(string q)
        {
            List<PurchaseOrder> purchaseOrders = new List<PurchaseOrder>();
            List<ApiPurchaseOrdersListView> apiPurchaseOrders = new List<ApiPurchaseOrdersListView>();

            purchaseOrders = uow.PurchaseOrderRepository.Get(includeProperties: "CreatedByStaff, RepliedByStaff").ToList();

            foreach (PurchaseOrder po in purchaseOrders)
            {
                apiPurchaseOrders.Add(new ApiPurchaseOrdersListView(po));
            }

            return apiPurchaseOrders;
        }

        [HttpGet]
        public ApiPurchaseOrderDetails GetByID(int id)
        {

            PurchaseOrder po = uow.PurchaseOrderRepository.Get(filter: x => x.ID == id, includeProperties: "PurchaseItems, CreatedByStaff, RepliedByStaff, Supplier, PurchaseItems.Tender").FirstOrDefault();
            ApiPurchaseOrderDetails apiDisbursementListView = new ApiPurchaseOrderDetails(po);
            return apiDisbursementListView;
        }

        [HttpPost]
        public bool ApproveRejectPurchaseOrder(int id, int response, string staffusername)
        {
            bool result = false;
            if (response == 1)//Approve
            {
                try
                {
                    PurchaseOrder po = uow.PurchaseOrderRepository.GetByID(id);

                    Staff approver = uow.StaffRepository.Get(filter: x => x.UserAccountID == staffusername).FirstOrDefault();
                    po.Approve(approver);
                    uow.PurchaseOrderRepository.Update(po);
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
                    PurchaseOrder po = uow.PurchaseOrderRepository.GetByID(id);
                    Staff rejector = uow.StaffRepository.Get(filter: x => x.UserAccountID == staffusername).FirstOrDefault();
                    po.Rejected(rejector);
                    uow.PurchaseOrderRepository.Update(po);
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
