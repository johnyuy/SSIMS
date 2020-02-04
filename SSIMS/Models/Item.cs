using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Item
    {
        [Key]
        [DisplayName("Item Code")]
        public string ID { get; set; }
        [DisplayName("Category")]
        public string Category { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        public string ImageURL { get; set; }
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

        public Item(string itemID, string category, string description, string unitOfMeasure, string imageUrl)
        {
            ID = itemID;
            Category = category;
            Description = description;
            UnitOfMeasure = unitOfMeasure;
            ImageURL = imageUrl;
        }


    }
}