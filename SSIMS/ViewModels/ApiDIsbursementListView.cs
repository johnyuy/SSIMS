using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiDisbursementListView
    {
        public string ID;

        public string CreatedByStaffID;
        public string CreatedByStaffName;
        public string RepliedByStaffID;
        public string RepliedByStaffName;
        public string DepartmentID;

        public string Comments;
        public string CreatedDate;
        public string ResponseDate;
        public string Status;

        public List<ApiTransactionItemView> transactionItemViewList;

        public ApiDisbursementListView()
        {
            transactionItemViewList = new List<ApiTransactionItemView>();
        }

        public ApiDisbursementListView(DisbursementList dl)
        {
            ID = dl.ID.ToString();
            CreatedByStaffID = dl.CreatedByStaff.ID.ToString();
            CreatedByStaffName = dl.CreatedByStaff.Name;
            RepliedByStaffID = dl.RepliedByStaff.ID.ToString();
            RepliedByStaffName = dl.RepliedByStaff.Name;
            DepartmentID = dl.Department.ID;

            Comments = dl.Comments;
            CreatedDate = dl.CreatedDate.ToString();
            ResponseDate = dl.ResponseDate.ToString();
            Status = dl.Status.ToString();

            transactionItemViewList = new List<ApiTransactionItemView>();
            foreach (TransactionItem ti in dl.ItemTransactions)
            {
                transactionItemViewList.Add(new ApiTransactionItemView(ti));
            }
        }

        public ApiDisbursementListView(string iD, string createdByStaffID, string createdByStaffName, string repliedByStaffID, string repliedByStaffName, string departmentID, string comments, string createdDate, string responseDate, string status, List<ApiTransactionItemView> transactionItemViewList)
        {
            ID = iD;
            CreatedByStaffID = createdByStaffID;
            CreatedByStaffName = createdByStaffName;
            RepliedByStaffID = repliedByStaffID;
            RepliedByStaffName = repliedByStaffName;
            DepartmentID = departmentID;
            Comments = comments;
            CreatedDate = createdDate;
            ResponseDate = responseDate;
            Status = status;
            this.transactionItemViewList = transactionItemViewList;
        }
    }
}