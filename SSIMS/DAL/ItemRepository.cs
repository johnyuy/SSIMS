using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;
using System.Collections;
using System.Web.Mvc;

namespace SSIMS.DAL
{
    public class ItemRepository : GenericRepository<Item>
    {
        public ItemRepository(DatabaseContext context)
            : base(context)
        {
        }



        // you can add methods specific to the class here 

        public int UpdateItemFullStock(int id)
        {
            Console.WriteLine("UpdateItemFullStock");
            return 1;
            //return context.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
        }

        //get category list
        public List<string> getCategoryList()
        {

            var items = Get();
            List<string> CategoryList = new List<string>();

            foreach (Item item in items)
            {
                CategoryList.Add(item.Category); 
            }
            CategoryList = CategoryList.Distinct().ToList();

            return CategoryList;
        }

        //get description list according to the category
        public List<string> getDescriptionList(String category)
        {
            var items = Get(filter: x => x.Category == category);
            List<string> DesList = new List<string>();
            foreach (Item item in items)
            {
                DesList.Add(item.Description);

            }
            return DesList.ToList();
        }

        //get selectlist for dynamic dropdownlist
        public IEnumerable<SelectListItem> GetCategories()
        {
            using (var context = new DatabaseContext())
            {
                List<SelectListItem> countries = context.Items.AsNoTracking()
                    .OrderBy(n => n.Category)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.Category,
                            Text = n.Category
                        }).Distinct().ToList();
                var categorytip = new SelectListItem()
                {
                    Value = null,
                    Text = "--- select category ---"
                };
                countries.Insert(0, categorytip);
                return new SelectList(countries, "Value", "Text");
            }
        }
        //get selectlist for dynamic dropdownlist
        public IEnumerable<SelectListItem> GetDescriptions()
        {
            List<SelectListItem> items = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };
            return items;
        }
        //get selectlist for dynamic dropdownlist
        public IEnumerable<SelectListItem> GetDescriptions(string category)
        {
            if (!String.IsNullOrWhiteSpace(category))
            {
                using (var context = new DatabaseContext())
                {
                    IEnumerable<SelectListItem> items = context.Items.AsNoTracking()
                        .OrderBy(n => n.Description)
                        .Where(n => n.Category == category)
                        .Select(n =>
                           new SelectListItem
                           {
                               Value = n.ID,
                               Text = n.Description
                           }).ToList();
                    return new SelectList(items, "Value", "Text");
                }
            }
            return null;

        }

        //get all distinct descriptions
        public IEnumerable<SelectListItem> GetAllDescriptions()
        {
            using (var context = new DatabaseContext())
            {
                List<SelectListItem> output = context.Items.AsNoTracking()
                    .OrderBy(n => n.Description)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.Description,
                            Text = n.Description
                        }).Distinct().ToList();
                var categorytip = new SelectListItem()
                {
                    Value = null,
                    Text = "---------- select item ----------"
                };
                output.Insert(0, categorytip);
                return new SelectList(output, "Value", "Text");
            }
        }


        //get item object by its description
        public Item GetItembyDescrption(string itemID)
        {
            var item = Get(filter: x => x.ID==itemID).First();
            return (Item)item;

        }

    }
}