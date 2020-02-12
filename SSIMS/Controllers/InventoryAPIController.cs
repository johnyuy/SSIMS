using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SSIMS.Models;
using SSIMS.ViewModels;
using System.Diagnostics;
using SSIMS.DAL;

namespace SSIMS.Controllers
{
    public class InventoryAPIController : ApiController
    {
        UnitOfWork uow = new UnitOfWork();

        [HttpGet]
        public List<ApiInventoryView> Get(string q)
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            List<ApiInventoryView> apiInventories = new List<ApiInventoryView>();

            if(q == "" || q == null || q == "Low Stock")
            {
                if(q =="Low Stock")
                {
                    inventoryItems = uow.InventoryItemRepository.Get(includeProperties: "Item").Where(i => i.InStoreQty < i.ReorderLvl).ToList();
                }
                else
                {
                    inventoryItems = uow.InventoryItemRepository.Get(includeProperties: "Item").ToList();

                }
            }
            else
            {
                inventoryItems = uow.InventoryItemRepository.Get(filter: x => x.Item.Description.ToUpper().Contains(q.ToUpper()) || x.Item.ID.ToUpper().Contains(q.ToUpper()), includeProperties: "Item").ToList();

            }
            foreach (InventoryItem ii in inventoryItems)
            {
                apiInventories.Add(new ApiInventoryView(ii));
            }

            return apiInventories;
        }

        [HttpGet]
        public ApiInventoryDetails GetInventoryDetails(string itemid)
        {
            InventoryItem ii = uow.InventoryItemRepository.Get(filter: x => x.Item.ID == itemid, includeProperties: "Item").FirstOrDefault();
            ApiInventoryDetails apiInventoryDetails = new ApiInventoryDetails(ii, uow);

            return apiInventoryDetails;
        }
    }
}
