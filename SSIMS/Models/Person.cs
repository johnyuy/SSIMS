using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SSIMS.Models
{
    public class Person
    {
        
        string name;
        string phoneNumber;
        string faxNumber;
        string email;

        public Person(string name, string phonenumber, string faxnumber, string email)
        {
            this.name = name;
            phoneNumber = phonenumber;
            faxNumber = faxnumber;
            this.email = email;
        }
    }
}