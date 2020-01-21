using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public enum Status
    {
        Pending, Approved, Rejected, Cancelled, Complete
    }

    public abstract class Document
    {
        public string DocumentID { get; set; }
        public virtual Staff Creator { get; set; }
        public virtual Staff Responder { get; set; }
        public DateTime CreateDate{ get; set; }
        public DateTime reponseDate { get; set; }
        public Status Status { get; set; }

    }
}