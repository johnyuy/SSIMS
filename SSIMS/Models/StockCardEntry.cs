using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class StockCardEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ID { get; set; }
        public virtual DeliveryOrder DeliveryOrder { get; set; }
        public virtual RetrievalList RetrievalList { get; set; }
        public virtual AdjustmentVoucher AdjustmentVoucher { get; set; }
        public Item Item { get; set; }
        public int QtyChanged { get; set; }

        public StockCardEntry()
        {
        }

        public StockCardEntry(Item item, int QuantityChanged, DeliveryOrder deliveryOrder)
        {
            Item = item;
            QtyChanged = QuantityChanged;
            RetrievalList = null;
            DeliveryOrder = deliveryOrder;
            AdjustmentVoucher = null;
        }
        public StockCardEntry(Item item, int QuantityChanged, RetrievalList retrievalList)
        {
            Item = item;
            QtyChanged = QuantityChanged;
            RetrievalList = retrievalList;
            DeliveryOrder = null;
            AdjustmentVoucher = null;
        }

        public StockCardEntry(Item item, int QuantityChanged, AdjustmentVoucher adjustmentVoucher)
        {
            Item = item;
            QtyChanged = QuantityChanged;
            RetrievalList = null; ;
            DeliveryOrder = null;
            AdjustmentVoucher = adjustmentVoucher;
        }

        public StockCardEntry(Item item, int QuantityChanged)
        {
            Item = item;
            QtyChanged = QuantityChanged;
            RetrievalList = null; ;
            DeliveryOrder = null;
            AdjustmentVoucher = null;
        }
    }
}