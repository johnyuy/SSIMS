using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SSIMS.Service
{

    public class NestedListValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            NestedListValidationResult result = new NestedListValidationResult();
            result.ErrorMessage = string.Format(@"Error occured at {0}", validationContext.DisplayName);

            IEnumerable<object> list = new List<object>();
            list = (List<object>)value;
            if (list == null)
            {
                // Single Object
                List<ValidationResult> results = new List<ValidationResult>();
                Validator.TryValidateObject(value, validationContext, results, true);
                result.NestedResults = results;

                return result;
            }


            else
            {
                List<ValidationResult> recursiveResultList = new List<ValidationResult>();

                // List Object
                foreach (var item in list)
                {
                    List<ValidationResult> nestedItemResult = new List<ValidationResult>();
                    ValidationContext context = new ValidationContext(item, validationContext.ServiceContainer, null);

                    NestedListValidationResult nestedParentResult = new NestedListValidationResult();
                    nestedParentResult.ErrorMessage = string.Format(@"Error occured at {0}", validationContext.DisplayName);

                    Validator.TryValidateObject(item, context, nestedItemResult, true);
                    nestedParentResult.NestedResults = nestedItemResult;
                    recursiveResultList.Add(nestedParentResult);
                }

                result.NestedResults = recursiveResultList;
                return result;
            }
        }
    }
}