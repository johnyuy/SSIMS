using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SSIMS.DAL;

namespace SSIMS.Models
{
    public class DocumentItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ID {get; set;}

        public Document Document { get; set; }
        public Item Item { get; set; }
        [Display(Name = "Quantity")]
        public int Qty { get; set; }

        public DocumentItem()
        {
        }

        public DocumentItem( int qty, Item item, Document document, UnitOfWork uow)
        {
            Qty = qty;
            Item = uow.ItemRepository.GetByID(item.ID); 
            Document = document;
        }

        public DocumentItem(string itemID, int qty, UnitOfWork uow)
        {
            Item = uow.ItemRepository.GetByID(itemID);
            Qty = qty;
        }

        public DocumentItem(Item item, int qty)
        {
            Item = item;
            Qty = qty;
        }

        public DocumentItem(TransactionItem transactionItem, UnitOfWork uow)
        {
            Item = uow.ItemRepository.GetByID(transactionItem.Item.ID);
            Qty = transactionItem.TakeOverQty;
        }

        public DocumentItem(TransactionItem transactionItem)
        {
            Item = transactionItem.Item;
            Qty = transactionItem.TakeOverQty;
        }

    }
}