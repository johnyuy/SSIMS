using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiDocumentItem
    {
        public String ID;
        public String ItemID;
        public String ItemDesc;
        public String Qty;


        public ApiDocumentItem(string iD, string itemID, string itemDesc, string qty)
        {
            ID = iD;
            ItemID = itemID;
            ItemDesc = itemDesc;
            Qty = qty;
        }
        public ApiDocumentItem(DocumentItem di)
        {
            ID = di.ID.ToString();
            ItemID = di.Item.ID.ToString();
            ItemDesc = di.Item.Description.ToString();
            Qty = di.Qty.ToString();
        }

        public ApiDocumentItem()
        {
        }
    }



}