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
        public int DocumentID { get; set; }

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
    }
}