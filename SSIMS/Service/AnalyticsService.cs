using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSIMS.ViewModels;
using SSIMS.Models;
using SSIMS.DAL;
using System.Web.Mvc;
using System.Diagnostics;

namespace SSIMS.Service
{
    public class AnalyticsService
    {
        UnitOfWork uow = new UnitOfWork();
        
        



        public static List<AnalyticsDetailsVM> ApplyFilter(List<AnalyticsDetailsVM> data, string filter, string value)
        {
            if (data == null || data.Count == 0) return null;
            if (filter == "" || value == "") return data;

            List<AnalyticsDetailsVM> output = null;
            if (filter == "Department") { output = FilterByDepartment(data, value); }
            else if (filter == "Category") { output = FilterByCategory(data, value); }
            else if (filter == "Item") { output = FilterByItem(data, value); }
            else if (filter == "Staff") { output = FilterByStaff(data, value); }
            else if (filter == "Month") {
                if (int.TryParse(value, out int month))
                    output = FilterByMonth(data, month);
                else
                    return null;
            }
            else if (filter == "Year")
            {
                if (int.TryParse(value, out int year))
                    output = FilterByYear(data, year);
                else
                    return null;
            }

            return output ?? null;
        }
        public static List<AnalyticsDetailsVM> ApplyGroup(List<AnalyticsDetailsVM> data, string group)
        {
            if (data == null || data.Count == 0) return null;
            if (group == "") return data;
            List<AnalyticsDetailsVM> output = null;
            if(group == "Department") { output = GroupByDepartment(data); }
            else if(group == "Category") { output = GroupByCategory(data); }
            else if(group == "Item") { output = GroupByItem(data); }
            else if(group == "Staff") { output = GroupByStaff(data); }
            else if(group == "Month") { output = GroupByMonth(data); }
            else if(group == "Year") { output = GroupByYear(data); }


            return output ?? null; ;
        }
        //For numerical fields like qty, count or cost
        public static ArrayList XAxis(List<AnalyticsDetailsVM> data, string group)
        {
            if (data == null || data.Count() == 0 || group=="") return null;
            ArrayList output = new ArrayList();
            foreach(AnalyticsDetailsVM item in data)
                output.Add(item.GetType().GetProperty(group).GetValue(item, null));
            return output.Count > 0 ? output : null;
        }

        public static ArrayList YAxisCost(List<AnalyticsDetailsVM> data)
        {
            if (data == null || data.Count() == 0) return null;
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM item in data)
                output.Add(item.Cost);
            return output.Count > 0 ? output : null;
        }

        public static ArrayList YAxisQty(List<AnalyticsDetailsVM> data)
        {
            if (data == null || data.Count() == 0) return null;
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM item in data)
                output.Add(item.Qty);
            return output.Count > 0 ? output : null;
        }

        public static ArrayList YAxisCount(List<AnalyticsDetailsVM> data)
        {
            if (data == null || data.Count() == 0) return null;
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM item in data)
                output.Add(item.Count);
            return output.Count > 0 ? output : null;
        }

        //FILTER Methods used first before GROUP Methods

        public static List<AnalyticsDetailsVM> FilterByDepartment(List<AnalyticsDetailsVM> list, string deptID)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Department == deptID).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByCategory(List<AnalyticsDetailsVM> list, string category)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Category == category).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByMonth(List<AnalyticsDetailsVM> list, int month)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Month == month).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByYear(List<AnalyticsDetailsVM> list, int year)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Year == year).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByItem(List<AnalyticsDetailsVM> list, string itemcode)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Item == itemcode).ToList();
            return result ?? null;
        }

        public static List<AnalyticsDetailsVM> FilterByStaff(List<AnalyticsDetailsVM> list, string orderstaff)
        {
            if (list == null || list.Count == 0)
                return null;

            List<AnalyticsDetailsVM> result = list.Where(x => x.Staff == orderstaff).ToList();
            return result ?? null;
        }

        //GROUP Methods are Aggregate Functions
        public static List<AnalyticsDetailsVM> GroupByCategory(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Category)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Category = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty requested per category
                    Count = cl.Sum(c=> c.Count),    //Number of times an item from this category requested (w/o qty)
                    Cost = cl.Sum(c=> c.Cost),      //Cost of all items requested per category
                }).ToList();

            return result??null;
        }

        public static List<AnalyticsDetailsVM> GroupByDepartment(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Department)
                .Select(cl => new AnalyticsDetailsVM
                {  
                    Department = cl.Key,
                    Qty = cl.Sum(c=>c.Qty),         //total qty of items ordered per department
                    Count = cl.Sum(c=> c.Count),    //Number of item requests made per department (w/o qty)
                    Cost = cl.Sum(c=>c.Cost)        //Total default tender chargeback per department
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByMonth(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Month)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Month = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of items ordered per month (need to check year filter)
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per month (w/o qty)
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per month (need to check year filter)
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByYear(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Year)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Year = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of items ordered per year
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per year (w/o qty)
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per year
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByItem(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Item)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Item = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of per itemcode
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per itemcode
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per item
                }).ToList();

            return result;
        }

        public static List<AnalyticsDetailsVM> GroupByStaff(List<AnalyticsDetailsVM> list)
        {
            if (list == null || list.Count == 0)
                return null;
            List<AnalyticsDetailsVM> result = list
                .GroupBy(l => l.Staff)
                .Select(cl => new AnalyticsDetailsVM
                {
                    Staff = cl.Key,
                    Qty = cl.Sum(c => c.Qty),       //total qty of items ordered per staff
                    Count = cl.Sum(c => c.Count),   //Number of item requests made per staff
                    Cost = cl.Sum(c => c.Cost)      //Total default tender chargeback per staff
                }).ToList();

            return result;
        }

        public static IEnumerable<SelectListItem> GetFilter2List(string filter1){
            List<SelectListItem> output = new List<SelectListItem>();
            
            output.Add(new SelectListItem { Value = "Category", Text = "Category" }); //1
            output.Add(new SelectListItem { Value = "Department", Text = "Department" }); //2
            output.Add(new SelectListItem { Value = "Year", Text = "Year" }); //3
            output.Add(new SelectListItem { Value = "Month", Text = "Month" }); //4
            output.Add(new SelectListItem { Value = "Item", Text = "Item" }); //5
            output.Add(new SelectListItem { Value = "Staff", Text = "Staff" }); //6
            if (filter1 == "Category" || filter1 == "Item") { output.RemoveAt(0); output.RemoveAt(3); }
            else if (filter1 == "Department" || filter1 == "Staff") { output.RemoveAt(1); output.RemoveAt(4); }
            else if (filter1 == "Year") { output.RemoveAt(2); }
            else if (filter1 == "Month") { output.RemoveAt(3); }
            return new SelectList(output, "Value", "Text");
        }
        public static IEnumerable<SelectListItem> GetFilterValues(string filter1)
        {
            UnitOfWork uow = new UnitOfWork();
            List<SelectListItem> output = new List<SelectListItem>();
            if (filter1 == "Category") { output = uow.ItemRepository.GetCategories().ToList(); }
            else if (filter1 == "Item") { output = uow.ItemRepository.GetAllDescriptions().ToList(); }
            else if (filter1 == "Year") { output = GetYearValues().ToList(); }
            else if (filter1 == "Month") { output = GetMonthValues().ToList(); }
            else if (filter1 == "Department") { output = uow.DepartmentRepository.GetAllDepartmentIDs().ToList(); }
            else if (filter1 == "Staff") { output = uow.StaffRepository.GetAllStaffNames().ToList(); }

            return output;
        }
        public static IEnumerable<SelectListItem> GetYearValues()
        {
            List<SelectListItem> output = new List<SelectListItem>();
            int startyear = 2018;
            int years = DateTime.Now.Year - startyear;
            for(int i = 0; i <= years; i++)
            {
                output.Add(new SelectListItem
                {
                    Value = (DateTime.Now.Year - i).ToString(),
                    Text = (DateTime.Now.Year - i).ToString()
                });
            }
            var categorytip = new SelectListItem()
            {
                Value = null,
                Text = "- select year -"
            };
            output.Insert(0, categorytip);
            return new SelectList(output, "Value", "Text");
        }
        public static IEnumerable<SelectListItem> GetMonthValues()
        {
            List<SelectListItem> output = new List<SelectListItem>();

            for (int i = 1; i <= 12; i++)
            {
                string month ="";
                if(i == 1) { month = "January"; }
                if(i == 2) { month = "February"; }
                if(i == 3) { month = "March"; }
                if(i == 4) { month = "April"; }
                if(i == 5) { month = "May"; }
                if(i == 6) { month = "June"; }
                if(i == 7) { month = "July"; }
                if(i == 8) { month = "August"; }
                if(i == 9) { month = "September"; }
                if(i == 10) { month = "October"; }
                if(i == 11) { month = "November"; }
                if(i == 12) { month = "December"; }
                output.Add(new SelectListItem
                {
                    Value = i.ToString(),
                    Text =  month
                });
            }
            var categorytip = new SelectListItem()
            {
                Value = null,
                Text = "- select month -"
            };
            output.Insert(0, categorytip);
            return new SelectList(output, "Value", "Text");
        }

        public static ArrayList QtyCategoryChart1X(List<AnalyticsDetailsVM> SummaryList)
        {

            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM rvsm in SummaryList)
            {
                output.Add(rvsm.Category);
            }
            if (output.Count > 0)
                return output;

            return null;
        }
        public static ArrayList QtyCategoryChart1Y(List<AnalyticsDetailsVM> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM rvsm in SummaryList)
            {
                output.Add(rvsm.Qty);
            }
            if (output.Count > 0)
                return output;

            return null;
        }
        public static ArrayList QtyCategoryChart2X(List<AnalyticsDetailsVM> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM rvsm in SummaryList)
            {
                output.Add(rvsm.Department);
            }
            if (output.Count > 0)
                return output;

            return null;
        }
        public static ArrayList QtyCategoryChart2Y(List<AnalyticsDetailsVM> SummaryList)
        {
            ArrayList output = new ArrayList();
            foreach (AnalyticsDetailsVM rvsm in SummaryList)
            {
                output.Add(rvsm.Count);
            }
            if (output.Count > 0)
                return output;

            return null;
        }



        public static List<AnalyticsDetailsVM> GetAnalyticsSampleData()
        {
            UnitOfWork uow = new UnitOfWork();
            List<AnalyticsDetailsVM> s = new List<AnalyticsDetailsVM>();
            s.Add(new AnalyticsDetailsVM(6, "File", "01-01-2018", "CPSC", "File-Blue with Logo", "Mr. Week Kian Fatt", uow));
            s.Add(new AnalyticsDetailsVM(6, "Tape", "01-03-2019", "REGR", "Scotch Tape Dispenser", "Mr. Tommy Lee Johnson", uow));
            s.Add(new AnalyticsDetailsVM(14, "Tparency", "01-04-2019", "COMM", "Transparency Reverse Blue", "Mr. Jones Fong", uow));
            s.Add(new AnalyticsDetailsVM(15, "Envelope", "01-05-2019", "COMM", "Envelope White (3\"x6\")", "Ms. Rebecca Hong", uow));
            s.Add(new AnalyticsDetailsVM(13, "Eraser", "01-06-2018", "CPSC", "Eraser (soft)", "Mr. Andrew Lee", uow));
            s.Add(new AnalyticsDetailsVM(21, "Tparency", "01-06-2018", "ENGG", "Transparency Green", "Ms. Sandra Cooper", uow));
            s.Add(new AnalyticsDetailsVM(1, "Clip", "01-07-2019", "ENGL", "Clips Double 1", "Ms. May Tan", uow));
            s.Add(new AnalyticsDetailsVM(21, "Pen", "01-11-2019", "ZOOL", "Pen Transparency Permanent", "Mr. Peter Tan Ah Meng", uow));
            s.Add(new AnalyticsDetailsVM(7, "Pen", "02-03-2019", "ARCH", "Highlighter Green", "Ms. Sakura Shinji", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pen", "02-04-2019", "CPSC", "Pen Ballpoint Blue", "Mr. Andrew Lee", uow));
            s.Add(new AnalyticsDetailsVM(4, "Pen", "02-05-2019", "MEDI", "Pencil B", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(5, "Clip", "02-06-2018", "CPSC", "Clips Double 1", "Ms. Temari Ang", uow));
            s.Add(new AnalyticsDetailsVM(7, "Exercise", "02-10-2019", "ENGL", "Exercise Book Hardcover (120 pg)", "Ms. May Tan", uow));
            s.Add(new AnalyticsDetailsVM(6, "Scissors", "03-01-2018", "ENGL", "Scissors", "Mr. Jacob Duke", uow));
            s.Add(new AnalyticsDetailsVM(15, "Tape", "03-01-2019", "REGR", "Scotch Tape Dispenser", "Mr. Tommy Lee Johnson", uow));
            s.Add(new AnalyticsDetailsVM(19, "Envelope", "03-02-2018", "CPSC", "Envelope Brown (5\"x7\") w/Window", "Mr. Kaung Kyaw", uow));
            s.Add(new AnalyticsDetailsVM(10, "Clip", "03-04-2018", "ARCH", "Clips Double 1", "Mr. Ye Yint Hein", uow));
            s.Add(new AnalyticsDetailsVM(3, "Stapler", "03-05-2018", "REGR", "Stapler No.36", "Mr. Ngoc Thuy", uow));
            s.Add(new AnalyticsDetailsVM(10, "Pen", "03-09-2018", "ARTS", "Pencil 2B", "Ms. Kabuto Sumiya", uow));
            s.Add(new AnalyticsDetailsVM(11, "Exercise", "03-09-2019", "REGR", "Exercise Book Hardcover (120 pg)", "Mr. Toni Than", uow));
            s.Add(new AnalyticsDetailsVM(12, "Clip", "03-10-2018", "ZOOL", "Clips Double 1", "Ms. Jiang Huang", uow));
            s.Add(new AnalyticsDetailsVM(3, "Shorthand", "04-02-2018", "REGR", "Shorthand Book (100 pg)", "Mr. Ngoc Thuy", uow));
            s.Add(new AnalyticsDetailsVM(3, "Envelope", "04-02-2019", "ZOOL", "Envelope White (5\"x7\") w/Window", "Mr. Peter Tan Ah Meng", uow));
            s.Add(new AnalyticsDetailsVM(10, "Clip", "04-03-2018", "ARTS", "Clips Double 1", "Ms. Sumi Ko", uow));
            s.Add(new AnalyticsDetailsVM(2, "Exercise", "04-04-2018", "ENGL", "Exercise Book Hardcover (100 pg)", "Mr. Jacob Duke", uow));
            s.Add(new AnalyticsDetailsVM(3, "Clip", "04-05-2019", "ARCH", "Clips Double 1", "Ms. Amanda Ceb", uow));
            s.Add(new AnalyticsDetailsVM(14, "Eraser", "04-06-2019", "ARTS", "Eraser (hard)", "Ms. Kabuto Sumiya", uow));
            s.Add(new AnalyticsDetailsVM(13, "Ruler", "04-07-2018", "MEDI", "Ruler 6", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(19, "Tacks", "04-10-2018", "ARTS", "Thumb Tacks Large", "Mr. Timmy Pong", uow));
            s.Add(new AnalyticsDetailsVM(17, "Shorthand", "04-12-2019", "ZOOL", "Shorthand Book (100 pg)", "Mr. Donald Rumsfield", uow));
            s.Add(new AnalyticsDetailsVM(20, "Exercise", "05-01-2018", "ENGG", "Exercise Book (120 pg)", "Mr. Ron Kent", uow));
            s.Add(new AnalyticsDetailsVM(20, "Tape", "05-02-2018", "COMM", "Scotch Tape", "Ms. Tammy Berth", uow));
            s.Add(new AnalyticsDetailsVM(20, "Pad", "05-03-2018", "COMM", "Pad Postit Memo 1\"x2\"", "Mr. John Tang", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pad", "05-11-2018", "REGR", "Pad Postit Memo 1\"x2\"", "Mr. Tommy Lee Johnson", uow));
            s.Add(new AnalyticsDetailsVM(3, "Tparency", "05-11-2019", "COMM", "Transparency Red", "Mr. John Tang", uow));
            s.Add(new AnalyticsDetailsVM(14, "Pen", "06-07-2018", "MEDI", "Pen Felt Tip Red", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(7, "Paper", "06-09-2018", "MEDI", "Paper Photostat A4", "Ms. Andrea Hei", uow));
            s.Add(new AnalyticsDetailsVM(18, "Envelope", "06-11-2019", "SCIE", "Envelope Brown (3\"x6\") w/Window", "Mr. Stanley Presley", uow));
            s.Add(new AnalyticsDetailsVM(2, "Ruler", "07-06-2018", "ENGG", "Ruler 6", "Ms. Jennifer Bullock", uow));
            s.Add(new AnalyticsDetailsVM(15, "Envelope", "07-07-2018", "ARTS", "Envelope White (3\"x6\") w/Window", "Mr. Kendrik Carlo", uow));
            s.Add(new AnalyticsDetailsVM(10, "Pen", "07-08-2019", "ZOOL", "Pen Whiteboard Marker Blue", "Mr. Lee Ming Xiang", uow));
            s.Add(new AnalyticsDetailsVM(1, "File", "07-08-2019", "ARCH", "File-Blue Plain", "Mr. Wan Lau En", uow));
            s.Add(new AnalyticsDetailsVM(19, "Pad", "07-09-2018", "REGR", "Pad Postit Memo 1/2\"x2\"", "Ms. Helen Ho", uow));
            s.Add(new AnalyticsDetailsVM(16, "Pen", "07-10-2019", "SCIE", "Pen Ballpoint Black", "Ms. Penny Shelby", uow));
            s.Add(new AnalyticsDetailsVM(20, "Shorthand", "08-05-2018", "CPSC", "Shorthand Book (120 pg)", "Ms. Lina Lim", uow));
            s.Add(new AnalyticsDetailsVM(25, "Tape", "08-08-2018", "REGR", "Scotch Tape Dispenser", "Mr. Ngoc Thuy", uow));
            s.Add(new AnalyticsDetailsVM(8, "File", "08-09-2019", "ZOOL", "Folder Plastic Blue", "Ms. Ching Chen", uow));
            s.Add(new AnalyticsDetailsVM(16, "Envelope", "09-06-2018", "ARTS", "Envelope White (5\"x7\") w/Window", "Mr. Kendrik Carlo", uow));
            s.Add(new AnalyticsDetailsVM(10, "Paper", "10-06-2018", "REGR", "Paper Photostat A3", "Ms. Chan Chen Ni", uow));
            s.Add(new AnalyticsDetailsVM(12, "File", "10-06-2018", "MEDI", "Folder Plastic Green", "Ms. Wendy Loo", uow));
            s.Add(new AnalyticsDetailsVM(14, "Eraser", "10-08-2019", "ARTS", "Eraser (hard)", "Mr. Kendrik Carlo", uow));
            s.Add(new AnalyticsDetailsVM(11, "Pen", "10-09-2018", "REGR", "Pencil B With Eraser End", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(17, "Pad", "10-10-2018", "REGR", "Pad Postit Memo 1/2\"x2\"", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(20, "Clip", "10-10-2019", "ZOOL", "Clips Paper Small", "Mr. Donald Rumsfield", uow));
            s.Add(new AnalyticsDetailsVM(8, "Envelope", "11-01-2018", "COMM", "Envelope Brown (3\"x6\")", "Ms. Summer Tran", uow));
            s.Add(new AnalyticsDetailsVM(9, "Pen", "11-01-2019", "MEDI", "Pen Whiteboard Marker Blue", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(12, "Stapler", "11-03-2018", "CPSC", "Stapler No.28", "Mr. Week Kian Fatt", uow));
            s.Add(new AnalyticsDetailsVM(6, "File", "11-04-2018", "ARCH", "Folder Plastic Yellow", "Mr. Wan Lau En", uow));
            s.Add(new AnalyticsDetailsVM(20, "File", "11-04-2018", "SCIE", "File-Blue Plain", "Ms. Penny Shelby", uow));
            s.Add(new AnalyticsDetailsVM(8, "Tray", "11-05-2018", "CPSC", "Trays In/Out", "Mr. Andrew Lee", uow));
            s.Add(new AnalyticsDetailsVM(5, "Exercise", "11-06-2018", "ARTS", "Exercise Book (120 pg)", "Ms. Yod Pornpattrison", uow));
            s.Add(new AnalyticsDetailsVM(1, "Exercise", "11-08-2019", "SCIE", "Exercise Book (100 pg)", "Ms. Pamela Tan", uow));
            s.Add(new AnalyticsDetailsVM(16, "Stapler", "11-09-2018", "ENGL", "Stapler No.28", "Ms. Pamela Kow", uow));
            s.Add(new AnalyticsDetailsVM(3, "Stapler", "11-10-2019", "REGR", "Stapler No.36", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(5, "Clip", "11-11-2019", "ARTS", "Clips Double 3/4", "Mr. Timmy Pong", uow));
            s.Add(new AnalyticsDetailsVM(13, "Pen", "12-01-2018", "SCIE", "Pen Felt Tip Blue", "Mr. Stanley Presley", uow));
            s.Add(new AnalyticsDetailsVM(5, "Pen", "12-01-2018", "ZOOL", "Pen Ballpoint Red", "Ms. Ching Chen", uow));
            s.Add(new AnalyticsDetailsVM(24, "Clip", "12-06-2019", "CPSC", "Clips Paper Small", "Ms. Lina Lim", uow));
            s.Add(new AnalyticsDetailsVM(14, "Pad", "13-02-2018", "ARTS", "Pad Postit Memo 2\"x3\"", "Mr. Timmy Pong", uow));
            s.Add(new AnalyticsDetailsVM(1, "Clip", "13-10-2018", "ENGL", "Clips Double 1", "Mr. Jacob Duke", uow));
            s.Add(new AnalyticsDetailsVM(18, "Envelope", "13-11-2018", "MEDI", "Envelope Brown (3\"x6\") w/Window", "Ms. Wendy Loo", uow));
            s.Add(new AnalyticsDetailsVM(5, "Envelope", "14-03-2018", "ENGL", "Envelope White (3\"x6\") w/Window", "Mr. Jacob Duke", uow));
            s.Add(new AnalyticsDetailsVM(5, "Pad", "14-09-2018", "SCIE", "Pad Postit Memo 2\"x4\"", "Mr. Thomas Thompson", uow));
            s.Add(new AnalyticsDetailsVM(6, "Tparency", "14-10-2019", "CPSC", "Transparency Green", "Ms. Lina Lim", uow));
            s.Add(new AnalyticsDetailsVM(11, "Envelope", "15-01-2018", "ENGG", "Envelope Brown (5\"x7\")", "Ms. Jennifer Bullock", uow));
            s.Add(new AnalyticsDetailsVM(15, "File", "15-03-2019", "REGR", "Folder Plastic Pink", "Mr. Tommy Lee Johnson", uow));
            s.Add(new AnalyticsDetailsVM(4, "Pad", "15-06-2018", "COMM", "Pad Postit Memo 2\"x4\"", "Mr. Mohd Azman", uow));
            s.Add(new AnalyticsDetailsVM(3, "Pen", "15-07-2018", "ARTS", "Pen Whiteboard Marker Red", "Ms. Sumi Ko", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pen", "15-08-2018", "ARCH", "Highlighter Green", "Mr. Patrick Coy", uow));
            s.Add(new AnalyticsDetailsVM(13, "Envelope", "15-09-2019", "MEDI", "Envelope Brown (5\"x7\")", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(20, "Tacks", "15-12-2018", "ENGG", "Thumb Tacks Small", "Mr. Kim Jung Ho", uow));
            s.Add(new AnalyticsDetailsVM(9, "Pen", "15-12-2018", "COMM", "Pencil B With Eraser End", "Ms. Tammy Berth", uow));
            s.Add(new AnalyticsDetailsVM(19, "Tparency", "16-01-2019", "ARTS", "Transparency Blue", "Mr. Kendrik Carlo", uow));
            s.Add(new AnalyticsDetailsVM(23, "File", "16-06-2019", "ENGG", "File-Brown w/o Logo", "Mr. Michael Angelo", uow));
            s.Add(new AnalyticsDetailsVM(16, "Pad", "16-07-2019", "COMM", "Pad Postit Memo 2\"x3\"", "Mr. Mohd Azman", uow));
            s.Add(new AnalyticsDetailsVM(4, "Pen", "16-09-2019", "ENGG", "Pen Felt Tip Black", "Mr. Kim Jung Ho", uow));
            s.Add(new AnalyticsDetailsVM(10, "Pen", "16-10-2018", "ZOOL", "Pen Felt Tip Black", "Ms. Jiang Huang", uow));
            s.Add(new AnalyticsDetailsVM(18, "Tacks", "16-10-2018", "ENGG", "Thumb Tacks Large", "Ms. Sandra Cooper", uow));
            s.Add(new AnalyticsDetailsVM(20, "Pen", "16-10-2019", "ZOOL", "Pencil 2B With Eraser End", "Mr. Peter Tan Ah Meng", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pen", "16-10-2019", "SCIE", "Pen Whiteboard Marker Green", "Mr. Stanley Presley", uow));
            s.Add(new AnalyticsDetailsVM(24, "Pad", "16-10-2019", "ENGG", "Pad Postit Memo 3/4\"x2\"", "Mr. Michael Angelo", uow));
            s.Add(new AnalyticsDetailsVM(22, "Pad", "17-02-2019", "ARTS", "Pad Postit Memo 1/2\"x2\"", "Mr. Kenny Tim", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pen", "17-03-2019", "REGR", "Pen Felt Tip Black", "Mr. Ngoc Thuy", uow));
            s.Add(new AnalyticsDetailsVM(19, "Pen", "17-05-2018", "REGR", "Pen Ballpoint Red", "Ms. Helen Ho", uow));
            s.Add(new AnalyticsDetailsVM(21, "File", "17-06-2019", "SCIE", "File-Brown w/o Logo", "Mr. Thomas Thompson", uow));
            s.Add(new AnalyticsDetailsVM(10, "Shorthand", "17-06-2019", "REGR", "Shorthand Book (80 pg)", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(12, "Exercise", "18-01-2019", "MEDI", "Exercise Book Hardcover (120 pg)", "Ms. Andrea Hei", uow));
            s.Add(new AnalyticsDetailsVM(22, "Envelope", "18-02-2018", "ARCH", "Envelope White (5\"x7\")", "Ms. Sakura Shinji", uow));
            s.Add(new AnalyticsDetailsVM(7, "Pen", "18-03-2018", "ARTS", "Pen Felt Tip Red", "Mr. Timmy Pong", uow));
            s.Add(new AnalyticsDetailsVM(9, "Pad", "18-07-2019", "ENGG", "Pad Postit Memo 1/2\"x2\"", "Mr. Kim Jung Ho", uow));
            s.Add(new AnalyticsDetailsVM(22, "Pad", "18-10-2019", "ENGG", "Pad Postit Memo 3/4\"x2\"", "Mr. Ron Kent", uow));
            s.Add(new AnalyticsDetailsVM(19, "Exercise", "18-11-2018", "ENGL", "Exercise Book A4 Hardcover (120 pg)", "Ms. Andrea Linux", uow));
            s.Add(new AnalyticsDetailsVM(11, "Pen", "19-01-2018", "SCIE", "Pen Whiteboard Marker Red", "Mr. Thomas Thompson", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pen", "19-03-2018", "CPSC", "Highlighter Pink", "Ms. Temari Ang", uow));
            s.Add(new AnalyticsDetailsVM(16, "Pen", "19-03-2019", "ENGG", "Pencil 2B With Eraser End", "Mr. Michael Angelo", uow));
            s.Add(new AnalyticsDetailsVM(25, "Exercise", "19-04-2018", "ENGG", "Exercise Book Hardcover (120 pg)", "Mr. Michael Angelo", uow));
            s.Add(new AnalyticsDetailsVM(13, "Pen", "19-10-2019", "COMM", "Pen Felt Tip Red", "Ms. Rebecca Hong", uow));
            s.Add(new AnalyticsDetailsVM(19, "Exercise", "19-11-2018", "CPSC", "Exercise Book (120 pg)", "Ms. Lina Lim", uow));
            s.Add(new AnalyticsDetailsVM(20, "Pen", "19-12-2019", "MEDI", "Highlighter Yellow", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(8, "Tacks", "20-02-2018", "REGR", "Thumb Tacks Medium", "Ms. Helen Ho", uow));
            s.Add(new AnalyticsDetailsVM(24, "Pad", "20-06-2018", "CPSC", "Pad Postit Memo 1/2\"x1\"", "Mr. Week Kian Fatt", uow));
            s.Add(new AnalyticsDetailsVM(7, "Ruler", "20-11-2018", "REGR", "Ruler 12", "Mr. Toni Than", uow));
            s.Add(new AnalyticsDetailsVM(16, "Envelope", "21-11-2018", "ENGL", "Envelope White (5\"x7\") w/Window", "Mr. Jacob Duke", uow));
            s.Add(new AnalyticsDetailsVM(8, "Puncher", "21-12-2018", "SCIE", "Hole Puncher 2 holes", "Ms. Penny Shelby", uow));
            s.Add(new AnalyticsDetailsVM(12, "File", "22-03-2019", "SCIE", "Folder Plastic Blue", "Ms. Alice Yu", uow));
            s.Add(new AnalyticsDetailsVM(18, "Clip", "22-04-2018", "REGR", "Clips Double 2", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(20, "File", "22-06-2018", "ARCH", "Folder Plastic Green", "Mr. Timmy Ting", uow));
            s.Add(new AnalyticsDetailsVM(20, "Exercise", "22-07-2019", "ZOOL", "Exercise Book (120 pg)", "Mr. Donald Rumsfield", uow));
            s.Add(new AnalyticsDetailsVM(5, "Pen", "22-08-2018", "REGR", "Pen Whiteboard Marker Green", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(7, "Pen", "22-09-2019", "ENGG", "Pen Whiteboard Marker Blue", "Mr. Michael Angelo", uow));
            s.Add(new AnalyticsDetailsVM(19, "File", "23-02-2018", "COMM", "File Separator", "Mr. Jones Fong", uow));
            s.Add(new AnalyticsDetailsVM(14, "Pen", "23-02-2018", "MEDI", "Pencil B With Eraser End", "Mr. Augustus Robinson", uow));
            s.Add(new AnalyticsDetailsVM(16, "Tacks", "23-03-2018", "SCIE", "Thumb Tacks Small", "Ms. Pamela Tan", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pen", "23-06-2019", "COMM", "Pen Ballpoint Black", "Mr. Mohd Azman", uow));
            s.Add(new AnalyticsDetailsVM(15, "Pen", "23-08-2018", "ENGL", "Highlighter Yellow", "Ms. May Tan", uow));
            s.Add(new AnalyticsDetailsVM(3, "Envelope", "23-08-2019", "REGR", "Envelope White (5\"x7\")", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(3, "Pad", "23-10-2018", "REGR", "Pad Postit Memo 1/2\"x1\"", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(21, "Clip", "23-10-2018", "ZOOL", "Clips Paper Small", "Ms. Ching Chen", uow));
            s.Add(new AnalyticsDetailsVM(10, "Sharpener", "23-11-2018", "ENGG", "Sharpener", "Ms. Jennifer Bullock", uow));
            s.Add(new AnalyticsDetailsVM(23, "Pen", "23-11-2019", "CPSC", "Pen Transparency Soluble", "Mr. Dan Shiok", uow));
            s.Add(new AnalyticsDetailsVM(2, "Shorthand", "24-02-2018", "ZOOL", "Shorthand Book (120 pg)", "Mr. Donald Rumsfield", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pen", "24-05-2018", "COMM", "Pencil 4H", "Ms. Rebecca Hong", uow));
            s.Add(new AnalyticsDetailsVM(18, "Puncher", "24-11-2019", "ARTS", "Hole Puncher Adjustable", "Ms. Kabuto Sumiya", uow));
            s.Add(new AnalyticsDetailsVM(9, "Pen", "24-11-2019", "REGR", "Pen Ballpoint Red", "Mr. Toni Than", uow));
            s.Add(new AnalyticsDetailsVM(22, "Pad", "24-11-2019", "ENGL", "Pad Postit Memo 1\"x2\"", "Mr. Jacob Duke", uow));
            s.Add(new AnalyticsDetailsVM(21, "Pen", "25-03-2018", "ZOOL", "Pen Felt Tip Red", "Mr. Peter Tan Ah Meng", uow));
            s.Add(new AnalyticsDetailsVM(18, "Shorthand", "25-03-2019", "ENGG", "Shorthand Book (100 pg)", "Mr. Kim Jung Ho", uow));
            s.Add(new AnalyticsDetailsVM(7, "Pen", "25-08-2019", "ARCH", "Pen Ballpoint Red", "Ms. Amanda Ceb", uow));
            s.Add(new AnalyticsDetailsVM(15, "Pen", "25-09-2019", "MEDI", "Pen Transparency Soluble", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(7, "Ruler", "25-10-2019", "ZOOL", "Ruler 6", "Mr. Donald Rumsfield", uow));
            s.Add(new AnalyticsDetailsVM(10, "Clip", "25-11-2019", "REGR", "Clips Double 1", "Ms. Chan Chen Ni", uow));
            s.Add(new AnalyticsDetailsVM(11, "Envelope", "26-01-2019", "ARCH", "Envelope Brown (3\"x6\")", "Ms. Sakura Shinji", uow));
            s.Add(new AnalyticsDetailsVM(8, "Pen", "26-02-2018", "ENGG", "Pencil 4H", "Mr. Kim Jung Ho", uow));
            s.Add(new AnalyticsDetailsVM(19, "Ruler", "26-03-2018", "ARTS", "Ruler 6", "Mr. Kendrik Carlo", uow));
            s.Add(new AnalyticsDetailsVM(3, "Envelope", "26-04-2019", "COMM", "Envelope Brown (3\"x6\")", "Ms. Summer Tran", uow));
            s.Add(new AnalyticsDetailsVM(10, "Clip", "26-04-2019", "ZOOL", "Clips Paper Small", "Mr. Donald Rumsfield", uow));
            s.Add(new AnalyticsDetailsVM(25, "Tape", "26-05-2019", "COMM", "Scotch Tape", "Ms. Tammy Berth", uow));
            s.Add(new AnalyticsDetailsVM(5, "Tparency", "26-05-2019", "MEDI", "Transparency Cover 3M", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(15, "Pen", "26-07-2019", "REGR", "Pen Ballpoint Black", "Mr. Toni Than", uow));
            s.Add(new AnalyticsDetailsVM(16, "Pad", "26-10-2019", "ENGL", "Pad Postit Memo 2\"x4\"", "Mr. Jacob Duke", uow));
            s.Add(new AnalyticsDetailsVM(17, "Envelope", "27-01-2019", "ZOOL", "Envelope White (3\"x6\")", "Ms. Ching Chen", uow));
            s.Add(new AnalyticsDetailsVM(1, "Pen", "27-02-2018", "COMM", "Pen Felt Tip Black", "Mr. Jones Fong", uow));
            s.Add(new AnalyticsDetailsVM(14, "Pen", "27-08-2018", "ARTS", "Pen Transparency Permanent", "Mr. Timmy Pong", uow));
            s.Add(new AnalyticsDetailsVM(19, "Pen", "27-08-2018", "ZOOL", "Pen Felt Tip Red", "Ms. Jiang Huang", uow));
            s.Add(new AnalyticsDetailsVM(20, "Tparency", "27-08-2019", "ARCH", "Transparency Green", "Mr. Ye Yint Hein", uow));
            s.Add(new AnalyticsDetailsVM(9, "Puncher", "28-03-2019", "ARTS", "Hole Puncher Adjustable", "Ms. Sumi Ko", uow));
            s.Add(new AnalyticsDetailsVM(6, "Pen", "28-04-2019", "ENGL", "Highlighter Yellow", "Ms. Anne Low", uow));
            s.Add(new AnalyticsDetailsVM(8, "Tacks", "28-05-2018", "SCIE", "Thumb Tacks Large", "Mr. Thomas Thompson", uow));
            s.Add(new AnalyticsDetailsVM(18, "Tape", "28-05-2019", "REGR", "Scotch Tape Dispenser", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(10, "Exercise", "28-08-2018", "COMM", "Exercise Book (120 pg)", "Ms. Rebecca Hong", uow));
            s.Add(new AnalyticsDetailsVM(25, "Tape", "28-11-2018", "COMM", "Scotch Tape Dispenser", "Mr. Mohd Azman", uow));
            s.Add(new AnalyticsDetailsVM(17, "Puncher", "28-12-2019", "ENGL", "Hole Puncher 3 holes", "Ms. Pamela Kow", uow));
            s.Add(new AnalyticsDetailsVM(25, "Pen", "29-03-2018", "ENGG", "Pen Felt Tip Red", "Mr. Ron Kent", uow));
            s.Add(new AnalyticsDetailsVM(15, "Shorthand", "29-04-2018", "REGR", "Shorthand Book (120 pg)", "Mr. Tommy Lee Johnson", uow));
            s.Add(new AnalyticsDetailsVM(10, "Clip", "29-04-2018", "MEDI", "Clips Double 1", "Ms. July Moh", uow));
            s.Add(new AnalyticsDetailsVM(19, "Pen", "29-05-2019", "ENGL", "Pen Felt Tip Red", "Ms. Pamela Kow", uow));
            s.Add(new AnalyticsDetailsVM(16, "File", "29-07-2018", "ENGG", "Folder Plastic Blue", "Ms. Jennifer Bullock", uow));
            s.Add(new AnalyticsDetailsVM(4, "Pen", "29-07-2019", "COMM", "Highlighter Yellow", "Ms. Summer Tran", uow));
            s.Add(new AnalyticsDetailsVM(9, "Envelope", "29-07-2019", "REGR", "Envelope Brown (5\"x7\")", "Mr. Ngoc Thuy", uow));
            s.Add(new AnalyticsDetailsVM(24, "Tparency", "29-08-2018", "MEDI", "Transparency Red", "Mr. Augustus Robinson", uow));
            s.Add(new AnalyticsDetailsVM(2, "Pen", "29-08-2018", "ARCH", "Pen Transparency Permanent", "Mr. Ye Yint Hein", uow));
            s.Add(new AnalyticsDetailsVM(25, "Pad", "30-01-2019", "ENGL", "Pad Postit Memo 3/4\"x2\"", "Ms. June Nguyen", uow));
            s.Add(new AnalyticsDetailsVM(10, "File", "30-01-2019", "ARTS", "Folder Plastic Blue", "Ms. Sumi Ko", uow));
            s.Add(new AnalyticsDetailsVM(7, "Envelope", "30-04-2018", "REGR", "Envelope White (5\"x7\")", "Ms. Tra Xiang", uow));
            s.Add(new AnalyticsDetailsVM(5, "Tacks", "30-04-2019", "COMM", "Thumb Tacks Small", "Mr. John Tang", uow));
            s.Add(new AnalyticsDetailsVM(10, "Puncher", "30-05-2018", "MEDI", "Hole Puncher 3 holes", "Mr. Duke Joneson", uow));
            s.Add(new AnalyticsDetailsVM(20, "Pad", "30-06-2018", "ENGG", "Pad Postit Memo 1\"x2\"", "Mr. Michael Angelo", uow));
            s.Add(new AnalyticsDetailsVM(20, "Pen", "30-07-2018", "ZOOL", "Pen Felt Tip Red", "Mr. Peter Tan Ah Meng", uow));
            s.Add(new AnalyticsDetailsVM(9, "File", "30-07-2019", "ENGL", "Folder Plastic Green", "Ms. June Nguyen", uow));
            s.Add(new AnalyticsDetailsVM(14, "File", "30-08-2018", "SCIE", "Folder Plastic Blue", "Ms. Alice Yu", uow));
            s.Add(new AnalyticsDetailsVM(2, "Stapler", "30-11-2018", "MEDI", "Stapler No.36", "Mr. Augustus Robinson", uow));
            s.Add(new AnalyticsDetailsVM(21, "Tparency", "31-01-2018", "CPSC", "Transparency Clear", "Mr. Week Kian Fatt", uow));
            s.Add(new AnalyticsDetailsVM(4, "Pad", "31-01-2019", "SCIE", "Pad Postit Memo 1\"x2\"", "Ms. Pamela Tan", uow));

            return s;
        }
    }
}