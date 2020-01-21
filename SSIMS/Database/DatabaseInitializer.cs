using System;
using System.Collections.Generic;

using System.Data.Entity;
using SSIMS.Models;

namespace SSIMS.Database
{
    public class DatabaseInitializer<T> : DropCreateDatabaseAlways<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {

            //Seed data
            InitCollectionPoints(context);
            context.SaveChanges();
            //other initializations copy:    static void Init (DatabaseContext context)

            base.Seed(context);
        }

        static void InitCollectionPoints(DatabaseContext context)
        {
            List<CollectionPoint> collectionPoints = new List<CollectionPoint>
            {
                new CollectionPoint("Stationery Store", DateTime.Parse("9:30 AM")),
                new CollectionPoint("Management School", DateTime.Parse("11:00 AM")),
                new CollectionPoint("Medical School", DateTime.Parse("9:30 AM")),
                new CollectionPoint("Engineering School", DateTime.Parse("11:00 AM")),
                new CollectionPoint("Science School", DateTime.Parse("9:30 AM")),
                new CollectionPoint("University Hospital", DateTime.Parse("11:00 AM"))
            };
            foreach (CollectionPoint cp in collectionPoints)
                context.CollectionPoints.Add(cp);
        }
    }
}