using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SSIMS.Models
{
    public class Person
    {
        public string name { get; set; }

        public string phoneNumber { get; set; }

        public string faxNumber { get; set; }

        public string email { get; set; }
    }

}

