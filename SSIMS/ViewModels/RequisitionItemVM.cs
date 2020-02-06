using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSIMS.Models;
using SSIMS.DAL;

namespace SSIMS.ViewModels
{
    public enum Status
    {
        Pending, Approved, Cancelled, Rejected, Completed, InProgress
    }

    public class RequisitionItemVM
    {
        [Display(Name ="Requisition ID")]
        public int ROID { get; set; }

        [Display(Name ="DocumentItem ID")]
        public Guid DoitemID { get; set; }
     
        [Display(Name = "Item Number")]
        public string ItemID { get; set; }

        [Display(Name = "Unit of Measure")]
        public string UnitOfMeasure { get; set; }

        [Range(1,100)]
        [Display(Name ="Quantity")]
        public int Quantity { get; set; }

        [Display(Name ="Create Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime CreatedDate { get; set; }// one day one requisitionOrder

        public Status Status { get; set; }

        [Display(Name = "Requisition ID")]
        public string displayDescription { get; set; }

       

        [Required]
        [Display(Name = "Category")]
        public string SelectedCategory { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string SelectedDescription { get; set; }
        public IEnumerable<SelectListItem> Descriptions { get; set; }

        public List<DocumentItem> DocumentItems { get; set; }


        //for display
        public RequisitionItemVM(DocumentItem di)
        {
            ROID = di.Document.ID;
            Quantity = di.Qty;
            SelectedCategory = di.Item.Category;
            SelectedDescription = di.Item.ID;
            UnitOfMeasure = di.Item.UnitOfMeasure;
            CreatedDate = di.Document.CreatedDate;
        }

        //for new entry with no category
        public RequisitionItemVM()
        {
            UnitOfWork uow = new UnitOfWork();
            Categories = uow.ItemRepository.GetCategories();
            Descriptions = uow.ItemRepository.GetDescriptions();
            SelectedCategory = "";
            SelectedDescription = "";
        }

        //for new entry with catergory specified
        public RequisitionItemVM(string category)
        {
            UnitOfWork uow = new UnitOfWork();
            SelectedCategory = category;
            Descriptions = uow.ItemRepository.GetDescriptions();
        }
    }
}