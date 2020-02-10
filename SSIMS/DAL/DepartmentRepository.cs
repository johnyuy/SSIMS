using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.Models;
using SSIMS.Database;
using System.Web.Mvc;

namespace SSIMS.DAL
{
    public class DepartmentRepository : GenericRepository<Department>
    {
        public DepartmentRepository(DatabaseContext context) : base(context)
        {
        }

        // you can add methods specific to the class here 

        public int AdditionalMethod(int id)
        {
            Console.WriteLine("AdditionalMethod");
            return 1;
            //return context.Database.ExecuteSqlCommand("UPDATE Course SET Credits = Credits * {0}", multiplier);
        }

        //get all distinct departments
        public IEnumerable<SelectListItem> GetAllDepartmentIDs()
        {
            using (var context = new DatabaseContext())
            {
                List<SelectListItem> output = context.Departments.AsNoTracking()
                    .Where(n=> n.ID != "STOR")
                    .OrderBy(n => n.ID)
                        .Select(n =>
                        new SelectListItem
                        {
                            Value = n.ID,
                            Text = n.DeptName
                        }).Distinct().ToList();
                var categorytip = new SelectListItem()
                {
                    Value = null,
                    Text = "- select department -"
                };
                output.Insert(0, categorytip);
                return new SelectList(output, "Value", "Text");
            }
        }
    }
}