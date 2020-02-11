using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiRequisitionDetailsView
    {

        public string ID;

        public string CreatedByStaffID;
        public string CreatedByStaffName;
        public string RepliedByStaffID;
        public string RepliedByStaffName;

        public string Comments;
        public string CreatedDate;
        public string ResponseDate;
        public string Status;

        public List<ApiDocumentItem> DocumentItems;

        public ApiRequisitionDetailsView()
        {
        }

        public ApiRequisitionDetailsView(string iD, string createdByStaffID, string createdByStaffName, string repliedByStaffID, string repliedByStaffName, string comments, string createdDate, string responseDate, string status, List<ApiDocumentItem> documentItems)
        {
            ID = iD;
            CreatedByStaffID = createdByStaffID;
            CreatedByStaffName = createdByStaffName;
            RepliedByStaffID = repliedByStaffID;
            RepliedByStaffName = repliedByStaffName;
            Comments = comments;
            CreatedDate = createdDate;
            ResponseDate = responseDate;
            Status = status;
            DocumentItems = documentItems;
        }

        public ApiRequisitionDetailsView(RequisitionOrder RO)
        {
            ID = RO.ID.ToString();
            CreatedByStaffID = RO.CreatedByStaff.ID.ToString();
            CreatedByStaffName = RO.CreatedByStaff.Name.ToString();
            RepliedByStaffID = RO.RepliedByStaff.ID.ToString();
            RepliedByStaffName = RO.RepliedByStaff.Name.ToString();
            Comments = RO.Comments;
            CreatedDate = RO.CreatedDate.ToString();
            ResponseDate = RO.ResponseDate.ToString();
            Status = RO.Status.ToString();
            DocumentItems = new List<ApiDocumentItem>();

            foreach (DocumentItem di in RO.DocumentItems)
            {
                DocumentItems.Add(new ApiDocumentItem(di));
            }

        }



    }
}