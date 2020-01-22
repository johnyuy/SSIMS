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
            InitDepartments(context);
            InitItems(context);
            InitSuppliers(context);
            context.Staffs.Add(new Staff("Frank Liu","98765432","frank@ocbc.com",1,"dept head"));
            context.Database.ExecuteSqlCommand(" SetIdentitySeed Staffs,10001 ");
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

            context.SaveChanges();
        }

        static void InitDepartments(DatabaseContext context)
        {
            List<Department> departments = new List<Department>
            {
                new Department("ARCH", "Architecture"),
                new Department("ARTS", "Arts"),
                new Department("COMM", "Commerce"),
                new Department("CPSC", "Computer Science"),
                new Department("ENGG", "Engineering"),
                new Department("ENGL", "English"),
                new Department("MEDI", "Medicine"),
                new Department("REGR", "Registrar"),
                new Department("SCIE", "Science"),
                new Department("ZOOL", "Zoology"),
            };
            foreach (Department dept in departments)
                context.Departments.Add(dept);
            context.SaveChanges();
        }
        
        static void InitItems(DatabaseContext context)
        {
            List<Item> items = new List<Item>
            {
                new Item("C001","Clip","Clips Double 1\"","Dozen"),
                new Item("C002","Clip","Clips Double 2\"","Dozen"),
                new Item("C003","Clip","Clips Double 3/4\"","Dozen"),
                new Item("C004","Clip","Clips Paper Large","Box"),
                new Item("C005","Clip","Clips Paper Medium","Box"),
                new Item("C006","Clip","Clips Paper Small","Box"),
                new Item("E001","Envelope","Envelope Brown (3\"x6\")","Each"),
                new Item("E002","Envelope","Envelope Brown (3\"x6\") w/Window","Each"),
                new Item("E003","Envelope","Envelope Brown (5\"x7\")","Each"),
                new Item("E004","Envelope","Envelope Brown (5\"x7\") w/Window","Each"),
                new Item("E005","Envelope","Envelope White (3\"x6\")","Each"),
                new Item("E006","Envelope","Envelope White (3\"x6\") w/Window","Each"),
                new Item("E007","Envelope","Envelope White (5\"x7\")","Each"),
                new Item("E008","Envelope","Envelope White (5\"x7\") w/Window","Each"),
                new Item("E020","Eraser","Eraser (hard)","Each"),
                new Item("E021","Eraser","Eraser (soft)","Each"),
                new Item("E030","Exercise","Exercise Book (100 pg)","Each"),
                new Item("E031","Exercise","Exercise Book (120 pg)","Each"),
                new Item("E032","Exercise","Exercise Book A4 Hardcover (100 pg)","Each"),
                new Item("E033","Exercise","Exercise Book A4 Hardcover (120 pg)","Each"),
                new Item("E034","Exercise","Exercise Book A4 Hardcover (200 pg)","Each"),
                new Item("E035","Exercise","Exercise Book Hardcover (100 pg)","Each"),
                new Item("E036","Exercise","Exercise Book Hardcover (120 pg)","Each"),
                new Item("F020","File","File Separator","Set"),
                new Item("F021","File","File-Blue Plain","Each"),
                new Item("F022","File","File-Blue with Logo","Each"),
                new Item("F023","File","File-Brown w/o Logo","Each"),
                new Item("F024","File","File-Brown with Logo","Each"),
                new Item("F031","File","Folder Plastic Blue","Each"),
                new Item("F032","File","Folder Plastic Clear","Each"),
                new Item("F033","File","Folder Plastic Green","Each"),
                new Item("F034","File","Folder Plastic Pink","Each"),
                new Item("F035","File","Folder Plastic Yellow","Each"),
                new Item("H011","Pen","Highlighter Blue","Box"),
                new Item("H012","Pen","Highlighter Green","Box"),
                new Item("H013","Pen","Highlighter Pink","Box"),
                new Item("H014","Pen","Highlighter Yellow","Box"),
                new Item("H031","Puncher","Hole Puncher 2 holes","Each"),
                new Item("H032","Puncher","Hole Puncher 3 holes","Each"),
                new Item("H033","Puncher","Hole Puncher Adjustable","Each"),
                new Item("P010","Pad","Pad Postit Memo 1\"x2\"","Packet"),
                new Item("P011","Pad","Pad Postit Memo 1/2\"x1\"","Packet"),
                new Item("P012","Pad","Pad Postit Memo 1/2\"x2\"","Packet"),
                new Item("P013","Pad","Pad Postit Memo 2\"x3\"","Packet"),
                new Item("P014","Pad","Pad Postit Memo 2\"x4\"","Packet"),
                new Item("P015","Pad","Pad Postit Memo 2\"x4\"","Packet"),
                new Item("P016","Pad","Pad Postit Memo 3/4\"x2\"","Packet"),
                new Item("P020","Paper","Paper Photostat A3","Box"),
                new Item("P021","Paper","Paper Photostat A4","Box"),
                new Item("P030","Pen","Pen Ballpoint Black","Dozen"),
                new Item("P031","Pen","Pen Ballpoint Blue","Dozen"),
                new Item("P032","Pen","Pen Ballpoint Red","Dozen"),
                new Item("P033","Pen","Pen Felt Tip Black","Dozen"),
                new Item("P034","Pen","Pen Felt Tip Blue","Dozen"),
                new Item("P035","Pen","Pen Felt Tip Red","Dozen"),
                new Item("P036","Pen","Pen Transparency Permanent","Packet"),
                new Item("P037","Pen","Pen Transparency Soluble","Packet"),
                new Item("P038","Pen","Pen Whiteboard Marker Black","Box"),
                new Item("P039","Pen","Pen Whiteboard Marker Blue","Box"),
                new Item("P040","Pen","Pen Whiteboard Marker Green","Box"),
                new Item("P041","Pen","Pen Whiteboard Marker Red","Box"),
                new Item("P042","Pen","Pencil 2B","Dozen"),
                new Item("P043","Pen","Pencil 2B With Eraser End","Dozen"),
                new Item("P044","Pen","Pencil 4H","Dozen"),
                new Item("P045","Pen","Pencil B","Dozen"),
                new Item("P046","Pen","Pencil B With Eraser End","Dozen"),
                new Item("R001","Ruler","Ruler 6\"","Dozen"),
                new Item("R002","Ruler","Ruler 12\"","Dozen"),
                new Item("S100","Scissors","Scissors","Each"),
                new Item("S040","Tape","Scotch Tape","Each"),
                new Item("S041","Tape","Scotch Tape Dispenser","Each"),
                new Item("S101","Sharpener","Sharpener","Each"),
                new Item("S010","Shorthand","Shorthand Book (100 pg)","Each"),
                new Item("S011","Shorthand","Shorthand Book (120 pg)","Each"),
                new Item("S012","Shorthand","Shorthand Book (80 pg)","Each"),
                new Item("S020","Stapler","Stapler No.28","Each"),
                new Item("S021","Stapler","Stapler No.36","Each"),
                new Item("S022","Stapler","Stapler No.28","Box"),
                new Item("S023","Stapler","Stapler No.36","Box"),
                new Item("T001","Tacks","Thumb Tacks Large","Box"),
                new Item("T002","Tacks","Thumb Tacks Medium","Box"),
                new Item("T003","Tacks","Thumb Tacks Small","Box"),
                new Item("T020","Tparency","Transparency Blue","Box"),
                new Item("T021","Tparency","Transparency Clear","Box"),
                new Item("T022","Tparency","Transparency Green","Box"),
                new Item("T023","Tparency","Transparency Red","Box"),
                new Item("T024","Tparency","Transparency Reverse Blue","Box"),
                new Item("T025","Tparency","Transparency Cover 3M","Box"),
                new Item("T100","Tray","Trays In/Out","Box"),
            };
            foreach (Item item in items)
                context.Items.Add(item);
            context.SaveChanges();
        }

        static void InitSuppliers(DatabaseContext context)
        {
            //List<Supplier> suppliers = 
        }
    }
}