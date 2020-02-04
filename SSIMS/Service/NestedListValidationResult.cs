using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SSIMS.Service
{


    public class NestedListValidationResult : ValidationResult
    {
        public NestedListValidationResult() : base("")
        {

        }
        public IList<ValidationResult> NestedResults { get; set; }
    }

}