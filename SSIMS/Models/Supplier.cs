using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Supplier
    {
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string GstReg { get; set; }
        public string ContactName { get; set; }

        public Supplier()
        {
        }

        public Supplier(string supplierName, string address, string phoneNumber, string gstReg, string contactName)
        {
            SupplierName = supplierName;
            Address = address;
            PhoneNumber = phoneNumber;
            GstReg = gstReg;
            ContactName = contactName;
        }
    }
}