using SSIMS.DAL;
using SSIMS.Service;
using SSIMS.Models;
using SSIMS.ViewModels;
using SSIMS.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Status = SSIMS.Models.Status;
using PagedList;
using System.Net.Mail;

namespace SSIMS.Controllers
{
    [AuthenticationFilter]
    [AuthorizationFilter]
    public class RequisitionController : Controller
    {
        //DatabaseContext db = new DatabaseContext();

        readonly IRequisitionService RequisitionService = new RequisitionService();
        readonly IStaffService StaffService = new StaffService();
        readonly ILoginService loginService = new LoginService();


        [HttpPost]
        public ActionResult Add(string SelectedCategory, string SelectedDescription, string Quantity)
        {
            UnitOfWork uni = new UnitOfWork();

            if (Session["RequestList"] == null)
                Session["RequestList"] = new List<RequisitionItemVM>();
            List<RequisitionItemVM> requisitionItems = (List<RequisitionItemVM>)Session["RequestList"];

            if (ModelState.IsValid)
            {

                RequisitionItemVM requisitionItem = new RequisitionItemVM { SelectedCategory = SelectedCategory, SelectedDescription = SelectedDescription, Quantity = int.Parse(Quantity) };
                requisitionItem.UnitOfMeasure = uni.ItemRepository.GetByID(SelectedDescription).UnitOfMeasure;
                requisitionItem.displayDescription = uni.ItemRepository.GetByID(SelectedDescription).Description;

                requisitionItems.Add(requisitionItem);
                Debug.WriteLine("Added item to list, going back to create");
            }
            Session["RequestList"] = requisitionItems;
            return RedirectToAction("Create", "Requisition");
        }

        // GET: Requisition/Create
        public ActionResult Create()
        {
            if (!LoginService.IsAuthorizedRoles("staff", "rep"))
                return RedirectToAction("Index", "Home");

            Debug.WriteLine("Hello im in create now");
            
            RequisitionItemVM vm = new RequisitionItemVM();
            List<RequisitionItemVM> list = (List<RequisitionItemVM>)Session["RequestList"];
            
            ViewBag.RequisitionItems = list;
            if (list == null)
            {
                List<RequisitionItemVM> vmList = new List<RequisitionItemVM>();
                vmList.Add(vm);
                Session["RequestList"] = vmList;
                ViewBag.RequisitionItems = vmList;
            }

            return View(vm);
        }

        [HttpGet]
        public ActionResult GetDescription(string category)
        {
            var unitofwork = new UnitOfWork();
            var irepo = unitofwork.ItemRepository;

            IEnumerable<SelectListItem> descriptionlist = (IEnumerable<SelectListItem>)irepo.GetDescriptions(category);
            return Json(descriptionlist, JsonRequestBehavior.AllowGet);

        }


        // POST: Requisition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRequisition()
        {
            var unitofwork = new UnitOfWork();
            List<RequisitionItemVM> vmList = (List<RequisitionItemVM>)Session["RequestList"];
            //RequisitionItemVM rcvm = new RequisitionItemVM;
            Debug.WriteLine(vmList.Count);
            foreach (RequisitionItemVM vm in vmList)
            {
                Debug.WriteLine(vm.Quantity);
            }

            List<DocumentItem> doitems = new List<DocumentItem>();
            if (ModelState.IsValid)
            {
                foreach (RequisitionItemVM rcvm in vmList)
                {
                    if (rcvm.Quantity != 0)
                    {
                        Item item = unitofwork.ItemRepository.GetItembyDescrption(rcvm.SelectedDescription);
                        DocumentItem doitem = unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);

                        doitems.Add(doitem);
                        unitofwork.Save();
                    }
                }

                Staff staff = loginService.StaffFromSession;
                RequisitionOrder rq = new RequisitionOrder(unitofwork.StaffRepository.GetByID(staff.ID));

                rq.DocumentItems = doitems;
                
                unitofwork.RequisitionOrderRepository.Insert(rq);

                unitofwork.Save();
                Debug.WriteLine("rq.ID:" + rq.ID);
 
                Session["RequestList"] = null;
               
                return RedirectToAction("Index");
            }

            return View();
        }



        // GET: Requisition
        public ActionResult Index(Staff staff, int? page, string sortOrder)
        {
            if (LoginService.IsAuthorizedRoles("head"))
                return RedirectToAction("Manage", "Requisition");
            if (LoginService.IsAuthorizedRoles("manager", "supervisor", "clerk"))
                return RedirectToAction("ViewHistory", "Requisition");

            staff = loginService.StaffFromSession;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.CreDates = String.IsNullOrEmpty(sortOrder) ? "cre_date" : "";
            ViewBag.ResDates = String.IsNullOrEmpty(sortOrder) ? "res_date" : "";

            Debug.WriteLine(staff.Name + staff.ID);
            Debug.WriteLine(Session["role"]);
            Debug.WriteLine(staff.StaffRole);
            ViewBag.staffrole = staff.StaffRole;
            ViewBag.department = staff.DepartmentID;
            //staff.ID = 10006;
            var unitofwork = new UnitOfWork();

            var reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.CreatedByStaffID == staff.ID).ToList();

            switch (sortOrder)
            {
                case "cre_date":
                    reqList = (List<RequisitionOrder>)reqList.OrderByDescending(i => i.CreatedDate).ToList();
                    break;
                case "res_date":
                    reqList = (List<RequisitionOrder>)reqList.OrderByDescending(i => i.ResponseDate).ToList();
                    break;
            }

                    Debug.WriteLine(reqList.Count);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(reqList.ToPagedList(pageNumber, pageSize));

        }

        //private ActionResult View(object p)
        //{
        //    throw new NotImplementedException();
        //}

        // GET: Requisition/ViewHistory
        public ActionResult ViewHistory(int? page, string status, string sortOrder)
        {
            if (LoginService.IsAuthorizedRoles("staff"))
                return RedirectToAction("Index", "Home");
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CreDates = String.IsNullOrEmpty(sortOrder) ? "cre_date" : "";
            ViewBag.ResDates = String.IsNullOrEmpty(sortOrder) ? "res_date" : "";

            ViewBag.CurrentStatus = status;

            var unitofwork = new UnitOfWork();
            var reqList = new List<RequisitionOrder>();
            //reqList = unitofwork.RequisitionOrderRepository.Get().ToList();

            Staff staff= loginService.StaffFromSession;
            ViewBag.staffrole = staff.StaffRole;

            Debug.WriteLine("staff role "+staff.StaffRole);
            Debug.WriteLine("session role "+ Session["role"]);

            //rep&head=>see their own department
            //all store staff can see all
            if (staff.StaffRole == "DeptHead" || staff.StaffRole == "DeptRep")
            {
                Debug.WriteLine(staff.StaffRole);
                reqList = unitofwork.RequisitionOrderRepository.Get(filter:x=>(x.Status == Models.Status.Approved || x.Status == Models.Status.Rejected || x.Status == Models.Status.Completed) && x.CreatedByStaff.DepartmentID==staff.DepartmentID).ToList();


                switch (status)
                {
                    case "Approved":
                        //reqList =(List<RequisitionOrder>)reqList.Where(x => x.Status == Models.Status.Approved);
                        reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Approved && x.CreatedByStaff.DepartmentID==staff.DepartmentID).ToList();
                        break;
                    case "Rejected":
                        //reqList = (List<RequisitionOrder>)reqList.Where(x => x.Status == Models.Status.Rejected);
                        reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Rejected && x.CreatedByStaff.DepartmentID == staff.DepartmentID).ToList();
                        break;
                    case "Completed":
                        //reqList = (List<RequisitionOrder>)reqList.Where(x => x.Status == Models.Status.Completed);
                        reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Completed && x.CreatedByStaff.DepartmentID == staff.DepartmentID).ToList();
                        break;
                    case "All":
                        //reqList = (List<RequisitionOrder>)reqList.Where(x => x.Status == Models.Status.Approved || x.Status == Models.Status.Rejected || x.Status == Models.Status.Completed);
                        //reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => (x.Status == Models.Status.Approved || x.Status == Models.Status.Rejected || x.Status == Models.Status.Completed) && x.CreatedByStaff.DepartmentID == staff.DepartmentID).ToList();
                        reqList=reqList;
                        break;
                    default:
                        //reqList = (List<RequisitionOrder>)reqList.Where(x => x.Status == Models.Status.Approved || x.Status == Models.Status.Rejected || x.Status == Models.Status.Completed);
                        //reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => (x.Status == Models.Status.Approved || x.Status == Models.Status.Rejected || x.Status == Models.Status.Completed) && x.CreatedByStaff.DepartmentID == staff.DepartmentID).ToList();
                        reqList = reqList;
                        break;
                }
                switch (sortOrder)
                {
                    case "cre_date":
                        reqList = (List<RequisitionOrder>)reqList.OrderByDescending(i => i.CreatedDate).ToList();
                        break;
                    case "res_date":
                        reqList = (List<RequisitionOrder>)reqList.OrderByDescending(i => i.ResponseDate).ToList();
                        break;
                }
            }
            if(staff.StaffRole== "Manager"|| staff.StaffRole == "Supervisor" || staff.StaffRole == "Clerk")
            {
                Debug.WriteLine(staff.StaffRole);
                reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Approved || x.Status == Models.Status.Rejected || x.Status == Models.Status.Completed).ToList();

                Debug.WriteLine(reqList.Count);

                switch (status)
                {
                    case "Approved":
                        reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Approved).ToList();
                        break;
                    case "Rejected":
                        reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Rejected).ToList();
                        break;
                    case "Completed":
                        reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Completed ).ToList();
                        break;
                    case "All":
                        reqList = reqList;
                        //reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Approved || x.Status == Models.Status.Rejected || x.Status == Models.Status.Completed).ToList();
                        break;
                    default:
                        reqList = reqList;
                        //reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Approved || x.Status == Models.Status.Rejected || x.Status == Models.Status.Completed).ToList();
                        break;

                }
                switch (sortOrder)
                {
                    case "cre_date":
                        reqList = (List<RequisitionOrder>)reqList.OrderByDescending(i => i.CreatedDate).ToList();
                        break;
                    case "res_date":
                        reqList = (List<RequisitionOrder>)reqList.OrderByDescending(i => i.ResponseDate).ToList();
                        break;
                }
                Debug.WriteLine(reqList.Count);

            }


           

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reqList.ToPagedList(pageNumber, pageSize));

        }

        //GET:Requisition/Manage
        public ActionResult Manage(int? page)
        {
            if (!LoginService.IsAuthorizedRoles("head"))
                return RedirectToAction("Index", "Home");

            Staff staff = loginService.StaffFromSession;
           

            Debug.WriteLine(staff.DepartmentID);
            var unitofwork = new UnitOfWork();
            var reqList = unitofwork.RequisitionOrderRepository.Get(filter: x => x.Status == Models.Status.Pending && x.CreatedByStaff.DepartmentID==staff.DepartmentID);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reqList.ToPagedList(pageNumber, pageSize));
        }

       
        // GET: Requisition/Create
        public ActionResult Create2()
        {
            //for create dynamic dropdownlist
            var unitofwork = new UnitOfWork();
            var irepo = unitofwork.ItemRepository;
            RequisitionItemVM vm = new RequisitionItemVM();
            vm.Categories = irepo.GetCategories();
            vm.Descriptions = irepo.GetDescriptions();

            return View(vm);
        }
        
       
        // POST: Requisition/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2([Bind(Include = "SelectedCategory,SelectedDescription,Quantity")] RequisitionItemVM rcvm)
        {
            var unitofwork = new UnitOfWork();
            Debug.WriteLine(rcvm.SelectedCategory + "\n" + rcvm.SelectedDescription + " x " + rcvm.Quantity.ToString());
            Debug.WriteLine(rcvm.CreatedDate);
            Debug.WriteLine(rcvm.Status);
            // Debug.WriteLine(rcvm.RequisitionOrderID);
            if (ModelState.IsValid)
            {

                //get item by itemID
                Item item = unitofwork.ItemRepository.GetItembyDescrption(rcvm.SelectedDescription);
                Debug.WriteLine(item.ID);

                //save this item into the doucumentitem(item, qty)
                DocumentItem doitem = unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);
                unitofwork.Save();
                Debug.WriteLine(doitem.ID);

                RequisitionOrder rq = new RequisitionOrder(unitofwork.StaffRepository.GetByID(10006));
                rq.DocumentItems.Add(unitofwork.DocumentItemRepository.GetByID(doitem.ID));

                unitofwork.RequisitionOrderRepository.Insert(rq);

                unitofwork.Save();

                return RedirectToAction("Index");
            }

            return View(rcvm);
        }


        // GET: Requisition/Details/4
        public ActionResult Details(int id)
        {
            //return documentitem by input requisitionOrder ID
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Staff staff = loginService.StaffFromSession;
            ViewBag.staffrole = staff.StaffRole;
            //Debug.WriteLine(ViewBag.staffrole);

            var unitofwork = new UnitOfWork();

            var requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();

            //Debug.WriteLine("staff department ID: " + requisitionOrder.CreatedByStaff.DepartmentID);
            //Debug.WriteLine("department name: " + requisitionOrder.CreatedByStaff.Department.DeptName);

            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            return View(requisitionOrder);

        }

        // GET: Requisition/Edit/4
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UnitOfWork uni = new UnitOfWork();
            Debug.WriteLine("edit id: " + id);
            RequisitionOrder ro = uni.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").First();

            Debug.WriteLine("ro.id: " + ro.ID);
            Debug.WriteLine(" ro.CreatedByStaffID: " + ro.CreatedByStaffID);
            Debug.WriteLine("ro.DocumentItems.Count: " + ro.DocumentItems.Count);
            Debug.WriteLine(" ro.DocumentItems: " + ro.DocumentItems.ToList().Count);

            RequisitionItemVM vm = new RequisitionItemVM();

            //ro.DocumentItems.ToList();

            //List<RequisitionItemVM> reitems = RequisitionService.ConvertDocumentItemsToRequisitionItems(doitems);
            //Debug.WriteLine("reitems.count " + reitems.Count);
            //Session["RequestList"]=reitems;
            //ViewBag.RequisitionItems = reitems;

            vm.ROID = id;
            vm.DocumentItems = ro.DocumentItems.ToList();
            return View(vm);
        }

        //POST:Requisition/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "SelectedCategory,SelectedDescription,Quantity,ROID, DocumentItems")] RequisitionItemVM rcvm, int id)
        {
            Debug.WriteLine("I am here");
            Debug.WriteLine(id);
            var unitofwork = new UnitOfWork();

            RequisitionOrder rq = unitofwork.RequisitionOrderRepository.Get(filter: x=> x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            //rq.DocumentItems = rcvm.DocumentItems;

            foreach(DocumentItem dt in rcvm.DocumentItems)
            {
                Debug.WriteLine("dt.Item.ID " + dt.Item.ID);
                foreach(DocumentItem olditem in rq.DocumentItems)
                {
                    if(dt.Item.ID == olditem.Item.ID)
                    {
                        olditem.Qty = dt.Qty;
                        unitofwork.DocumentItemRepository.Update(olditem);
                        unitofwork.Save();
                        Debug.WriteLine("olditem.Qty: "+olditem.Qty);
                    }
                }

            }

            //unitofwork.RequisitionOrderRepository.Update(rq);
            unitofwork.Save();
            //Debug.WriteLine(rq.DocumentItems.Count);

            //List<DocumentItem> vmList = new List<DocumentItem>();

            //vmList = ViewBag.RequisitionItems;
            //vmList = rcvm.DocumentItems;
            //RequisitionItemVM rcvm = new RequisitionItemVM;
            //Debug.WriteLine("vmlist.count: " + vmList.Count);

            //List<DocumentItem> doitems = new List<DocumentItem>();

            //List<DocumentItem> newitems = new List<DocumentItem>();
            // if (ModelState.IsValid)
            //{
            //foreach (DocumentItem di in vmList)
            //{
            // if (di.Qty != 0)
            // {
            //     Item item = unitofwork.ItemRepository.GetByID(di.Item.ID);
            //    DocumentItem doitem = unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);

            //   doitems.Add(doitem);
            //  unitofwork.Save();
            // }
            //}
            //Debug.WriteLine("doitems.count "+doitems.Count);

            //rq.DocumentItems = vmList;

            ///unitofwork.Save();
            //Debug.WriteLine(rq.DocumentItems.Count);

            //Debug.WriteLine("rq.ID:" + rq.ID);

            //Session["RequestList"] = null;

            return RedirectToAction("Index");
            //return View();
        }




        ////POST:Requisition/Remove
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Remove(Guid id)
        //{
        //    Debug.WriteLine(id);
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var unitofwork = new UnitOfWork();
        //    DocumentItem documentItem = unitofwork.DocumentItemRepository.Get(filter: x => x.ID == id, includeProperties: "Document").First();

        //    int roid = documentItem.Document.ID;
        //    unitofwork.DocumentItemRepository.Delete(id);
        //    unitofwork.Save();

        //    string URL = string.Format("/Requisition/Edit?id", roid);

        //    return Redirect(URL);

        //    ///return RedirectToAction("Edit", "Requisition");

        //}

        // POST: Requisition/Edit/4
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SelectedCategory,SelectedDescription,Quantity,ROID")] RequisitionItemVM rcvm)
        {
            var unitofwork = new UnitOfWork();
            Debug.WriteLine(rcvm.SelectedCategory + "\n" + rcvm.SelectedDescription + " x " + rcvm.Quantity.ToString());

            Debug.WriteLine(rcvm.ROID);
            // Debug.WriteLine(rcvm.RequisitionOrderID);
            if (ModelState.IsValid)
            {

                //get item by itemID
                Item item = unitofwork.ItemRepository.GetItembyDescrption(rcvm.SelectedDescription);
                Debug.WriteLine(item.ID);

                //save this item into the doucumentitem(item, qty)
                DocumentItem doitem = unitofwork.DocumentItemRepository.InsertDocumentItembyItemandQty(item, rcvm.Quantity);

                //doitem.Document.ID = rcvm.ROID;
                unitofwork.Save();
                Debug.WriteLine(doitem.ID);

                RequisitionOrder rq = new RequisitionOrder(unitofwork.StaffRepository.GetByID(10006));
                rq.DocumentItems.Add(unitofwork.DocumentItemRepository.GetByID(doitem.ID));
                rq.ID = rcvm.ROID;
                Debug.WriteLine(rq.ID);


                unitofwork.RequisitionOrderRepository.Insert(rq);
                Debug.WriteLine(rq.ID);

                unitofwork.Save();
                Debug.WriteLine(rq.ID);

                return RedirectToAction("Index");
            }

            return View(rcvm);
        }

        //GET: Requisition/Cancel/4
        public ActionResult Cancel(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var unitofwork = new UnitOfWork();

            var requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            requisitionOrder.Status = Models.Status.Cancelled;
            unitofwork.Save();

            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            return View(requisitionOrder);

        }

        //GET: Requisition/Approve/4
        public ActionResult Approve(int id,string comment)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debug.WriteLine("comment = " + comment);
            var unitofwork = new UnitOfWork();

            RequisitionOrder requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            requisitionOrder.Approve(loginService.StaffFromSession);
            requisitionOrder.Comments = comment;
            unitofwork.RequisitionOrderRepository.Update(requisitionOrder);
            unitofwork.Save();

            
            return View(requisitionOrder);
        }

        //GET: Requisition/Reject/4
        public ActionResult Reject(int id, string comment)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Debug.WriteLine("comment = " + comment);
            var unitofwork = new UnitOfWork();

            RequisitionOrder requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            requisitionOrder.Rejected(loginService.StaffFromSession);
            requisitionOrder.Comments = comment;
            unitofwork.RequisitionOrderRepository.Update(requisitionOrder);
            unitofwork.Save();


            return View(requisitionOrder);
        }

        public ActionResult Approvequick(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           
            var unitofwork = new UnitOfWork();

            RequisitionOrder requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            requisitionOrder.Approve(loginService.StaffFromSession);
           
            unitofwork.RequisitionOrderRepository.Update(requisitionOrder);
            unitofwork.Save();


            return RedirectToAction("Manage", "Requisition");
        }
        public ActionResult Rejectquick(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var unitofwork = new UnitOfWork();

            RequisitionOrder requisitionOrder = unitofwork.RequisitionOrderRepository.Get(filter: x => x.ID == id, includeProperties: "DocumentItems.Item").FirstOrDefault();
            if (requisitionOrder == null)
            {
                return HttpNotFound();
            }
            requisitionOrder.Rejected(loginService.StaffFromSession);

            unitofwork.RequisitionOrderRepository.Update(requisitionOrder);
            unitofwork.Save();


            return RedirectToAction("Manage", "Requisition");
        }



        //public ActionResult SendEmail()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult SendEmail(string receiver, string subject, string message)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var senderEmail = new MailAddress("logicssims@outlook.com", "Logic University SSIMS");
        //            var receiverEmail = new MailAddress(receiver, "Receiver");
        //            var password = "ss1msadm1np@sswOrd";
        //            var sub = subject;
        //            var body = message;
        //            var smtp = new SmtpClient
        //            {
        //                Host = "smtp.outlook.com",
        //                Port = 587,
        //                EnableSsl = true,
        //                DeliveryMethod = SmtpDeliveryMethod.Network,
        //                UseDefaultCredentials = false,
        //                Credentials = new NetworkCredential(senderEmail.Address, password)
        //            };
        //            using (var mess = new MailMessage(senderEmail, receiverEmail)
        //            {
        //                Subject = subject,
        //                Body = body
        //            })
        //            {
        //                smtp.Send(mess);
        //            }
        //            return View();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        ViewBag.Error = "Some Error";
        //    }
        //    return View();
        //}



    }
}
    


