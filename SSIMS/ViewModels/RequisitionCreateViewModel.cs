using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.ViewModels
{
    public class RequisitionCreateViewModel
    {
        [Display(Name = "Item Number")]
        public string ItemID { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        [Display(Name ="Quantity")]
        public string Quantity { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string SelectedDescription { get; set; }
        public IEnumerable<SelectListItem> Descriptions { get; set; }
    }
}