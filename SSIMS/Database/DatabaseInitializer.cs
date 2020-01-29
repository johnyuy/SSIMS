using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data.Entity;
using SSIMS.Models;
using Microsoft.EntityFrameworkCore;
using SSIMS.DAL;

namespace SSIMS.Database
{
    public class DatabaseInitializer<T> : DropCreateDatabaseAlways<DatabaseContext>
    {
        static StaffRepository StaffRepository;
        static ItemRepository ItemRepository;
        static DepartmentRepository DepartmentRepository;
        static CollectionPointRepository CollectionPointRepository;
        static RequisitionOrderRepository RequisitionOrderRepository;
        static DocumentItemRepository DocumentItemRepository;

        protected override void Seed(DatabaseContext context)
        {
            Debug.WriteLine("\nSEEDING DATABASE...");
            StaffRepository = new StaffRepository(context);
            ItemRepository = new ItemRepository(context);
            DepartmentRepository = new DepartmentRepository(context);
            CollectionPointRepository = new CollectionPointRepository(context);
            RequisitionOrderRepository = new RequisitionOrderRepository(context);
            DocumentItemRepository = new DocumentItemRepository(context);

            //Seed data
            InitCollectionPoints(context);
            InitDepartments(context);
            InitItems(context);
            InitSuppliers(context);
            InitStaffs(context);
            InitUserAccounts(context);
            InitInventoryItems(context);
            InitDocuments(context);
           
            //other initializations copy:    static void Init (DatabaseContext context)
            context.SaveChanges();
            base.Seed(context);
            Debug.WriteLine("SEEDING COMPLETED!");
        }

        static void InitCollectionPoints(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing CollectionPoints");
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
            Debug.WriteLine("\tInitializing Departments");
            List<Department> departments = new List<Department>
            {
                new Department("ARCH", "Architecture","68901257","68921001", CollectionPointRepository.GetByID(1)),
                new Department("ARTS", "Arts","68901226", "68921011", CollectionPointRepository.GetByID(1)),
                new Department("COMM", "Commerce","68741284","68921256", CollectionPointRepository.GetByID(2)),
                new Department("CPSC", "Computer Science","68901235","68921457", CollectionPointRepository.GetByID(2)),
                new Department("ENGG", "Engineering","68901776","68922395", CollectionPointRepository.GetByID(3)),
                new Department("ENGL", "English","68742234","68921456", CollectionPointRepository.GetByID(3)),
                new Department("MEDI", "Medicine","67848808","68928106", CollectionPointRepository.GetByID(4)),
                new Department("REGR", "Registrar","68901266","68921465", CollectionPointRepository.GetByID(4)),
                new Department("SCIE", "Science","68907191","68921992", CollectionPointRepository.GetByID(5)),
                new Department("ZOOL", "Zoology","68901266","68921465", CollectionPointRepository.GetByID(5)),
                new Department("STOR", "Store","","",null)
            };
            foreach (Department dept in departments)
                context.Departments.Add(dept);
            context.SaveChanges();
        }

        static void InitItems(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing Items");
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
            Debug.WriteLine("\tInitializing Suppliers");
            Supplier supplier1 = new Supplier(
                "ALPHA Office Supplies", "ALPA",
                "Blk 1128, Ang Mo Kio Industrial Park #02-1108 Ang Mo Kio Street 62 Singapore 622262",
                "64619928", "64612238", "MR-8500440-2", "Ms Irene Tan");
            Supplier supplier2 = new Supplier(
                "BANES Shop", "BANE",
                "Blk 124, Alexandra Road #03-04 Banes Building",
                "64781234", "64792434", "MR-8200420-2", "Mr Loh Ah Pek");
            Supplier supplier3 = new Supplier(
                "Cheap Stationer", "CHEP",
                "Blk 34, Clementi Road #07-02 Ban Ban Soh Building Singapore 110525",
                "63543234", "64742434", "nil", "Mr Soh Kway Koh");
            Supplier supplier4 = new Supplier(
                "OMEGA Stationery Supplier", "OMEG",
                "Blk 11, Hillview Avenue #03-04 Singapore 679036",
                "67671233", "67671234", "MR-8555330-1", "Mr Ronnie Ho");

            context.Suppliers.Add(supplier1);
            context.Suppliers.Add(supplier2);
            context.Suppliers.Add(supplier3);
            context.Suppliers.Add(supplier4);
            context.SaveChanges();
        }

        static void InitStaffs(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing Staffs");
            DatabaseCustomizer.SetDefaultID(new Staff("System Admin", "", "ssims@u.logic.edu", "SystemAdmin"), nameof(Staff.ID), 10000, context);
            List<Staff> staffs = new List<Staff>
            {
                //Store Department
                new Staff("Store Manager","", "storemanager@u.logic.edu", DepartmentRepository.GetByID("STOR"), "Manager"),
                new Staff("Store Supervisor", "", "storesupervisor@u.logic.edu", DepartmentRepository.GetByID("STOR"), "Supervisor"),
                new Staff("Store Clerk 1", "", "storeclerk1@u.logic.edu", DepartmentRepository.GetByID("STOR"), "Clerk"),
                new Staff("Store Clerk 2", "", "storeclerk2@u.logic.edu", DepartmentRepository.GetByID("STOR"), "Clerk"),
                new Staff("Store Clerk 3", "", "storeclerk3@u.logic.edu", DepartmentRepository.GetByID("STOR"), "Clerk"),

                //Architecture Department Dept 1
                new Staff("Prof. Kian Bong Kee", "81210001", "kianbongkee@u.logic.edu", DepartmentRepository.GetByID("ARCH"), "DeptHead"),
                new Staff("Mr. Wan Lau En", "91211002","wanlauen@u.logic.edu",DepartmentRepository.GetByID("ARCH"),"DeptRep"),
                new Staff("Mr. Timmy Ting","81213003","timmyting@u.logic.edu",DepartmentRepository.GetByID("ARCH"),"Staff"),
                new Staff("Mr. Patrick Coy","81215005","patrickcoy@u.logic.edu",DepartmentRepository.GetByID("ARCH"),"Staff"),
                new Staff("Mr. Ye Yint Hein", "91214004","yeyinthein@u.logic.edu",DepartmentRepository.GetByID("ARCH"),"Staff"),
                new Staff("Ms. Amanda Ceb","81210601","amandaceb@u.logic.edu",DepartmentRepository.GetByID("ARCH"),"Staff"),
                new Staff("Ms. Sakura Shinji", "91210009","sakurashinji@u.logic.edu",DepartmentRepository.GetByID("ARCH"),"Staff"),

                //Arts Department Dept 2
                new Staff("Prof. Su Myat Mon", "91710001","sumyatmon@u.logic.edu",DepartmentRepository.GetByID("ARTS"),"DeptHead"),
                new Staff("Ms. Sumi Ko","81711002","sumiko@u.logic.edu",DepartmentRepository.GetByID("ARTS"),"DeptRep"),
                new Staff("Mr. Kendrik Carlo", "91713003","kendricarlo@u.logic.edu",DepartmentRepository.GetByID("ARTS"),"Staff"),
                new Staff("Mr. Kenny Tim","81715005","timkenny@u.logic.edu",DepartmentRepository.GetByID("ARTS"),"Staff"),
                new Staff("Mr. Timmy Pong", "91714004","timmypong@u.logic.edu",DepartmentRepository.GetByID("ARTS"),"Staff"),
                new Staff("Ms. Yod Pornpattrison","81710601","yodpattrison@u.logic.edu",DepartmentRepository.GetByID("ARTS"),"Staff"),
                new Staff("Ms. Kabuto Sumiya", "91710009","sumiyakabuto@u.logic.edu",DepartmentRepository.GetByID("ARTS"),"Staff"),

                //Commerce Department Dept 3
                new Staff("Dr. Chiah Leow Bee", "98910001","chiahleowbee@u.logic.edu",DepartmentRepository.GetByID("COMM"),"DeptHead"),
                new Staff("Mr. Mohd Azman","88911002","mohdazman@u.logic.edu",DepartmentRepository.GetByID("COMM"),"DeptRep"),
                new Staff("Ms. Tammy Berth", "98913003","tammyberth@u.logic.edu",DepartmentRepository.GetByID("COMM"),"Staff"),
                new Staff("Ms. Summer Tran", "88915005","summertran@u.logic.edu",DepartmentRepository.GetByID("COMM"),"Staff"),
                new Staff("Mr. John Tang", "98914004","johntang@u.logic.edu",DepartmentRepository.GetByID("COMM"),"Staff"),
                new Staff("Mr. Jones Fong", "88910601","jonesfong@u.logic.edu",DepartmentRepository.GetByID("COMM"),"Staff"),
                new Staff("Ms. Rebecca Hong", "98910009","rebeccahong@u.logic.edu",DepartmentRepository.GetByID("COMM"),"Staff"),

                //Computer Science Department Dept 4
                new Staff("Dr. Soh Kian Wee", "89010001","sohkianwee@u.logic.edu",DepartmentRepository.GetByID("CPSC"),"DeptHead"),
                new Staff("Mr. Week Kian Fatt", "99011002","weekianfat@u.logic.edu",DepartmentRepository.GetByID("CPSC"),"DeptRep"),
                new Staff("Mr. Dan Shiok", "99013003","danshiok@u.logic.edu",DepartmentRepository.GetByID("CPSC"),"Staff"),
                new Staff("Mr. Andrew Lee","89015005","andrewlee@u.logic.edu",DepartmentRepository.GetByID("CPSC"),"Staff"),
                new Staff("Mr. Kaung Kyaw", "99014004","kaungkyaw@u.logic.edu",DepartmentRepository.GetByID("CPSC"),"Staff"),
                new Staff("Ms. Lina Lim", "89010601","linalim@u.logic.edu",DepartmentRepository.GetByID("CPSC"),"Staff"),
                new Staff("Ms. Temari Ang", "99010009","temariang@u.logic.edu",DepartmentRepository.GetByID("CPSC"),"Staff"),

                //Engineering Department Dept 5
                new Staff("Prof. Lucas Liang Tan", "98110001","lucasliangtan@u.logic.edu",DepartmentRepository.GetByID("ENGG"),"DeptHead"),
                new Staff("Mr. Yoshi Nori","88111002","yoshinori@u.logic.edu",DepartmentRepository.GetByID("ENGG"),"DeptRep"),
                new Staff("Mr. Ron Kent", "98113003","ronkent@u.logic.edu",DepartmentRepository.GetByID("ENGG"),"Staff"),
                new Staff("Mr. Kim Jung Ho", "98115005","kimjungho@u.logic.edu",DepartmentRepository.GetByID("ENGG"),"Staff"),
                new Staff("Mr. Michael Angelo","88114004","michaelangelo@u.logic.edu",DepartmentRepository.GetByID("ENGG"),"Staff"),
                new Staff("Ms. Sandra Cooper","88110601","sandracooper@u.logic.edu",DepartmentRepository.GetByID("ENGG"),"Staff"),
                new Staff("Ms. Jennifer Bullock", "98110009","jenniferbullock@u.logic.edu",DepartmentRepository.GetByID("ENGG"),"Staff"),

                //English Department Dept 6
                new Staff("Prof. Ezra Pound", "90010001","ezrapound@u.logic.edu",DepartmentRepository.GetByID("ENGL"),"DeptHead"),
                new Staff("Ms. Pamela Kow","80011002","pamelakow@u.logic.edu",DepartmentRepository.GetByID("ENGL"),"DeptRep"),
                new Staff("Mr. Jacob Duke", "90013003","jacobduke@u.logic.edu",DepartmentRepository.GetByID("ENGL"),"Staff"),
                new Staff("Ms. Andrea Linux","80015005","andrelinux@u.logic.edu",DepartmentRepository.GetByID("ENGL"),"Staff"),
                new Staff("Ms. Anne Low", "90014004","annelow@u.logic.edu",DepartmentRepository.GetByID("ENGL"),"Staff"),
                new Staff("Ms. May Tan","82010601","maytan@u.logic.edu",DepartmentRepository.GetByID("ENGL"),"Staff"),
                new Staff("Ms. June Nguyen", "91010009","junenguyen@u.logic.edu",DepartmentRepository.GetByID("ENGL"),"Staff"),

                //Medicine Department Dept 7
                new Staff("Prof. Russel Jones","81110001","russeljones@u.logic.edu",DepartmentRepository.GetByID("MEDI"),"DeptHead"),
                new Staff("Ms. Kim Chia Lin","81111002","kimchialin@u.logic.edu",DepartmentRepository.GetByID("MEDI"),"DeptRep"),
                new Staff("Mr. Duke Joneson","81113003","dukejoneson@u.logic.edu",DepartmentRepository.GetByID("MEDI"),"Staff"),
                new Staff("Ms. Andrea Hei", "91115005","andreahei@u.logic.edu",DepartmentRepository.GetByID("MEDI"),"Staff"),
                new Staff("Ms. Wendy Loo", "91114004","wendyloo@u.logic.edu",DepartmentRepository.GetByID("MEDI"),"Staff"),
                new Staff("Ms. July Moh", "91110601","julymoh@u.logic.edu",DepartmentRepository.GetByID("MEDI"),"Staff"),
                new Staff("Mr. Augustus Robinson", "91110009","augustusrobinson@u.logic.edu",DepartmentRepository.GetByID("MEDI"),"Staff"),

                //Registrar Department Dept 8
                new Staff("Ms. Low Kway Boo", "95610001","lowkwayboo@u.logic.edu",DepartmentRepository.GetByID("REGR"),"DeptHead"),
                new Staff("Ms. Helen Ho", "95611002","helenho@u.logic.edu",DepartmentRepository.GetByID("REGR"),"DeptRep"),
                new Staff("Mr. Ngoc Thuy", "95613003","ngocthuy@u.logic.edu",DepartmentRepository.GetByID("REGR"),"Staff"),
                new Staff("Ms. Chan Chen Ni", "95615005","chanchenni@u.logic.edu",DepartmentRepository.GetByID("REGR"),"Staff"),
                new Staff("Mr. Tommy Lee Johnson","85614004","tomleejohnson@u.logic.edu",DepartmentRepository.GetByID("REGR"),"Staff"),
                new Staff("Mr. Toni Than","85610601","tonithan@u.logic.edu",DepartmentRepository.GetByID("REGR"),"Staff"),
                new Staff("Ms. Tra Xiang","85610009","traxiang@u.logic.edu",DepartmentRepository.GetByID("REGR"),"Staff"),

                //Science Department Dept 9
                new Staff("Ms. Polly Timberland", "95890001","pollytimberland@u.logic.edu",DepartmentRepository.GetByID("SCIE"),"DeptHead"),
                new Staff("Ms. Penny Shelby", "95891002","pennyshelby@u.logic.edu",DepartmentRepository.GetByID("SCIE"),"DeptRep"),
                new Staff("Mr. Thomas Thompson", "956893003","thomasthompson@u.logic.edu",DepartmentRepository.GetByID("SCIE"),"Staff"),
                new Staff("Ms. Alice Yu", "95895005","aliceyu@u.logic.edu",DepartmentRepository.GetByID("SCIE"),"Staff"),
                new Staff("Mr. Victor Tun", "95894004","victortun@u.logic.edu",DepartmentRepository.GetByID("SCIE"),"Staff"),
                new Staff("Mr. Stanley Presley", "95890601","stanleypresley@u.logic.edu",DepartmentRepository.GetByID("SCIE"),"Staff"),
                new Staff("Ms. Pamela Tan", "95890009","pamelatan@u.logic.edu",DepartmentRepository.GetByID("SCIE"),"Staff"),

                //Zoology Department Dept 10
                new Staff("Prof. Tan", "94810001","proftan@u.logic.edu",DepartmentRepository.GetByID("ZOOL"),"DeptHead"),
                new Staff("Mr. Peter Tan Ah Meng", "94811002","petertan@u.logic.edu",DepartmentRepository.GetByID("ZOOL"),"DeptRep"),
                new Staff("Ms. Ching Chen", "94813003","chingchen@u.logic.edu",DepartmentRepository.GetByID("ZOOL"),"Staff"),
                new Staff("Ms. Natalie Noel", "94815005","noelnatalie@u.logic.edu",DepartmentRepository.GetByID("ZOOL"),"Staff"),
                new Staff("Mr. Donald Rumsfield", "94814004","donaldrumsfield@u.logic.edu",DepartmentRepository.GetByID("ZOOL"),"Staff"),
                new Staff("Mr. Lee Ming Xiang", "94810601","leemingxiang@u.logic.edu",DepartmentRepository.GetByID("ZOOL"),"Staff"),
                new Staff("Ms. Jiang Huang", "94810009","jianghuang@u.logic.edu",DepartmentRepository.GetByID("ZOOL"),"Staff"),

            };
            foreach (Staff staff in staffs)
                context.Staffs.Add(staff);
            context.SaveChanges();
        }

        static void InitUserAccounts(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing UserAccounts");
            Debug.WriteLine("\t\tAdding system admin account");
            context.UserAccounts.Add(new UserAccount("ssims", "admin", 3, 3));
            Debug.WriteLine("\t\tAdding store accounts");
            List<UserAccount> accounts = new List<UserAccount>();
            List<List<string>> namelist = StaffRepository.GetStaffAccountNames();
            foreach (string name in namelist[3])
                accounts.Add(new UserAccount(name, "clerk", 1, 0));
            foreach (string name in namelist[4])
                accounts.Add(new UserAccount(name, "supervisor", 2, 0));
            foreach (string name in namelist[5])
                accounts.Add(new UserAccount(name, "manager", 3, 0));
            Debug.WriteLine("\t\tAdding department staff accounts");
            foreach (string name in namelist[0])
                accounts.Add(new UserAccount(name, "password", 0, 1));
            Debug.WriteLine("\t\tAdding department rep accounts");
            foreach (string name in namelist[1])
                accounts.Add(new UserAccount(name, "password", 0, 2));
            Debug.WriteLine("\t\tAdding department head accounts");
            foreach (string name in namelist[2])
                accounts.Add(new UserAccount(name, "password", 0, 3));

            foreach (UserAccount account in accounts)
                context.UserAccounts.Add(account);
            context.SaveChanges();
        }

        static void InitInventoryItems(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing InventoryItems");
            //setting for how much initial store balance = restock level * multiplier
            int stockLevelMultiplier = 3;

            List<Item> items = (List<Item>)ItemRepository.Get();
            List<InventoryItem> inv = new List<InventoryItem>();

            foreach (Item item in items)
            {
                int reorderLvl = 0, reorderQty = 0;
                if (item.ID.Contains("C00")) { reorderLvl = 50; reorderQty = 30; }
                else if (item.ID.Contains("E00")) { reorderLvl = 600; reorderQty = 400; }
                else if (item.ID.Contains("E02")) { reorderLvl = 50; reorderQty = 20; }
                else if (item.ID.Contains("E03")) { reorderLvl = 100; reorderQty = 50; }
                else if (item.ID.Contains("F02"))
                {
                    if (item.ID == "F020") { reorderLvl = 100; reorderQty = 50; }
                    else if (item.ID == "F021") { reorderLvl = 200; reorderQty = 100; }
                    else if (item.ID == "F022") { reorderLvl = 200; reorderQty = 100; }
                    else { reorderLvl = 200; reorderQty = 150; }
                }
                else if (item.ID.Contains("F03")) { reorderLvl = 200; reorderQty = 150; }
                else if (item.ID.Contains("H01")) { reorderLvl = 100; reorderQty = 80; }
                else if (item.ID.Contains("H03")) { reorderLvl = 50; reorderQty = 20; }
                else if (item.ID.Contains("P01")) { reorderLvl = 100; reorderQty = 60; }
                else if (item.ID.Contains("P02")) { reorderLvl = 500; reorderQty = 500; }
                else if (item.ID.Contains("P03")) { reorderLvl = 100; reorderQty = 50; }
                else if (item.ID.Contains("P04")) { reorderLvl = 100; reorderQty = 50; }
                else if (item.ID.Contains("R00")) { reorderLvl = 50; reorderQty = 20; }
                else if (item.ID.Contains("S01")) { reorderLvl = 100; reorderQty = 80; }
                else if (item.ID.Contains("S02")) { reorderLvl = 50; reorderQty = 20; }
                else if (item.ID.Contains("S04")) { reorderLvl = 50; reorderQty = 20; }
                else if (item.ID.Contains("S10")) { reorderLvl = 50; reorderQty = 20; }
                else if (item.ID.Contains("T00")) { reorderLvl = 10; reorderQty = 10; }
                else if (item.ID.Contains("T02"))
                {
                    if (item.ID == "T021") { reorderLvl = 500; reorderQty = 400; }
                    else if (item.ID == "T025") { reorderLvl = 500; reorderQty = 400; }
                    else { reorderLvl = 100; reorderQty = 200; }
                }
                else if (item.ID.Contains("T10")) { reorderLvl = 20; reorderQty = 10; }
                inv.Add(new InventoryItem(reorderLvl * stockLevelMultiplier, 0, reorderLvl, reorderQty, 0, item));
            }
            foreach (InventoryItem inventoryItem in inv)
                context.InventoryItems.Add(inventoryItem);
            context.SaveChanges();
        }

        static void InitDocuments(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing Documents");
            Staff staff1 = StaffRepository.GetByID(10005);
            RequisitionOrder reqform = new RequisitionOrder(staff1);
            reqform.RepliedByStaff = staff1;
            List<DocumentItem> documentItems = new List<DocumentItem>{
                new DocumentItem(ItemRepository.GetByID("C001"),2),
                new DocumentItem(ItemRepository.GetByID("E031"),5),
                new DocumentItem(ItemRepository.GetByID("P039"),7),
            };
            reqform.DocumentItems = documentItems;
            context.RequisitionForms.Add(reqform);

            Staff staff2 = StaffRepository.GetByID(10010);
            RequisitionOrder reqform2 = new RequisitionOrder(staff2);
            reqform2.RepliedByStaff = StaffRepository.GetByID(10006);
            List<DocumentItem> documentItems2 = new List<DocumentItem>
            {
                new DocumentItem(ItemRepository.GetByID("C001"),10),
                new DocumentItem(ItemRepository.GetByID("H014"),4),
                new DocumentItem(ItemRepository.GetByID("F033"),20),
            };
            reqform2.DocumentItems = documentItems2;
            reqform2.Status = (Status)1;
            context.RequisitionForms.Add(reqform2);

            Staff staff3 = StaffRepository.GetByID(10044);
            RequisitionOrder reqform3 = new RequisitionOrder(staff3);
            reqform2.RepliedByStaff = StaffRepository.GetByID(10041);
            List<DocumentItem> documentItems3 = new List<DocumentItem>
            {
                new DocumentItem(ItemRepository.GetByID("C001"),10),
                new DocumentItem(ItemRepository.GetByID("H014"),5),
                new DocumentItem(ItemRepository.GetByID("F033"),20),
            };
            reqform3.DocumentItems = documentItems3;
            reqform3.Status =(Status)1;
            context.RequisitionForms.Add(reqform3);

            RetrievalList retrievalList = new RetrievalList(staff1, DepartmentRepository.GetByID("ARTS"));
            List<TransactionItem> transactionItems = new List<TransactionItem>
            {
                new TransactionItem(2,2,"Retrieval",ItemRepository.GetByID("C001")),
                new TransactionItem(5,5,"Retrieval",ItemRepository.GetByID("E031")),
                new TransactionItem(7,7,"Retrieval",ItemRepository.GetByID("P039")),
            };
            retrievalList.ItemTransactions = transactionItems;
            context.RetrievalLists.Add(retrievalList);

            DisbursementList disbursementList = new DisbursementList(staff1, DepartmentRepository.GetByID("ARTS"));
            disbursementList.RepliedByStaff = StaffRepository.GetByID(10014);
            List<TransactionItem> disbursedItems = new List<TransactionItem>
            {
                new TransactionItem(2,2,"Disbursement",ItemRepository.GetByID("C001")),
                new TransactionItem(5,5,"Disbursement",ItemRepository.GetByID("E031")),
                new TransactionItem(7,7,"Disbursement",ItemRepository.GetByID("P039")),
            };
            disbursementList.ItemTransactions = disbursedItems;
            context.DisbursementLists.Add(disbursementList);
            context.SaveChanges();
        }

      
    }
}