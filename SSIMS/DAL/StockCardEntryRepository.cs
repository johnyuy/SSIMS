using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;
using System.Diagnostics;

namespace SSIMS.DAL
{
    public class StockCardEntryRepository : GenericRepository<StockCardEntry>
    {
        public StockCardEntryRepository(DatabaseContext context): base(context)
        {
        }

        public bool ProcessDeliveryOrderAcceptance(DeliveryOrder DO)
        {
            if (DO == null)
                return false;
            List<DocumentItem> items = DO.DocumentItems?.ToList();
            if (items == null || items.Count==0)
                return false;

            bool isTransactionOk = true;
            List<StockCardEntry> transactions = new List<StockCardEntry>();
            foreach(DocumentItem item in items)
            {
                if (item.Item == null || item.Qty < 1)
                {
                    isTransactionOk = false;
                    break;
                } else
                {
                    transactions.Add(new StockCardEntry(item.Item, item.Qty, DO));
                }
            }
            if (isTransactionOk)
            {
                foreach(StockCardEntry transaction in transactions)
                    Insert(transaction);
                return true;
            }
            else
            {
                Debug.WriteLine("ERROR: DELIVERY ORDER ACCEPTANCE TRANSACTION FAILED");
                Debug.WriteLine("DO-ID: " + DO.ID);
                return false;
            }
        }

        public bool ProcessRetrivalListCompletion(RetrievalList RL)
        {
            if (RL == null)
                return false;
            List<TransactionItem> items = RL.ItemTransactions?.ToList();
            if (items == null || items.Count == 0)
                return false;

            bool isTransactionOk = true;
            List<StockCardEntry> transactions = new List<StockCardEntry>();
            foreach (TransactionItem item in items)
            {
                if (item.Item == null)
                {
                    isTransactionOk = false;
                    break;
                }
                else
                {
                    transactions.Add(new StockCardEntry(item.Item, -item.TakeOverQty,RL));
                }
            }
            if (isTransactionOk)
            {
                foreach (StockCardEntry transaction in transactions)
                    Insert(transaction);
                return true;
            }
            else
            {
                Debug.WriteLine("ERROR: RETRIVAL LIST CONFIRMATION TRANSACTION FAILED");
                Debug.WriteLine("RL-ID: " + RL.ID);
                return false;
            }
        }

        public bool ProcessAdjustmentVoucher(AdjustmentVoucher AV)
        {
            if (AV == null)
                return false;
            List<DocumentItem> items = AV.DocumentItems?.ToList();
            if (items == null || items.Count == 0)
                return false;

            bool isTransactionOk = true;
            List<StockCardEntry> transactions = new List<StockCardEntry>();
            foreach (DocumentItem item in items)
            {
                if (item.Item == null || item.Qty == 0)
                {
                    isTransactionOk = false;
                    break;
                }
                else
                {
                    transactions.Add(new StockCardEntry(item.Item, item.Qty, AV));
                }
            }
            if (isTransactionOk)
            {
                foreach (StockCardEntry transaction in transactions)
                    Insert(transaction);
                return true;
            }
            else
            {
                Debug.WriteLine("ERROR: ADJUSTMENT TRANSACTION FAILED");
                Debug.WriteLine("AV-ID: " + AV.ID);
                return false;
            }
        }
    }
    
}