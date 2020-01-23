﻿using System;
using SSIMS.Models;
using SSIMS.Database;

namespace SSIMS.DAL
{
    public class UnitOfWork : IDisposable
    {
        private DatabaseContext context = new DatabaseContext();
        private CollectionPointRepository collectionPointRepository;
        private ItemRepository itemRepository;
        private DepartmentRepository departmentRepository;
        private GenericRepository<InventoryItem> inventoryItemRepository;


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