using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SSIMS.Database;
using SSIMS.DAL;

namespace SSIMS.Models
{
    public class Tender
    {
        public int ID { get; set; }
        public virtual Item Item { get; set; }
        public virtual Supplier Supplier { get; set; }
        public double Price { get; set; }

        public Tender()
        {
        }

        public Tender(string itemID, string supplierID, double price, UnitOfWork unitOfWork)
        {
            Item = unitOfWork.ItemRepository.GetByID(itemID);
            Supplier = unitOfWork.SupplierRepository.GetByID(supplierID);
            Price = price;
        }

    }

}