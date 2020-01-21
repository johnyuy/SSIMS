using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public abstract class Person
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }

        public Person()
        {
        }

        public Person(string name, string phoneNumber, string faxNumber, string email)
        {
            Name = name ;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
            Email = email;
        }
    }
}