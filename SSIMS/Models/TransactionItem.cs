﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SSIMS.DAL;
using System.Web;
using Foolproof;
using SSIMS.ViewModels;
using System.ComponentModel;

namespace SSIMS.Models
{
    public class TransactionItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid ID { get; set; }
        [DisplayName("Handover Qty")]
        public int HandOverQty { get; set; }

        //[LessThanOrEqualTo("HandOverQty")]
        [DisplayName("Takeover Qty")]
        public int TakeOverQty { get; set; }

        public string Reason { get; set; }

        public virtual Item Item { get; set; }

        public virtual Document Document { get; set; }

        UnitOfWork uow = new UnitOfWork();

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

        public TransactionItem(int handOverQty, int takeOverQty, string reason, Item item, Document document)
        {
            HandOverQty = handOverQty;
            TakeOverQty = takeOverQty;
            Reason = reason;
            Item = item;
            Document = document;
        }

        public TransactionItem(int handOverQty, int takeOverQty, string reason, string itemID, UnitOfWork uow)
        {
            Item = uow.ItemRepository.Get(filter: i => i.ID == itemID).FirstOrDefault();
            HandOverQty = handOverQty;
            TakeOverQty = takeOverQty;
            Reason = reason;
        }

        public TransactionItem(DeptRetrievalItemViewModel drvm, UnitOfWork uow)
        {
            
            string itemID = drvm.transactionItem.Item.ID;
            Item = uow.ItemRepository.GetByID(itemID);
            HandOverQty = drvm.transactionItem.HandOverQty;
            TakeOverQty = drvm.transactionItem.TakeOverQty;
            Reason = drvm.transactionItem.Reason;


        }

        public TransactionItem(string itemID, UnitOfWork uow)
        {
            Item = uow.ItemRepository.GetByID(itemID);
        }
    }
}