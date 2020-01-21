using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSIMS.Models
{
    public class Department
    {
        public string DeptId { get; set; }
        public string DeptName { get; set; }
        public virtual Staff DeptRep { get; set; }
        public virtual Staff DeptHead { get; set; }
        public virtual CollectionPoint CollectionPoint { get; set; }
        public Dictionary<Item,Integer> ConsolidatedReqMap { get; set; }
        public List<Item> Shortfall { get; set; }

        public Department()
        {
        }

        public Department(string DeptId, string DeptName, Staff DeptRep, Staff DeptHead, CollectionPoint CollectionPoint,Dictionary<Item,Integer> ConsolidatedReqMap,List<Item> Shortfall)
        {
            deptId = DeptId;
            deptName = DeptName;
            deptRep = DeptRep;
            deptHead = DeptHead;
            collectionPoint = CollectionPoint;
            consolidatedReqMap = ConsolidatedReqMap;
            shortfall = Shortfall;
        }
    }
}