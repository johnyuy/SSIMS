using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Item
    {
        [Key]
        public string ID { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string UnitOfMeasure { get; set; }

        public Item()
        {
        }

        public Item(string itemID, string category, string description, string unitOfMeasure)
        {
            ID = itemID;
            Category = category;
            Description = description;
            UnitOfMeasure = unitOfMeasure;
        }
    }
}