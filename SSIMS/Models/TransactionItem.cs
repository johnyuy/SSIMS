using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace SSIMS.Models
{
    public class TransactionItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ID { get; set; }
        public int HandOverQty { get; set; }
        public int TakeOverQty { get; set; }
        public string Reason { get; set; }

        public Item Item { get; set; }

        public TransactionItem()
        {
        }

        public TransactionItem(int handOverQty, int takeOverQty, string reason, Item item)
        {
            HandOverQty = handOverQty;
            TakeOverQty = takeOverQty;
            Reason = reason;
            Item = item;
        }
    }
}