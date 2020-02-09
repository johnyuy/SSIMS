using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiTransactionItemView
    {
        public string ID;
        public string ItemID;
        public string ItemDesc;
        public int HandOverQty;
        public int TakeOverQty;
        public string Reason;

        public ApiTransactionItemView()
        {
        }

        public ApiTransactionItemView(TransactionItem ti)
        {
            ID = ti.ID.ToString();
            ItemID = ti.Item.ID.ToString();
            ItemDesc = ti.Item.Description.ToString();
            HandOverQty = ti.HandOverQty;
            TakeOverQty = ti.TakeOverQty;
            Reason = ti.Reason;
        }


        public ApiTransactionItemView(string iD, string itemDesc, int handOverQty, int takeOverQty, string reason)
        {
            ID = iD;
            ItemDesc = itemDesc;
            HandOverQty = handOverQty;
            TakeOverQty = takeOverQty;
            Reason = reason;
        }
    }
}