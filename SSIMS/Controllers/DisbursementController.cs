using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSIMS.Models;

namespace SSIMS.Controllers
{
    public class DisbursementController : Controller
    {
        public ICollection<TransactionItem> GenerateRetrievalList(RequisitionOrder requisition)
        {
            ArrayList retrievalList = new ArrayList();
            ArrayList requisitionArray = new ArrayList(requisition.DocumentItems.ToArray());
            foreach(DocumentItem doc in requisitionArray)
            {

            }
            return null;
        }

    }
}