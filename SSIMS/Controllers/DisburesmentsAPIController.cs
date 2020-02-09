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
    public class DisbursementsAPIController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();

        // GET: api/DisburesmentsAPI
        [HttpGet]
        public List<ApiDisbursementListListView> Get(string q)
        {
            List<DisbursementList> disbursements = new List<DisbursementList>();
            List<ApiDisbursementListListView> apiDisburementList = new List<ApiDisbursementListListView>();

            if(q == null || q == "")
            {
                disbursements = uow.DisbursementListRepository.Get(filter: x => x.Status.ToString() == "Pending", includeProperties: "Department").ToList();
            }
            else
            {
                disbursements = uow.DisbursementListRepository.Get(filter: x=> x.Status.ToString() == "Pending" && x.Department.CollectionPoint.Location == q, includeProperties: "Department").ToList();
            }

            foreach(DisbursementList dl in disbursements)
            {
                apiDisburementList.Add(new ApiDisbursementListListView(dl, uow));
            }

            return apiDisburementList;
        }

        [HttpGet]
        public ApiDisbursementListView GetByID(int id)
        {

            DisbursementList dl = uow.DisbursementListRepository.Get(filter: x => x.ID == id, includeProperties: "Department, CreatedByStaff, RepliedByStaff").FirstOrDefault();
            ApiDisbursementListView apiDisbursementListView = new ApiDisbursementListView(dl);
            return apiDisbursementListView;
        }

    }
}
