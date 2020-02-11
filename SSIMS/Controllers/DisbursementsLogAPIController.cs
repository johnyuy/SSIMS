using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSIMS.ViewModels;
using SSIMS.DAL;
using SSIMS.Models;

namespace SSIMS.Controllers
{
    public class DisbursementsLogAPIController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();

        [HttpGet]
        public List<ApiDisbursementListLogView> Get(string q)
        {
            List<DisbursementList> disbursements = new List<DisbursementList>();
            List<ApiDisbursementListLogView> apiDisburementList = new List<ApiDisbursementListLogView>();

            disbursements = uow.DisbursementListRepository.Get(filter: x => x.Status.ToString() == "Completed", includeProperties: "Department").ToList();

            foreach (DisbursementList dl in disbursements)
            {
                apiDisburementList.Add(new ApiDisbursementListLogView(dl));
            }

            return apiDisburementList;
        }

    }
}
