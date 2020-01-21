using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SSIMS.Models
{

    public class Document
    {
        public int DocumentID { get; set; }
        public int CreatorID { get; set; }
        public int ResponderID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ResponseDate { get; set; }

        [ForeignKey("CreatorID")]
        public Staff Creator { get; set; }

        [ForeignKey("ResponderID")]
        public Staff Responder { get; set; }
    }
}