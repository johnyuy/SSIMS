using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.ViewModels
{
    public enum Status
    {
        Pending, Approved, Cancelled, Rejected, Completed, InProgress
    }

    public class RequisitionCreateViewModel
    {
        [Display(Name = "Item Number")]
        public string ItemID { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        [Range(1,100)]
        [Display(Name ="Quantity")]
        public int Quantity { get; set; }

        [Display(Name ="Create Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }// one day one requisitionOrder

        public Status Status { get; set; }

       

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