﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.ViewModels
{
    public class InventoryStockCardVM
    {
        public string Date { get; set; }
        public string Movement { get; set; }
        public int QtyChange { get; set; }
        public int InStoreBalance { get; set; }
        public InventoryStockCardVM(string date, string movement, int qtyChange, int inStoreBalance)
        {
            Date = date;
            Movement = movement;
            QtyChange = qtyChange;
            InStoreBalance = inStoreBalance;
        }
        public override string ToString()
        {
            return Date + " | " + Movement + " | " + QtyChange + " | " + InStoreBalance;
        }
    }
}