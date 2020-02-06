using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SSIMS.Models
{
    public enum Status
    {
        Pending, Approved, Cancelled, Rejected, Completed, InProgress
    }

    public abstract class Document
    {
        [Key]
        [DisplayFormat(DataFormatString ="ID{0:100000}",ApplyFormatInEditMode =true)]
        [Display(Name = "Requisition ID")]
        public int ID { get; set; }

        [ForeignKey("CreatedByStaff")]
        public int? CreatedByStaffID { get; set; }
        [ForeignKey("RepliedByStaff")]
        public int? RepliedByStaffID { get; set; }

        public string Comments { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ResponseDate { get; set; }
        public Status Status { get; set; }

        public virtual Staff CreatedByStaff { get; set; }
        public virtual Staff RepliedByStaff { get; set; }

        protected Document(Staff creator)
        {
            CreatedByStaff = creator;
            RepliedByStaff = null;
            CreatedDate = DateTime.Now;
            ResponseDate = null;
            Status = 0;
        }
        public Document() {
            CreatedDate = DateTime.Now;
        }

        public void Approve(Staff RepliedByStaffID)
        {
            RepliedByStaff = RepliedByStaffID;
            ResponseDate = DateTime.Now;
            Status = Status.Approved;
        }

        public void Cancelled(Staff RepliedByStaffID)
        {
            RepliedByStaff = RepliedByStaffID;
            ResponseDate = DateTime.Now;
            Status = Status.Cancelled;
        }

        public void Rejected(Staff RepliedByStaffID)
        {
            RepliedByStaff = RepliedByStaffID;
            ResponseDate = DateTime.Now;
            Status = Status.Rejected;
        }

        public void Completed(Staff RepliedByStaffID)
        {
            RepliedByStaff = RepliedByStaffID;
            ResponseDate = DateTime.Now;
            Status = Status.Completed;
        }

        public void Completed()
        {
            Status = Status.Completed;
        }

        public void InProgress(Staff RepliedByStaffID)
        {
            RepliedByStaff = RepliedByStaffID;
            ResponseDate = DateTime.Now;
            Status = Status.InProgress;
        }

        public void InProgress()
        {
            Status = Status.InProgress;
        }

        public void InProgress()
        {
            ResponseDate = DateTime.Now;
            Status = Status.InProgress;
        }
    }
}