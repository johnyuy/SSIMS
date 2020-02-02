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
        private TenderRepository tenderRepository;
        private UserAccountRepository userAccountRepository;

        public UnitOfWork()
        {
            this.context = new DatabaseContext(); ;
        }

        public UnitOfWork(DatabaseContext context)
        {
            this.context = context;
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
            context.SaveChanges();
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
