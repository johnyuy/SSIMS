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
        public static int GetNextIndexFromSearchList(int currentID, InventoryViewModel inventoryViewModel)
        {
            if (inventoryViewModel == null)
                return 0;
            List<InventoryItem> list = inventoryViewModel.inventoryItems;
            if (list.Count == 0)
                return 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (currentID == list[i].ID && i<list.Count-1)
                    return list[i + 1].ID;
            }
            return 0;
        }
        public static int GetPrevIndexFromSearchList(int currentID, InventoryViewModel inventoryViewModel)
        {
            if (inventoryViewModel == null)
                return 0;
            List<InventoryItem> list = inventoryViewModel.inventoryItems;
            if (list.Count == 0)
                return 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (currentID == list[i].ID && i > 0)
                    return list[i - 1].ID;
            }
            return 0;
        }
    }
}