using SSIMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSIMS.ViewModels
{
    public class DisbursementViewModel
    {
        public List<DisbursementList> deptDL { get; set; }

        [Required]
        [Display(Name = "Collection Point")]
        public List<CollectionPoint> CollectionPoint { get; set; }
       
    }
}