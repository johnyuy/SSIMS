using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            //InitSuppliers(context);
            InitStaffs(context);
            
            //other initializations copy:    static void Init (DatabaseContext context)
            context.SaveChanges();
            base.Seed(context);
        }

        static void InitCollectionPoints(DatabaseContext context)
        {
            Debug.WriteLine("Initializing CollectionPoints");
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
            Debug.WriteLine("Initializing Departments");
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
            Debug.WriteLine("Initializing Items");
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
            Debug.WriteLine("Initializing Suppliers");
        }

        static void InitStaffs(DatabaseContext context)
        {
            Debug.WriteLine("Initializing Staffs");
            DatabaseCustomizer.SetDefaultID(new Staff("Prof. Kian Bong Kee", "81210001", "kianbongkee@u.logic.edu", 1, "DeptHead"), nameof(Staff.StaffID), 100001, context);
            List<Staff> staffs = new List<Staff>
            {
                //Architecture Department Dept 1
                new Staff("Mr. Wan Lau Eh", "91211002","wanlaueh@u.logic.edu",1,"DeptRep"),
                new Staff("Mr. Timmy Ting","81213003","timmyting@u.logic.edu",1,"Staff"),
                new Staff("Mr. Patrick Coy","81215005","patrickcoy@u.logic.edu",1,"Staff"),
                new Staff("Mr. Ye Yint Hein", "91214004","yeyinthein@u.logic.edu",1,"Staff"),
                new Staff("Mrs. Amanda Ceb","81210601","amandaceb@u.logic.edu",1,"Staff"),
                new Staff("Mrs. Sakura Shinji", "91210009","sakurashinji@u.logic.edu",1,"Staff"),

                //Arts Department Dept 2
                new Staff("Prof. Su Myat Mon", "91710001","sumyatmon@u.logic.edu",2,"DeptHead"),
                new Staff("Mrs. Sumi Ko","81711002","sumiko@u.logic.edu",2,"DeptRep"),
                new Staff("Mr. Kendrik Carlo", "91713003","kendricarlo@u.logic.edu",2,"Staff"),
                new Staff("Mr. Kenny Tim","81715005","timkenny@u.logic.edu",2,"Staff"),
                new Staff("Mr. Timmy Pong", "91714004","timmypong@u.logic.edu",2,"Staff"),
                new Staff("Mrs. Yod Pornpattrison","81710601","yodpattrison@u.logic.edu",2,"Staff"),
                new Staff("Mrs. Kabuto Sumiya", "91710009","sumiyakabuto@u.logic.edu",2,"Staff"),

                //Commerce Department Dept 3
                new Staff("Dr. Chiah Leow Bee", "98910001","chiahleowbee@u.logic.edu",3,"DeptHead"),
                new Staff("Mr. Mohd Azman","88911002","mohdazman@u.logic.edu",3,"DeptRep"),
                new Staff("Mrs. Tammy Berth", "98913003","tammyberth@u.logic.edu",3,"Staff"),
                new Staff("Mrs. Summer Tran", "88915005","summertran@u.logic.edu",3,"Staff"),
                new Staff("Mr. John Tang", "98914004","johntang@u.logic.edu",3,"Staff"),
                new Staff("Mr. Jones Fong", "88910601","jonesfong@u.logic.edu",3,"Staff"),
                new Staff("Mrs. Rebecca Hong", "98910009","rebeccahong@u.logic.edu",3,"Staff"),

                //Computer Science Department Dept 4
                new Staff("Dr. Soh Kian Wee", "89010001","sohkianwee@u.logic.edu",4,"DeptHead"),
                new Staff("Mr. Week Kian Fatt", "99011002","weekianfat@u.logic.edu",4,"DeptRep"),
                new Staff("Mr. Dan Shiok", "99013003","danshiok@u.logic.edu",4,"Staff"),
                new Staff("Mr. Andrew Lee","89015005","andrewlee@u.logic.edu",4,"Staff"),
                new Staff("Mr. Kaung Kyaw", "99014004","kaungkyaw@u.logic.edu",4,"Staff"),
                new Staff("Mrs. Lina Lim", "89010601","linalim@u.logic.edu",4,"Staff"),
                new Staff("Mrs. Temari Ang", "99010009","temariang@u.logic.edu",4,"Staff"),

                //Engineering Department Dept 5
                new Staff("Prof. Lucas Liang Tan", "98110001","lucasliangtan@u.logic.edu",5,"DeptHead"),
                new Staff("Mr. Yoshi Nori","88111002","yoshinori@u.logic.edu",5,"DeptRep"),
                new Staff("Mr. Ron Kent", "98113003","ronkent@u.logic.edu",5,"Staff"),
                new Staff("Mr. Kim Jung Ho", "98115005","kimjungho@u.logic.edu",5,"Staff"),
                new Staff("Mr. Michael Angelo","88114004","michaelangelo@u.logic.edu",5,"Staff"),
                new Staff("Mrs. Sandra Cooper","88110601","sandracooper@u.logic.edu",5,"Staff"),
                new Staff("Mrs. Jennifer Bullock", "98110009","jenniferbullock@u.logic.edu",5,"Staff"),

                //English Department Dept 6
                new Staff("Prof. Ezra Pound", "90010001","ezrapound@u.logic.edu",6,"DeptHead"),
                new Staff("Mrs. Pamela Kow","80011002","pamelakow@u.logic.edu",6,"DeptRep"),
                new Staff("Mr. Jacob Duke", "90013003","jacobduke@u.logic.edu",6,"Staff"),
                new Staff("Mrs. Andrea Linux","80015005","andrelinux@u.logic.edu",6,"Staff"),
                new Staff("Mrs. Anne Low", "90014004","annelow@u.logic.edu",6,"Staff"),
                new Staff("Mrs. May Tan","82010601","maytan@u.logic.edu",6,"Staff"),
                new Staff("Mrs. June Nguyen", "91010009","junenguyen@u.logic.edu",6,"Staff"),

                //Medicine Department Dept 7
                new Staff("Prof. Russel Jones","81110001","russeljones@u.logic.edu",7,"DeptHead"),
                new Staff("Mrs. Jin Chia Lin","81111002","jinchialin@u.logic.edu",7,"DeptRep"),
                new Staff("Mr. Duke Joneson","81113003","dukejoneson@u.logic.edu",7,"Staff"),
                new Staff("Mrs. Andrea Hei", "91115005","andreahei@u.logic.edu",7,"Staff"),
                new Staff("Mrs. Wendy Loo", "91114004","wendyloo@u.logic.edu",7,"Staff"),
                new Staff("Mrs. July Moh", "91110601","julymoh@u.logic.edu",7,"Staff"),
                new Staff("Mr. Augustus Robinson", "91110009","augustusrobinson@u.logic.edu",7,"Staff"),

                //Registrar Department Dept 8
                new Staff("Mrs. Low Kway Boo", "95610001","lowkwayboo@u.logic.edu",8,"DeptHead"),
                new Staff("Ms. Helen Ho", "95611002","helenho@u.logic.edu",8,"DeptRep"),
                new Staff("Mr. Ngoc Thuy", "95613003","ngocthuy@u.logic.edu",8,"Staff"),
                new Staff("Mrs. Chan Chen", "95615005","chanchen@u.logic.edu",8,"Staff"),
                new Staff("Mr. Tommy Lee Johnson","85614004","tomleejohnson@u.logic.edu",8,"Staff"),
                new Staff("Mr. Toni Than","85610601","tonithan@u.logic.edu",8,"Staff"),
                new Staff("Mrs. Tra Xiang","85610009","traxiang@u.logic.edu",8,"Staff"),

                //Science Department Dept 9
                new Staff("Mrs. Polly Timberland", "95890001","pollytimberland@u.logic.edu",9,"DeptHead"),
                new Staff("Ms. Penny Shelby", "95891002","pennyshelby@u.logic.edu",9,"DeptRep"),
                new Staff("Mr. Thomas Thompson", "956893003","thomasthompson@u.logic.edu",9,"Staff"),
                new Staff("Mrs. Alice Yu", "95895005","aliceyu@u.logic.edu",9,"Staff"),
                new Staff("Mr. Victor Tun", "95894004","victortun@u.logic.edu",9,"Staff"),
                new Staff("Mr. Stanley Presley", "95890601","stanleypresley@u.logic.edu",9,"Staff"),
                new Staff("Mrs. Pan Tan", "95890009","pantan@u.logic.edu",9,"Staff"),

                //Zoology Department Dept 10
                new Staff("Prof. Tan", "94810001","tanzoo67@u.logic.edu",10,"DeptHead"),
                new Staff("Mr. Peter Tan Ah Meng", "94811002","petertan@u.logic.edu",10,"DeptRep"),
                new Staff("Mrs. Ching Chen", "94813003","chingchen@u.logic.edu",10,"Staff"),
                new Staff("Mrs. Natalie Noel", "94815005","noelnatalie@u.logic.edu",10,"Staff"),
                new Staff("Mr. Donald Dom", "94814004","domdonald@u.logic.edu",10,"Staff"),
                new Staff("Mr. Ting Xiong", "94810601","tingxiong@u.logic.edu",10,"Staff"),
                new Staff("Mrs. Jiang Huang", "94810009","jianghuang@u.logic.edu",10,"Staff"),

            };
            foreach (Staff staff in staffs)
                context.Staffs.Add(staff);
            context.SaveChanges();
        }
        
    }
}