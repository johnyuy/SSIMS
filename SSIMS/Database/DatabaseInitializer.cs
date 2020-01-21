using System;
using System.Collections.Generic;

using System.Data.Entity;
using SSIMS.Models;

namespace SSIMS.Database
{
    public class DatabaseInitializer<T> : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {

            //Seed data
            InitCollectionPoints(context);
            context.SaveChanges();
            base.Seed(context);
        }

        static void InitCollectionPoints(DatabaseContext context)
        {
            List<CollectionPoint> collectionPoints = new List<CollectionPoint>
            {
                new CollectionPoint("Stationery Store", DateTime.Parse("9:30 AM"))
            };
            Console.WriteLine(DateTime.Now.ToString("T"));
            foreach (CollectionPoint cp in collectionPoints)
                context.CollectionPoints.Add(cp);
        }
    }
}