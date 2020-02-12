using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;
using System.Diagnostics;

namespace SSIMS.DAL
{
    public class TenderRepository : GenericRepository<Tender>
    {

        public TenderRepository(DatabaseContext context) : base(context)
        {
        }

        public double GetItemPriceDefaultSupplier(string itemCode)
        {
            IEnumerable<Tender> tenders = Get(filter: x => x.Item.ID == itemCode, includeProperties: "Item");

            if(tenders==null || tenders.Count() == 0)
                return 0;

            Tender tender = tenders.OrderByDescending(x => x.Price).LastOrDefault();

            return tender.Price;
        }

        public double GetSampleTenderPrice(string itemdesc)
        {
            IEnumerable<Tender> tenders = Get(filter: x => x.Item.Description == itemdesc, includeProperties: "Item");

            if (tenders == null || tenders.Count() == 0)
                return 0;

            Tender tender = tenders.OrderByDescending(x => x.Price).LastOrDefault();

            return tender.Price;
        }

    }
}