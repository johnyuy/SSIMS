using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Person
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }

        public Person()
        {
        }

        public Person(string Name, string PhoneNumber, string FaxNumber, string Email)
        {
            name = Name;
            phoneNumber = PhoneNumber;
            faxNumber = FaxNumber;
            email = Email;
        }
    }
}