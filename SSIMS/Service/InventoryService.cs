using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.ViewModels;
using SSIMS.Models;
using SSIMS.DAL;
namespace SSIMS.Service
{
    public class InventoryService
    {
        UnitOfWork uow = new UnitOfWork();
        public static int GetItemIndexFromSearchList(int inventoryItemID, InventoryViewModel inventoryViewModel)
        {
            if (inventoryViewModel == null)
                return 1;

            List<InventoryItem> list = inventoryViewModel.inventoryItems;
            if (list.Count == 0)
                return 1;

            for (int i = 0; i < list.Count; i++)
            {
                if (inventoryItemID == list[i].ID)
                    return i + 1;
            }
            return 1;
        }

    }
}