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
    //public class DatabaseInitializer<T> : CreateDatabaseIfNotExists<DatabaseContext>

    {
        static StaffRepository StaffRepository;
        static ItemRepository ItemRepository;
        static DepartmentRepository DepartmentRepository;
        static CollectionPointRepository CollectionPointRepository;
        static RequisitionOrderRepository RequisitionOrderRepository;
        static DocumentItemRepository DocumentItemRepository;
        static DeptHeadAuthorizationRepository DeptHeadAuthorizationRepository;

        protected override void Seed(DatabaseContext context)
        {
            Debug.WriteLine("\nSEEDING DATABASE...");
            StaffRepository = new StaffRepository(context);
            ItemRepository = new ItemRepository(context);
            DepartmentRepository = new DepartmentRepository(context);
            CollectionPointRepository = new CollectionPointRepository(context);
            RequisitionOrderRepository = new RequisitionOrderRepository(context);
            DocumentItemRepository = new DocumentItemRepository(context);
            DeptHeadAuthorizationRepository = new DeptHeadAuthorizationRepository(context);


            //Seed data
            
            InitCollectionPoints(context);
            InitDepartments(context);
            InitStaffs(context);
            InitItems(context);
            InitSuppliers(context);
            InitTenders(context);
            InitUserAccounts(context);
            InitInventoryItems(context);
            InitDocuments(context);
            InitPurchaseOrders(context);
            InitDeptHeadAuthorizations(context);


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
                new Department("ARCH", "Architecture","68901257","68921001", CollectionPointRepository.GetByID(1),null,null,null),
                new Department("ARTS", "Arts","68901226", "68921011", CollectionPointRepository.GetByID(1),null,null,null),
                new Department("COMM", "Commerce","68741284","68921256", CollectionPointRepository.GetByID(2),null,null,null),
                new Department("CPSC", "Computer Science","68901235","68921457", CollectionPointRepository.GetByID(2),null,null,null),
                new Department("ENGG", "Engineering","68901776","68922395", CollectionPointRepository.GetByID(3),null,null,null),
                new Department("ENGL", "English","68742234","68921456", CollectionPointRepository.GetByID(3),null,null,null),
                new Department("MEDI", "Medicine","67848808","68928106", CollectionPointRepository.GetByID(4),null,null,null),
                new Department("REGR", "Registrar","68901266","68921465", CollectionPointRepository.GetByID(4),null,null,null),
                new Department("SCIE", "Science","68907191","68921992", CollectionPointRepository.GetByID(5),null,null,null),
                new Department("ZOOL", "Zoology","68901266","68921465", CollectionPointRepository.GetByID(5),null,null,null),
                new Department("STOR", "Store","","",null,null,null,null)
            };
            foreach (Department dept in departments)
                context.Departments.Add(dept);
            context.SaveChanges();
        }

        static void InitDepartments2(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing Departments");
            List<Department> departments = new List<Department>
            {
                new Department("ARCH", "Architecture","68901257","68921001", CollectionPointRepository.GetByID(1),DeptHeadAuthorizationRepository.GetByID(1),StaffRepository.GetStaffbyID(10006),StaffRepository.GetStaffbyID(10007)),
                new Department("ARTS", "Arts","68901226", "68921011", CollectionPointRepository.GetByID(1),DeptHeadAuthorizationRepository.GetByID(2),StaffRepository.GetStaffbyID(10013),StaffRepository.GetStaffbyID(10014)),
                new Department("COMM", "Commerce","68741284","68921256", CollectionPointRepository.GetByID(2),DeptHeadAuthorizationRepository.GetByID(3),StaffRepository.GetStaffbyID(10020),StaffRepository.GetStaffbyID(10021)),
                new Department("CPSC", "Computer Science","68901235","68921457", CollectionPointRepository.GetByID(2),DeptHeadAuthorizationRepository.GetByID(4),StaffRepository.GetStaffbyID(10027),StaffRepository.GetStaffbyID(10028)),
                new Department("ENGG", "Engineering","68901776","68922395", CollectionPointRepository.GetByID(3),DeptHeadAuthorizationRepository.GetByID(5),StaffRepository.GetStaffbyID(10034),StaffRepository.GetStaffbyID(10035)),
                new Department("ENGL", "English","68742234","68921456", CollectionPointRepository.GetByID(3),DeptHeadAuthorizationRepository.GetByID(6),StaffRepository.GetStaffbyID(10041),StaffRepository.GetStaffbyID(10042)),
                new Department("MEDI", "Medicine","67848808","68928106", CollectionPointRepository.GetByID(4),DeptHeadAuthorizationRepository.GetByID(7),StaffRepository.GetStaffbyID(10048),StaffRepository.GetStaffbyID(10049)),
                new Department("REGR", "Registrar","68901266","68921465", CollectionPointRepository.GetByID(4),DeptHeadAuthorizationRepository.GetByID(8),StaffRepository.GetStaffbyID(10055),StaffRepository.GetStaffbyID(10056)),
                new Department("SCIE", "Science","68907191","68921992", CollectionPointRepository.GetByID(5),DeptHeadAuthorizationRepository.GetByID(9),StaffRepository.GetStaffbyID(10062),StaffRepository.GetStaffbyID(10063)),
                new Department("ZOOL", "Zoology","68901266","68921465", CollectionPointRepository.GetByID(5),DeptHeadAuthorizationRepository.GetByID(10),StaffRepository.GetStaffbyID(10069),StaffRepository.GetStaffbyID(10070)),
                new Department("STOR", "Store","","",null,null,null,null)
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
                new Item("C001","Clip","Clips Double 1\"","Dozen","~/Resources/Items/clips.png"),
                new Item("C002","Clip","Clips Double 2\"","Dozen","~/Resources/Items/clips.png"),
                new Item("C003","Clip","Clips Double 3/4\"","Dozen","~/Resources/Items/clips.png"),
                new Item("C004","Clip","Clips Paper Large","Box","~/Resources/Items/clips.png"),
                new Item("C005","Clip","Clips Paper Medium","Box","~/Resources/Items/clips.png"),
                new Item("C006","Clip","Clips Paper Small","Box","~/Resources/Items/clips.png"),
                new Item("E001","Envelope","Envelope Brown (3\"x6\")","Each","~/Resources/Items/brownenvelope.png"),
                new Item("E002","Envelope","Envelope Brown (3\"x6\") w/Window","Each","~/Resources/Items/brownenvelope.png"),
                new Item("E003","Envelope","Envelope Brown (5\"x7\")","Each","~/Resources/Items/brownenvelope.png"),
                new Item("E004","Envelope","Envelope Brown (5\"x7\") w/Window","Each","~/Resources/Items/brownenvelope.png"),
                new Item("E005","Envelope","Envelope White (3\"x6\")","Each","~/Resources/Items/whiteenvelope.png"),
                new Item("E006","Envelope","Envelope White (3\"x6\") w/Window","Each","~/Resources/Items/whiteenvelope.png"),
                new Item("E007","Envelope","Envelope White (5\"x7\")","Each","~/Resources/Items/whiteenvelope.png"),
                new Item("E008","Envelope","Envelope White (5\"x7\") w/Window","Each","~/Resources/Items/whiteenvelope.png"),
                new Item("E020","Eraser","Eraser (hard)","Each","~/Resources/Items/eraser.png"),
                new Item("E021","Eraser","Eraser (soft)","Each","~/Resources/Items/eraser.png"),
                new Item("E030","Exercise","Exercise Book (100 pg)","Each","~/Resources/Items/exercisebk.png"),
                new Item("E031","Exercise","Exercise Book (120 pg)","Each","~/Resources/Items/exercisebk.png"),
                new Item("E032","Exercise","Exercise Book A4 Hardcover (100 pg)","Each","~/Resources/Items/exercisebk.png"),
                new Item("E033","Exercise","Exercise Book A4 Hardcover (120 pg)","Each","~/Resources/Items/exercisebk.png"),
                new Item("E034","Exercise","Exercise Book A4 Hardcover (200 pg)","Each","~/Resources/Items/exercisebk.png"),
                new Item("E035","Exercise","Exercise Book Hardcover (100 pg)","Each","~/Resources/Items/exercisebk.png"),
                new Item("E036","Exercise","Exercise Book Hardcover (120 pg)","Each","~/Resources/Items/exercisebk.png"),
                new Item("F020","File","File Separator","Set","~/Resources/Items/fileseparator.png"),
                new Item("F021","File","File-Blue Plain","Each","~/Resources/Items/files.png"),
                new Item("F022","File","File-Blue with Logo","Each","~/Resources/Items/files.png"),
                new Item("F023","File","File-Brown w/o Logo","Each","~/Resources/Items/files.png"),
                new Item("F024","File","File-Brown with Logo","Each","~/Resources/Items/files.png"),
                new Item("F031","File","Folder Plastic Blue","Each","~/Resources/Items/folder.png"),
                new Item("F032","File","Folder Plastic Clear","Each","~/Resources/Items/folder.png"),
                new Item("F033","File","Folder Plastic Green","Each","~/Resources/Items/folder.png"),
                new Item("F034","File","Folder Plastic Pink","Each","~/Resources/Items/folder.png"),
                new Item("F035","File","Folder Plastic Yellow","Each","~/Resources/Items/folder.png"),
                new Item("H011","Pen","Highlighter Blue","Box","~/Resources/Items/highlighter.png"),
                new Item("H012","Pen","Highlighter Green","Box","~/Resources/Items/highlighter.png"),
                new Item("H013","Pen","Highlighter Pink","Box","~/Resources/Items/highlighter.png"),
                new Item("H014","Pen","Highlighter Yellow","Box","~/Resources/Items/highlighter.png"),
                new Item("H031","Puncher","Hole Puncher 2 holes","Each","~/Resources/Items/holepuncher2.png"),
                new Item("H032","Puncher","Hole Puncher 3 holes","Each","~/Resources/Items/holepuncher3.png"),
                new Item("H033","Puncher","Hole Puncher Adjustable","Each","~/Resources/Items/holepuncher2.png"),
                new Item("P010","Pad","Pad Postit Memo 1\"x2\"","Packet","~/Resources/Items/postits.png"),
                new Item("P011","Pad","Pad Postit Memo 1/2\"x1\"","Packet","~/Resources/Items/postits.png"),
                new Item("P012","Pad","Pad Postit Memo 1/2\"x2\"","Packet","~/Resources/Items/postits.png"),
                new Item("P013","Pad","Pad Postit Memo 2\"x3\"","Packet","~/Resources/Items/postits.png"),
                new Item("P014","Pad","Pad Postit Memo 2\"x4\"","Packet","~/Resources/Items/postits.png"),
                new Item("P015","Pad","Pad Postit Memo 2\"x4\"","Packet","~/Resources/Items/postits.png"),
                new Item("P016","Pad","Pad Postit Memo 3/4\"x2\"","Packet","~/Resources/Items/postits.png"),
                new Item("P020","Paper","Paper Photostat A3","Box","~/Resources/Items/A3.png"),
                new Item("P021","Paper","Paper Photostat A4","Box","~/Resources/Items/A4.png"),
                new Item("P030","Pen","Pen Ballpoint Black","Dozen","~/Resources/Items/pensballpoint.png"),
                new Item("P031","Pen","Pen Ballpoint Blue","Dozen","~/Resources/Items/pensballpoint.png"),
                new Item("P032","Pen","Pen Ballpoint Red","Dozen","~/Resources/Items/pensballpoint.png"),
                new Item("P033","Pen","Pen Felt Tip Black","Dozen","~/Resources/Items/pensfelttip.png"),
                new Item("P034","Pen","Pen Felt Tip Blue","Dozen","~/Resources/Items/pensfelttip.png"),
                new Item("P035","Pen","Pen Felt Tip Red","Dozen","~/Resources/Items/pensfelttip.png"),
                new Item("P036","Pen","Pen Transparency Permanent","Packet","~/Resources/Items/pentransparency.png"),
                new Item("P037","Pen","Pen Transparency Soluble","Packet","~/Resources/Items/pentransparency.png"),
                new Item("P038","Pen","Pen Whiteboard Marker Black","Box","~/Resources/Items/penmarkers.png"),
                new Item("P039","Pen","Pen Whiteboard Marker Blue","Box","~/Resources/Items/penmarkers.png"),
                new Item("P040","Pen","Pen Whiteboard Marker Green","Box","~/Resources/Items/penmarkers.png"),
                new Item("P041","Pen","Pen Whiteboard Marker Red","Box","~/Resources/Items/penmarkers.png"),
                new Item("P042","Pen","Pencil 2B","Dozen","~/Resources/Items/pencils.png"),
                new Item("P043","Pen","Pencil 2B With Eraser End","Dozen","~/Resources/Items/pencilsdozen.png"),
                new Item("P044","Pen","Pencil 4H","Dozen","~/Resources/Items/pencils.png"),
                new Item("P045","Pen","Pencil B","Dozen","~/Resources/Items/pencils.png"),
                new Item("P046","Pen","Pencil B With Eraser End","Dozen","~/Resources/Items/pencilsdozen.png"),
                new Item("R001","Ruler","Ruler 6\"","Dozen","~/Resources/Items/ruler6.png"),
                new Item("R002","Ruler","Ruler 12\"","Dozen","~/Resources/Items/ruler12.png"),
                new Item("S100","Scissors","Scissors","Each","~/Resources/Items/scissors.png"),
                new Item("S040","Tape","Scotch Tape","Each","~/Resources/Items/scotchtape.png"),
                new Item("S041","Tape","Scotch Tape Dispenser","Each","~/Resources/Items/scotchtapedispenser.png"),
                new Item("S101","Sharpener","Sharpener","Each","~/Resources/LogicUIcon.png"),
                new Item("S010","Shorthand","Shorthand Book (100 pg)","Each","~/Resources/Items/shorthand.png"),
                new Item("S011","Shorthand","Shorthand Book (120 pg)","Each","~/Resources/Items/shorthand.png"),
                new Item("S012","Shorthand","Shorthand Book (80 pg)","Each","~/Resources/Items/shorthand.png"),
                new Item("S020","Stapler","Stapler No.28","Each","~/Resources/Items/staplers.png"),
                new Item("S021","Stapler","Stapler No.36","Each","~/Resources/Items/staplers.png"),
                new Item("S022","Stapler","Stapler No.28","Box","~/Resources/Items/staplers.png"),
                new Item("S023","Stapler","Stapler No.36","Box","~/Resources/Items/staplers.png"),
                new Item("T001","Tacks","Thumb Tacks Large","Box","~/Resources/Items/thumbtacks.png"),
                new Item("T002","Tacks","Thumb Tacks Medium","Box","~/Resources/Items/thumbtacks.png"),
                new Item("T003","Tacks","Thumb Tacks Small","Box","~/Resources/Items/thumbtacks.png"),
                new Item("T020","Tparency","Transparency Blue","Box","~/Resources/LogicUIcon.png"),
                new Item("T021","Tparency","Transparency Clear","Box","~/Resources/LogicUIcon.png"),
                new Item("T022","Tparency","Transparency Green","Box","~/Resources/LogicUIcon.png"),
                new Item("T023","Tparency","Transparency Red","Box","~/Resources/LogicUIcon.png"),
                new Item("T024","Tparency","Transparency Reverse Blue","Box","~/Resources/LogicUIcon.png"),
                new Item("T025","Tparency","Transparency Cover 3M","Box","~/Resources/LogicUIcon.png"),
                new Item("T100","Tray","Trays In/Out","Box","~/Resources/Items/trays.png"),
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

        static void InitTenders(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing Tenders");
            UnitOfWork uow = new UnitOfWork(context);
            //BANE Tenders...
            List<Tender> banetender = new List<Tender>
            {
                new Tender("C001", "BANE", 2.49, uow),
                new Tender("C002", "BANE", 2.79, uow),
                new Tender("C003", "BANE", 2.99, uow),
                new Tender("C004", "BANE", 5.99, uow),
                new Tender("C005", "BANE", 5.49, uow),
                new Tender("C006", "BANE", 4.99, uow),
                new Tender("E001", "BANE", 1.29, uow),
                new Tender("E002", "BANE", 1.39, uow),
                new Tender("E003", "BANE", 1.49, uow),
                new Tender("E004", "BANE", 1.59, uow),
                new Tender("E005", "BANE", 1.29, uow),
                new Tender("E006", "BANE", 1.39, uow),
                new Tender("E007", "BANE", 1.49, uow),
                new Tender("E008", "BANE", 1.59, uow),
                new Tender("E020", "BANE", 0.39, uow),
                new Tender("E021", "BANE", 0.29, uow),
                new Tender("E030", "BANE", 1.19, uow),
                new Tender("E031", "BANE", 1.29, uow),
                new Tender("E032", "BANE", 1.79, uow),
                new Tender("E033", "BANE", 1.89, uow),
                new Tender("E034", "BANE", 2.29, uow),
                new Tender("E035", "BANE", 1.29, uow),
                new Tender("E036", "BANE", 1.39, uow),
                new Tender("F020", "BANE", 1.99, uow),
                new Tender("F021", "BANE", 1.59, uow),
                new Tender("F022", "BANE", 1.65, uow),
                new Tender("F023", "BANE", 1.59, uow),
                new Tender("F024", "BANE", 1.65, uow),
                new Tender("F031", "BANE", 0.79, uow),
                new Tender("F032", "BANE", 0.79, uow),
                new Tender("F033", "BANE", 0.79, uow),
                new Tender("F034", "BANE", 0.79, uow),
                new Tender("F035", "BANE", 0.79, uow),
                new Tender("H031", "BANE", 4.59, uow),
                new Tender("H032", "BANE", 4.99, uow),
                new Tender("H033", "BANE", 5.59, uow),
                new Tender("P010", "BANE", 1.49, uow),
                new Tender("P011", "BANE", 1.59, uow),
                new Tender("P012", "BANE", 1.69, uow),
                new Tender("P013", "BANE", 1.79, uow),
                new Tender("P014", "BANE", 1.89, uow),
                new Tender("P015", "BANE", 1.89, uow),
                new Tender("P016", "BANE", 1.99, uow),
                new Tender("P020", "BANE", 10.99, uow),
                new Tender("P021", "BANE", 4.99, uow),
                new Tender("H011", "BANE", 14.49, uow),
                new Tender("H012", "BANE", 14.39, uow),
                new Tender("H013", "BANE", 14.49, uow),
                new Tender("H014", "BANE", 14.39, uow),
                new Tender("P030", "BANE", 5.49, uow),
                new Tender("P031", "BANE", 5.49, uow),
                new Tender("P032", "BANE", 5.39, uow),
                new Tender("P033", "BANE", 5.99, uow),
                new Tender("P034", "BANE", 5.99, uow),
                new Tender("P035", "BANE", 5.89, uow),
                new Tender("P036", "BANE", 1.89, uow),
                new Tender("P037", "BANE", 1.59, uow),
                new Tender("P038", "BANE", 20.49, uow),
                new Tender("P039", "BANE", 20.49, uow),
                new Tender("P040", "BANE", 20.49, uow),
                new Tender("P041", "BANE", 20.29, uow),
                new Tender("P042", "BANE", 4.29, uow),
                new Tender("P043", "BANE", 4.39, uow),
                new Tender("P044", "BANE", 4.35, uow),
                new Tender("P045", "BANE", 4.25, uow),
                new Tender("P046", "BANE", 4.35, uow),
                new Tender("R001", "BANE", 7.19, uow),
                new Tender("R002", "BANE", 9.59, uow),
                new Tender("S100", "BANE", 2.39, uow),
                new Tender("S040", "BANE", 1.49, uow),
                new Tender("S041", "BANE", 8.99, uow),
                new Tender("S101", "BANE", 1.10, uow),
                new Tender("S010", "BANE", 0.99, uow),
                new Tender("S011", "BANE", 1.09, uow),
                new Tender("S012", "BANE", 0.89, uow),
                new Tender("S020", "BANE", 1.00, uow),
                new Tender("S021", "BANE", 1.50, uow),
                new Tender("S022", "BANE", 24.00, uow),
                new Tender("S023", "BANE", 36.00, uow),
                new Tender("T001", "BANE", 2.00, uow),
                new Tender("T002", "BANE", 1.50, uow),
                new Tender("T003", "BANE", 1.00, uow),
                new Tender("T020", "BANE", 3.99, uow),
                new Tender("T021", "BANE", 3.89, uow),
                new Tender("T022", "BANE", 3.99, uow),
                new Tender("T023", "BANE", 3.99, uow),
                new Tender("T024", "BANE", 3.79, uow),
                new Tender("T025", "BANE", 4.09, uow),
                new Tender("T100", "BANE", 12.59, uow),


            };

            foreach ( Tender t in banetender)
            {
                context.Tenders.Add(t);
            }
            context.SaveChanges();
            List<Tender> omegtender = new List<Tender>
            {
                new Tender("C001", "OMEG", 2.50, uow),
                new Tender("C002", "OMEG", 2.80, uow),
                new Tender("C003", "OMEG", 3.00, uow),
                new Tender("C004", "OMEG", 6.00, uow),
                new Tender("C005", "OMEG", 5.50, uow),
                new Tender("C006", "OMEG", 5.00, uow),
                new Tender("E001", "OMEG", 1.30, uow),
                new Tender("E002", "OMEG", 1.40, uow),
                new Tender("E003", "OMEG", 1.50, uow),
                new Tender("E004", "OMEG", 1.60, uow),
                new Tender("E005", "OMEG", 1.30, uow),
                new Tender("E006", "OMEG", 1.40, uow),
                new Tender("E007", "OMEG", 1.50, uow),
                new Tender("E008", "OMEG", 1.60, uow),
                new Tender("E020", "OMEG", 0.40, uow),
                new Tender("E021", "OMEG", 0.30, uow),
                new Tender("E030", "OMEG", 0.30, uow),
                new Tender("E031", "OMEG", 0.30, uow),
                new Tender("E032", "OMEG", 0.30, uow),
                new Tender("F020", "OMEG", 2.00, uow),
                new Tender("F021", "OMEG", 1.60, uow),
                new Tender("F022", "OMEG", 1.70, uow),
                new Tender("F023", "OMEG", 1.60, uow),
                new Tender("F024", "OMEG", 1.70, uow),
                new Tender("F031", "OMEG", 0.80, uow),
                new Tender("F032", "OMEG", 0.80, uow),
                new Tender("F033", "OMEG", 0.80, uow),
                new Tender("F034", "OMEG", 0.80, uow),
                new Tender("F035", "OMEG", 0.80, uow),
                new Tender("H031", "OMEG", 4.50, uow),
                new Tender("H032", "OMEG", 4.90, uow),
                new Tender("H033", "OMEG", 5.50, uow),
                new Tender("P010", "OMEG", 1.50, uow),
                new Tender("P011", "OMEG", 1.60, uow),
                new Tender("P012", "OMEG", 1.70, uow),
                new Tender("P013", "OMEG", 1.80, uow),
                new Tender("P014", "OMEG", 1.90, uow),
                new Tender("P015", "OMEG", 1.90, uow),
                new Tender("P016", "OMEG", 2.00, uow),
                new Tender("P020", "OMEG", 10.89, uow),
                new Tender("P021", "OMEG", 4.89, uow),
                new Tender("H011", "OMEG", 14.50, uow),
                new Tender("H012", "OMEG", 14.40, uow),
                new Tender("H013", "OMEG", 14.50, uow),
                new Tender("H014", "OMEG", 14.40, uow),
                new Tender("P030", "OMEG", 5.50, uow),
                new Tender("P031", "OMEG", 5.50, uow),
                new Tender("P032", "OMEG", 5.40, uow),
                new Tender("P033", "OMEG", 6.00, uow),
                new Tender("P034", "OMEG", 6.00, uow),
                new Tender("P035", "OMEG", 5.90, uow),
                new Tender("P036", "OMEG", 1.90, uow),
                new Tender("P037", "OMEG", 1.90, uow),
                new Tender("P038", "OMEG", 20.50, uow),
                new Tender("P039", "OMEG", 20.50, uow),
                new Tender("P040", "OMEG", 20.30, uow),
                new Tender("P041", "OMEG", 20.40, uow),
                new Tender("P042", "OMEG", 4.30, uow),
                new Tender("P043", "OMEG", 4.40, uow),
                new Tender("P044", "OMEG", 4.36, uow),
                new Tender("P045", "OMEG", 4.26, uow),
                new Tender("P046", "OMEG", 4.36, uow),
                new Tender("R001", "OMEG", 7.20, uow),
                new Tender("R002", "OMEG", 10.00, uow),
                new Tender("S100", "OMEG", 2.40, uow),
                new Tender("S040", "OMEG", 1.50, uow),
                new Tender("S041", "OMEG", 9.00, uow),
                new Tender("S101", "OMEG", 1.09, uow),
                new Tender("S010", "OMEG", 1.00, uow),
                new Tender("S011", "OMEG", 1.10, uow),
                new Tender("S012", "OMEG", 0.90, uow),
                new Tender("S020", "OMEG", 0.99, uow),
                new Tender("S021", "OMEG", 1.49, uow),
                new Tender("S022", "OMEG", 23.99, uow),
                new Tender("S023", "OMEG", 35.99, uow),
                new Tender("T001", "OMEG", 1.99, uow),
                new Tender("T002", "OMEG", 1.49, uow),
                new Tender("T003", "OMEG", 0.99, uow),
                new Tender("T020", "OMEG", 4.00, uow),
                new Tender("T021", "OMEG", 3.90, uow),
                new Tender("T022", "OMEG", 4.00, uow),
                new Tender("T023", "OMEG", 4.00, uow),
                new Tender("T024", "OMEG", 3.80, uow),
                new Tender("T025", "OMEG", 4.10, uow),
                new Tender("T100", "OMEG", 12.60, uow),
            };

            foreach (Tender t in omegtender)
            {
                context.Tenders.Add(t);
            }
            context.SaveChanges();
            List<Tender> alpatender = new List<Tender>
            {
                new Tender("C001", "ALPA", 2.69, uow),
                new Tender("C002", "ALPA", 2.99, uow),
                new Tender("C003", "ALPA", 3.19, uow),
                new Tender("C004", "ALPA", 6.19, uow),
                new Tender("C005", "ALPA", 5.69, uow),
                new Tender("C006", "ALPA", 5.19, uow),
                new Tender("E001", "ALPA", 1.35, uow),
                new Tender("E002", "ALPA", 1.45, uow),
                new Tender("E003", "ALPA", 1.55, uow),
                new Tender("E004", "ALPA", 1.65, uow),
                new Tender("E005", "ALPA", 1.35, uow),
                new Tender("E006", "ALPA", 1.45, uow),
                new Tender("E007", "ALPA", 1.55, uow),
                new Tender("E008", "ALPA", 1.65, uow),
                new Tender("E020", "ALPA", 0.45, uow),
                new Tender("E021", "ALPA", 0.35, uow),
                new Tender("E030", "ALPA", 1.15, uow),
                new Tender("E031", "ALPA", 1.25, uow),
                new Tender("E032", "ALPA", 1.75, uow),
                new Tender("E033", "ALPA", 1.85, uow),
                new Tender("E034", "ALPA", 2.25, uow),
                new Tender("E035", "ALPA", 1.25, uow),
                new Tender("E036", "ALPA", 1.35, uow),
                new Tender("F020", "ALPA", 2.05, uow),
                new Tender("F021", "ALPA", 1.65, uow),
                new Tender("F022", "ALPA", 1.75, uow),
                new Tender("F023", "ALPA", 1.65, uow),
                new Tender("F024", "ALPA", 1.75, uow),
                new Tender("F031", "ALPA", 0.85, uow),
                new Tender("F032", "ALPA", 0.85, uow),
                new Tender("F033", "ALPA", 0.85, uow),
                new Tender("F034", "ALPA", 0.85, uow),
                new Tender("F035", "ALPA", 0.85, uow),
                new Tender("H031", "ALPA", 4.55, uow),
                new Tender("H032", "ALPA", 4.95, uow),
                new Tender("H033", "ALPA", 5.55, uow),
                new Tender("P010", "ALPA", 1.55, uow),
                new Tender("P011", "ALPA", 1.65, uow),
                new Tender("P012", "ALPA", 1.75, uow),
                new Tender("P013", "ALPA", 1.85, uow),
                new Tender("P014", "ALPA", 1.95, uow),
                new Tender("P015", "ALPA", 1.95, uow),
                new Tender("P016", "ALPA", 2.05, uow),
                new Tender("P020", "ALPA", 10.85, uow),
                new Tender("P021", "ALPA", 4.85, uow),
                new Tender("H011", "ALPA", 14.55, uow),
                new Tender("H012", "ALPA", 14.45, uow),
                new Tender("H013", "ALPA", 14.55, uow),
                new Tender("H014", "ALPA", 14.45, uow),
                new Tender("P030", "ALPA", 5.55, uow),
                new Tender("P031", "ALPA", 5.55, uow),
                new Tender("P032", "ALPA", 5.45, uow),
                new Tender("P033", "ALPA", 6.05, uow),
                new Tender("P034", "ALPA", 6.05, uow),
                new Tender("P035", "ALPA", 5.95, uow),
                new Tender("P036", "ALPA", 1.95, uow),
                new Tender("P037", "ALPA", 1.65, uow),
                new Tender("P038", "ALPA", 20.55, uow),
                new Tender("P039", "ALPA", 20.55, uow),
                new Tender("P040", "ALPA", 20.35, uow),
                new Tender("P041", "ALPA", 20.45, uow),
                new Tender("P042", "ALPA", 4.25, uow),
                new Tender("P043", "ALPA", 4.35, uow),
                new Tender("P044", "ALPA", 4.30, uow),
                new Tender("P045", "ALPA", 4.20, uow),
                new Tender("P046", "ALPA", 4.30, uow),
                new Tender("R001", "ALPA", 7.25, uow),
                new Tender("R002", "ALPA", 10.05, uow),
                new Tender("S100", "ALPA", 2.45, uow),
                new Tender("S040", "ALPA", 1.55, uow),
                new Tender("S041", "ALPA", 9.05, uow),
                new Tender("S101", "ALPA", 1.05, uow),
                new Tender("S010", "ALPA", 0.95, uow),
                new Tender("S011", "ALPA", 1.05, uow),
                new Tender("S012", "ALPA", 0.85, uow),
                new Tender("S020", "ALPA", 0.95, uow),
                new Tender("S021", "ALPA", 1.45, uow),
                new Tender("S022", "ALPA", 24.05, uow),
                new Tender("S023", "ALPA", 36.05, uow),
                new Tender("T001", "ALPA", 1.95, uow),
                new Tender("T002", "ALPA", 1.45, uow),
                new Tender("T003", "ALPA", 0.95, uow),
                new Tender("T020", "ALPA", 4.05, uow),
                new Tender("T021", "ALPA", 3.95, uow),
                new Tender("T022", "ALPA", 4.05, uow),
                new Tender("T023", "ALPA", 4.05, uow),
                new Tender("T024", "ALPA", 3.85, uow),
                new Tender("T025", "ALPA", 4.15, uow),
                new Tender("T100", "ALPA", 12.55, uow),
            };

            foreach (Tender t in alpatender)
            {
                context.Tenders.Add(t);
            }
            context.SaveChanges();
            List<Tender> cheptender = new List<Tender>
            {
                new Tender("C001", "CHEP", 2.45, uow),
                new Tender("C002", "CHEP", 2.75, uow),
                new Tender("C003", "CHEP", 2.95, uow),
                new Tender("C004", "CHEP", 5.95, uow),
                new Tender("C005", "CHEP", 5.45, uow),
                new Tender("C006", "CHEP", 4.95, uow),
                new Tender("E001", "CHEP", 1.25, uow),
                new Tender("E002", "CHEP", 1.35, uow),
                new Tender("E003", "CHEP", 1.45, uow),
                new Tender("E004", "CHEP", 1.55, uow),
                new Tender("E005", "CHEP", 1.25, uow),
                new Tender("E006", "CHEP", 1.35, uow),
                new Tender("E007", "CHEP", 1.45, uow),
                new Tender("E008", "CHEP", 1.55, uow),
                new Tender("E020", "CHEP", 0.35, uow),
                new Tender("E021", "CHEP", 0.25, uow),
                new Tender("E030", "CHEP", 1.29, uow),
                new Tender("E031", "CHEP", 1.39, uow),
                new Tender("E032", "CHEP", 1.89, uow),
                new Tender("E033", "CHEP", 1.99, uow),
                new Tender("E034", "CHEP", 2.39, uow),
                new Tender("E035", "CHEP", 1.39, uow),
                new Tender("E036", "CHEP", 1.49, uow),
                new Tender("F020", "CHEP", 2.00, uow),
                new Tender("F021", "CHEP", 1.60, uow),
                new Tender("F022", "CHEP", 1.70, uow),
                new Tender("F023", "CHEP", 1.60, uow),
                new Tender("F024", "CHEP", 1.70, uow),
                new Tender("F031", "CHEP", 0.80, uow),
                new Tender("F032", "CHEP", 0.80, uow),
                new Tender("F033", "CHEP", 0.80, uow),
                new Tender("F034", "CHEP", 0.80, uow),
                new Tender("F035", "CHEP", 0.80, uow),
                new Tender("H031", "CHEP", 4.60, uow),
                new Tender("H032", "CHEP", 5.00, uow),
                new Tender("H033", "CHEP", 5.60, uow),
                new Tender("P010", "CHEP", 1.45, uow),
                new Tender("P011", "CHEP", 1.55, uow),
                new Tender("P012", "CHEP", 1.65, uow),
                new Tender("P013", "CHEP", 1.75, uow),
                new Tender("P014", "CHEP", 1.85, uow),
                new Tender("P015", "CHEP", 1.85, uow),
                new Tender("P016", "CHEP", 1.95, uow),
                new Tender("P020", "CHEP", 11.00, uow),
                new Tender("P021", "CHEP", 5.00, uow),
                new Tender("H011", "CHEP", 14.45, uow),
                new Tender("H012", "CHEP", 14.35, uow),
                new Tender("H013", "CHEP", 14.45, uow),
                new Tender("H014", "CHEP", 14.35, uow),
                new Tender("P030", "CHEP", 5.45, uow),
                new Tender("P031", "CHEP", 5.45, uow),
                new Tender("P032", "CHEP", 5.35, uow),
                new Tender("P033", "CHEP", 6.09, uow),
                new Tender("P034", "CHEP", 6.09, uow),
                new Tender("P035", "CHEP", 5.99, uow),
                new Tender("P036", "CHEP", 1.99, uow),
                new Tender("P037", "CHEP", 1.69, uow),
                new Tender("P038", "CHEP", 20.45, uow),
                new Tender("P039", "CHEP", 20.45, uow),
                new Tender("P040", "CHEP", 20.25, uow),
                new Tender("P041", "CHEP", 20.35, uow),
                new Tender("P042", "CHEP", 4.24, uow),
                new Tender("P043", "CHEP", 4.34, uow),
                new Tender("P044", "CHEP", 4.29, uow),
                new Tender("P045", "CHEP", 4.19, uow),
                new Tender("P046", "CHEP", 4.29, uow),
                new Tender("R001", "CHEP", 7.15, uow),
                new Tender("R002", "CHEP", 9.55, uow),
                new Tender("S100", "CHEP", 2.50, uow),
                new Tender("S040", "CHEP", 1.60, uow),
                new Tender("S041", "CHEP", 9.10, uow),
                new Tender("S101", "CHEP", 1.00, uow),
                new Tender("S010", "CHEP", 0.90, uow),
                new Tender("S011", "CHEP", 1.00, uow),
                new Tender("S012", "CHEP", 0.80, uow),
                new Tender("S020", "CHEP", 1.05, uow),
                new Tender("S021", "CHEP", 1.55, uow),
                new Tender("S022", "CHEP", 24.09, uow),
                new Tender("S023", "CHEP", 36.09, uow),
                new Tender("T001", "CHEP", 2.05, uow),
                new Tender("T002", "CHEP", 1.55, uow),
                new Tender("T003", "CHEP", 1.05, uow),
                new Tender("T020", "CHEP", 3.95, uow),
                new Tender("T021", "CHEP", 3.85, uow),
                new Tender("T022", "CHEP", 3.95, uow),
                new Tender("T023", "CHEP", 3.95, uow),
                new Tender("T024", "CHEP", 3.75, uow),
                new Tender("T025", "CHEP", 4.05, uow),
                new Tender("T100", "CHEP", 12.50, uow),
            };

            foreach (Tender t in cheptender)
            {
                context.Tenders.Add(t);
            }

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
            {
                if (inventoryItem.Item.ID.Contains("3"))
                    inventoryItem.InStoreQty /= 5;
                context.InventoryItems.Add(inventoryItem);
            }
                
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
            context.RequisitionOrders.Add(reqform);

            //Approved Requisition Orders
            Staff staff2 = StaffRepository.GetByID(10010); //ARCH dept
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
            context.RequisitionOrders.Add(reqform2);

            Staff staff3 = StaffRepository.GetByID(10044); //ENGL dept
            RequisitionOrder reqform3 = new RequisitionOrder(staff3);
            reqform3.RepliedByStaff = StaffRepository.GetByID(10041);
            List<DocumentItem> documentItems3 = new List<DocumentItem>
            {
                new DocumentItem(ItemRepository.GetByID("C001"),10),
                new DocumentItem(ItemRepository.GetByID("H014"),5),
                new DocumentItem(ItemRepository.GetByID("F033"),20),
            };
            reqform3.DocumentItems = documentItems3;
            reqform3.Status =(Status)1;
            context.RequisitionOrders.Add(reqform3);

            Staff staff4 = StaffRepository.GetByID(10024); //COMM dept
            RequisitionOrder reqform4 = new RequisitionOrder(staff4);
            reqform4.RepliedByStaff = StaffRepository.GetByID(10020);
            List<DocumentItem> documentItems4 = new List<DocumentItem>
            {
                new DocumentItem(ItemRepository.GetByID("C001"),5),
                new DocumentItem(ItemRepository.GetByID("H014"),15),
                new DocumentItem(ItemRepository.GetByID("P010"),20),
            };
            reqform4.DocumentItems = documentItems4;
            reqform4.Status = (Status)1;
            context.RequisitionOrders.Add(reqform4);

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

        static void InitPurchaseOrders(DatabaseContext context)
        {
            UnitOfWork uow = new UnitOfWork(context);

            Staff Supervisor = StaffRepository.GetByID(10002);

            Staff staff1 = StaffRepository.GetByID(10003);

            Debug.WriteLine("\tInitializing Purchase Orders");
            // Clerk 1
            PurchaseOrder PO1 = new PurchaseOrder(10003, "ALPA", uow);
            List<PurchaseItem> purchaseItems = new List<PurchaseItem>{
                new PurchaseItem("C005",PO1.Supplier.ID,10,uow),
                new PurchaseItem("H032",PO1.Supplier.ID,12,uow),
                new PurchaseItem("C001",PO1.Supplier.ID,10,uow),
            };
            PO1.PurchaseItems = purchaseItems;
            PO1.Approve(Supervisor);
            context.PurchaseOrders.Add(PO1);

            // Clerk 2
            PurchaseOrder PO2 = new PurchaseOrder(10004, "BANE", uow);
            List<PurchaseItem> purchaseItems2 = new List<PurchaseItem>{
                new PurchaseItem("C006",PO2.Supplier.ID,10,uow),
                new PurchaseItem("E003",PO2.Supplier.ID, 9,uow),
                new PurchaseItem("C001",PO2.Supplier.ID,10,uow),
            };
            PO2.PurchaseItems = purchaseItems2;
            PO2.Approve(Supervisor);
            context.PurchaseOrders.Add(PO2);

            //Clerk 3
            PurchaseOrder PO3 = new PurchaseOrder(10004, "CHEP", uow);
            List<PurchaseItem> purchaseItems3 = new List<PurchaseItem>{
                new PurchaseItem("C003",PO3.Supplier.ID,10,uow),
                new PurchaseItem("P046",PO3.Supplier.ID, 9,uow),
                new PurchaseItem("R001",PO3.Supplier.ID,3,uow),
            };
            PO3.PurchaseItems = purchaseItems3;
            PO3.Approve(Supervisor);
            context.PurchaseOrders.Add(PO3);

            // Clerk 1
            PurchaseOrder PO4 = new PurchaseOrder(10003, "ALPA", uow);
            List<PurchaseItem> purchaseItems4 = new List<PurchaseItem>{
                new PurchaseItem("C005",PO4.Supplier.ID,10,uow),
                new PurchaseItem("H032",PO4.Supplier.ID,12,uow),
                new PurchaseItem("C001",PO4.Supplier.ID,10,uow),
            };
            PO4.PurchaseItems = purchaseItems4;
            context.PurchaseOrders.Add(PO4);

            // Clerk 2
            PurchaseOrder PO5 = new PurchaseOrder(10004, "BANE", uow);
            List<PurchaseItem> purchaseItems5 = new List<PurchaseItem>{
                new PurchaseItem("C006",PO5.Supplier.ID,10,uow),
                new PurchaseItem("E003",PO5.Supplier.ID, 9,uow),
                new PurchaseItem("C001",PO5.Supplier.ID,10,uow),
            };
            PO5.PurchaseItems = purchaseItems5;
            context.PurchaseOrders.Add(PO5);

            //Clerk 3
            PurchaseOrder PO6 = new PurchaseOrder(10004, "CHEP", uow);
            List<PurchaseItem> purchaseItems6 = new List<PurchaseItem>{
                new PurchaseItem("C003",PO6.Supplier.ID,10,uow),
                new PurchaseItem("P046",PO6.Supplier.ID, 9,uow),
                new PurchaseItem("R001",PO6.Supplier.ID,3,uow),
            };
            PO6.PurchaseItems = purchaseItems6;
            context.PurchaseOrders.Add(PO6);

            context.SaveChanges();
        }
        
        static void InitDeptHeadAuthorizations(DatabaseContext context)
        {
            Debug.WriteLine("\tInitializing DeptHeadAuthorizations");
            List<DeptHeadAuthorization> deptHeadAuthorizations = new List<DeptHeadAuthorization>
            {
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10006),1,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10013),2,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10020),3,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10027),4,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10034),5,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10041),6,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10048),7,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10055),8,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10062),9,DateTime.Now,DateTime.Now.AddDays(365)),
               new DeptHeadAuthorization(StaffRepository.GetStaffbyID(10069),11,DateTime.Now,DateTime.Now.AddDays(365)),
            };
            foreach (DeptHeadAuthorization dha in deptHeadAuthorizations)
                context.DeptHeadAuthorizations.Add(dha);
            context.SaveChanges();
        }

    }
}