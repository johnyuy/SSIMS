using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SSIMS.Models
{
    public enum Status
    {
        Pending , Approved, Cancelled, Rejected, Completed 
    }

    public abstract class Document
    {
        public int DocumentID { get; set; }
        public int CreatorID { get; set; }
        public int ResponderID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ResponseDate { get; set; }
        public Status Status { get; set; }

        [ForeignKey("CreatorID")]
        public Staff Creator { get; set; }

        [ForeignKey("ResponderID")]
        public Staff Responder { get; set; }
    }
}