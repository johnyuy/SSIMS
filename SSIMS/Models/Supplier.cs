using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Supplier
    {
        public string ID { get; set; } //supplier code
        public string SupplierName { get; set; }
        public string Address { get; set; }
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