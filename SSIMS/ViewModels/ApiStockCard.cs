using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;

namespace SSIMS.ViewModels
{
    public class ApiStockCard
    {
        public string Date;
        public string Movement;
        public string QtyChanged;

        public ApiStockCard(string date, string movement, string qtyChanged)
        {
            Date = date;
            Movement = movement;
            QtyChanged = qtyChanged;
        }

        public ApiStockCard(StockCardEntry sce)
        {
            if(sce.DeliveryOrder != null)
            {
                Date = sce.DeliveryOrder.CreatedDate.ToString();
                Movement = "Delivery Order: " + (sce.DeliveryOrder.ID + 100000).ToString();
            }

            else if (sce.RetrievalList != null)
            {
                Date = sce.RetrievalList.CreatedDate.ToString();
                Movement = "Retrival List: " + (sce.RetrievalList.ID + 100000).ToString();
            }

            else if (sce.AdjustmentVoucher != null)
            {
                Date = sce.AdjustmentVoucher.CreatedDate.ToString();
                Movement = "Adjustment Voucher: " + (sce.AdjustmentVoucher.ID + 100000).ToString();
            }
            else
            {
                Date = "Initial";
                Movement = "Initial Stock";
            }

            QtyChanged = sce.QtyChanged.ToString();
        }
    }
}