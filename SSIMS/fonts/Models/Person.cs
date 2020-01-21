using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SSIMS.Models
{
    public class Person
    {
        public Person()
        {
        }

        public Person(string name, string phoneNumber, string faxNumber, string email)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            FaxNumber = faxNumber;
            Email = email;
        }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string FaxNumber { get; set; }

        public string Email { get; set; }


    }

}

