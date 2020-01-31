using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.DAL;
using System.Diagnostics;


namespace SSIMS.ViewModels
{
    public class InventoryViewModel
    {
        public List<InventoryItem> inventoryItems { get; }
        public bool lowStock {get;}
        public InventoryViewModel(string searchString, bool low)
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            lowStock = low;
            var items = unitOfWork.InventoryItemRepository.Get(includeProperties:"Item");
             if (!String.IsNullOrEmpty(searchString))
             {
                 items = items.Where(
                     i => i.Item.Description.ToUpper().Contains(searchString.ToUpper())
                     || i.Item.Category.ToUpper().Contains(searchString.ToUpper())
                     || i.Item.ID.ToUpper().Contains(searchString.ToUpper()));
             }

            if (low)
            {
                Debug.WriteLine("Low Stock Filter");
                items = items.Where(
                    i => i.InStoreQty < i.ReorderLvl
                    );
            }
            inventoryItems = (List<InventoryItem>)items.ToList();
        }

    }
}