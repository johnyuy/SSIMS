using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class CollectionPoint
    {
        public string Location { get; set; }
        public DateTime Time { get; set; }

        public CollectionPoint()
        {
        }

        public CollectionPoint(string Location, DateTime Time)
        {
            location = Location;
            time = Time;
        }
    }
}