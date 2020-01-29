using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using System.Diagnostics;
using System.Collections;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.Service
{
    public class DisbursementService
    {
        static DatabaseContext context = new DatabaseContext();
        RequisitionOrderRepository ROrepo = new RequisitionOrderRepository(context);
        public List<TransactionItem> GenerateDeptRetrievalList(string dept)
        {
            //requisitionOrders are approved
            
            List<TransactionItem> transItemList = new List<TransactionItem>();
            List<DocumentItem> docItemsArray = new List<DocumentItem>();
            ICollection<TransactionItem> retrievalList;

            //retrieve all RO with status approved
            var approvedRO = ROrepo.Get(filter: x => x.Status.ToString() == "Approved");

            foreach(RequisitionOrder RO in approvedRO)
            {
                Debug.WriteLine(RO.ID);
            }
            //filter by department
            
            //get all ROs DI into a combined list

            //group all DI by item.ID, construct TI with sum of qty in DIs

            //add TI into a list
           

            return null;
        }
    }
}