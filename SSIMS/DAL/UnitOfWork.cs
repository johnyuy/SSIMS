using System;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private DatabaseContext context;
        private CollectionPointRepository collectionPointRepository;
        private ItemRepository itemRepository;
        private DepartmentRepository departmentRepository;
        private GenericRepository<InventoryItem> inventoryItemRepository;
        private PurchaseOrderRepository purchaseOrderRepository;
        private StaffRepository staffRepository;
        private RequisitionOrderRepository requisitionOrderRepository;
        private DocumentItemRepository documentItemRepository;
        private SupplierRepository supplierRepository;
        private RetrievalListRepository retrievalListRepository;
        private UserAccountRepository userAccountRepository;
        private TransactionItemRepository transactionItemRepository;
        private DisbursementListRepository disbursementListRepository;
        private TenderRepository tenderRepository;
        private PurchaseItemRepository purchaseItemRepository;
        private DeliveryOrderRepository deliveryOrderRepository;
        private StockCardEntryRepository stockCardEntryRepository;
        private AdjustmentVoucherRepository adjustmentVoucherRepository;

        public UnitOfWork()
        {
            this.context = new DatabaseContext(); ;
        }

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
        }

        public AdjustmentVoucherRepository AdjustmentVoucherRepository
        {
            get
            {
                if(this.adjustmentVoucherRepository == null)
                {
                    this.adjustmentVoucherRepository = new AdjustmentVoucherRepository(context);
                }
                return adjustmentVoucherRepository;
            }
        }

        public StockCardEntryRepository StockCardEntryRepository
        {
            get
            {

                if (this.stockCardEntryRepository == null)
                {
                    this.stockCardEntryRepository = new StockCardEntryRepository(context);
                }
                return stockCardEntryRepository;
            }
        }

        public DeliveryOrderRepository DeliveryOrderRepository
        {
            get
            {

                if (this.deliveryOrderRepository == null)
                {
                    this.deliveryOrderRepository = new DeliveryOrderRepository(context);
                }
                return deliveryOrderRepository;
            }
        }


        public PurchaseItemRepository PurchaseItemRepository
        {
            get
            {

                if (this.purchaseItemRepository == null)
                {
                    this.purchaseItemRepository = new PurchaseItemRepository(context);
                }
                return purchaseItemRepository;
            }
        }

        public DisbursementListRepository DisbursementListRepository
        {
            get
            {

                if (this.disbursementListRepository == null)
                {
                    this.disbursementListRepository = new DisbursementListRepository(context);
                }
                return disbursementListRepository;
            }
        }

        public TransactionItemRepository TransactionItemRepository
        {
            get
            {

                if (this.transactionItemRepository == null)
                {
                    this.transactionItemRepository = new TransactionItemRepository(context);
                }
                return transactionItemRepository;
            }
        }

        public UserAccountRepository UserAccountRepository
        {
            get
            {

                if (this.userAccountRepository == null)
                {
                    this.userAccountRepository = new UserAccountRepository(context);
                }
                return userAccountRepository;
            }
        }

        public TenderRepository TenderRepository
        {
            get
            {

                if (this.tenderRepository == null)
                {
                    this.tenderRepository = new TenderRepository(context);
                }
                return tenderRepository;
            }
        }

        public SupplierRepository SupplierRepository
        {
            get
            {

                if (this.supplierRepository == null)
                {
                    this.supplierRepository = new SupplierRepository(context);
                }
                return supplierRepository;
            }
        }

        public RetrievalListRepository RetrievalListRepository
        {
            get
            {

                if (this.retrievalListRepository == null)
                {
                    this.retrievalListRepository = new RetrievalListRepository(context);
                }
                return retrievalListRepository;
            }
        }

        public StaffRepository StaffRepository
        {
            get
            {

                if (this.staffRepository == null)
                {
                    this.staffRepository = new StaffRepository(context);
                }
                return staffRepository;
            }
        }

        public PurchaseOrderRepository PurchaseOrderRepository
        {
            get
            {

                if (this.purchaseOrderRepository == null)
                {
                    this.purchaseOrderRepository = new PurchaseOrderRepository(context);
                }
                return purchaseOrderRepository;
            }
        }

        public CollectionPointRepository CollectionPointRepository
        {
            get
            {

                if (this.collectionPointRepository == null)
                {
                    this.collectionPointRepository = new CollectionPointRepository(context);
                }
                return collectionPointRepository;
            }
        }

        public DepartmentRepository DepartmentRepository
        {
            get
            {

                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new DepartmentRepository(context);
                }
                return departmentRepository;
            }
        }

        public ItemRepository ItemRepository
        {
            get
            {

                if (this.itemRepository == null)
                {
                    this.itemRepository = new ItemRepository(context);
                }
                return itemRepository;
            }
        }

        public GenericRepository<InventoryItem> InventoryItemRepository
        {
            get
            {

                if (this.inventoryItemRepository == null)
                {
                    this.inventoryItemRepository = new GenericRepository<InventoryItem>(context);
                }
                return inventoryItemRepository;
            }
        }

        public RequisitionOrderRepository RequisitionOrderRepository
        {
            get
            {

                if (this.requisitionOrderRepository == null)
                {
                    this.requisitionOrderRepository = new RequisitionOrderRepository(context);
                }
                return requisitionOrderRepository;
            }
        }

        public DocumentItemRepository DocumentItemRepository
        {
            get
            {

                if (this.documentItemRepository == null)
                {
                    this.documentItemRepository = new DocumentItemRepository(context);
                }
                return documentItemRepository;
            }
        }

        public void Save()
        {
            try {
                context.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
