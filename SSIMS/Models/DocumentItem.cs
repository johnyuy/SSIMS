using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class DocumentItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ID {get; set;}

        public Document Document { get; set; }
        public Item Item { get; set; }

        public int Qty { get; set; }

        public DocumentItem()
        {
        }

        public DocumentItem(int itemID, int documentID, int qty, Item item, Document document)
        {
            //ItemID = itemID;
            //DocumentID = documentID;
            Qty = qty;
            Item = item;
            Document = document;
        }

        public DocumentItem(Item item, int qty)
        {
            Item = item;
            Qty = qty;
        }
    }
}