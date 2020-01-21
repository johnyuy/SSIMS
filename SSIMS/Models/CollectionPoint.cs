using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class CollectionPoint
    {
        public int CollectionPointID { get; set; }
        public string Location { get; set; }
        public DateTime Time { get; set; }

        public CollectionPoint()
        {
        }

        public CollectionPoint(string location, DateTime time)
        {
            Location = location;
            Time = time;
        }
    }
}