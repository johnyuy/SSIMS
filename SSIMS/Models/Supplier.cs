using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Supplier
    {
        [Required]
        public string ID { get; set; } //supplier code
        [Required]
        public string SupplierName { get; set; }
        [Required]
        public string Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\+\s?)?((?<!\+.*)\(\+?\d+([\s\-\.]?\d+)?\)|\d+)([\s\-\.]?(\(\d+([\s\-\.]?\d+)?\)|\d+))*(\s?(x|ext\.?)\s?\d+)?$", ErrorMessage = "The PhoneNumber field is not a valid phone number")]
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string GstReg { get; set; }
        public string ContactName { get; set; }

        public Supplier()
        {
        }

        public Supplier(string supplierName, string supplierid, string address, string phoneNumber, string faxNumber, string gstReg, string contactName)
        {
            SupplierName = supplierName;
            ID = supplierid;
            Address = address;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
            GstReg = gstReg;
            ContactName = contactName;
        }
    }
}