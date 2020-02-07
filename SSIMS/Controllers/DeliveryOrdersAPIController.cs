using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SSIMS.Database;
using SSIMS.Models;
using System.Diagnostics;

namespace SSIMS.Controllers
{

    public class DeliveryOrdersAPIController : ApiController
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: api/DeliveryOrdersAPI
        public IQueryable<DeliveryOrder> GetDeliveryOrders()
        {
            Debug.WriteLine("In GetDeliveryOrders");
            return db.DeliveryOrders;
        }

        // GET: api/DeliveryOrdersAPI/5
        [ResponseType(typeof(DeliveryOrder))]
        public IHttpActionResult GetDeliveryOrder(int id)
        {
            DeliveryOrder deliveryOrder = db.DeliveryOrders.Find(id);
            if (deliveryOrder == null)
            {
                return NotFound();
            }

            return Ok(deliveryOrder);
        }

        // PUT: api/DeliveryOrdersAPI/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDeliveryOrder(int id, DeliveryOrder deliveryOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != deliveryOrder.ID)
            {
                return BadRequest();
            }

            db.Entry(deliveryOrder).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/DeliveryOrdersAPI
        [ResponseType(typeof(DeliveryOrder))]
        public IHttpActionResult PostDeliveryOrder(DeliveryOrder deliveryOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DeliveryOrders.Add(deliveryOrder);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = deliveryOrder.ID }, deliveryOrder);
        }

        // DELETE: api/DeliveryOrdersAPI/5
        [ResponseType(typeof(DeliveryOrder))]
        public IHttpActionResult DeleteDeliveryOrder(int id)
        {
            DeliveryOrder deliveryOrder = db.DeliveryOrders.Find(id);
            if (deliveryOrder == null)
            {
                return NotFound();
            }

            db.DeliveryOrders.Remove(deliveryOrder);
            db.SaveChanges();

            return Ok(deliveryOrder);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeliveryOrderExists(int id)
        {
            return db.DeliveryOrders.Count(e => e.ID == id) > 0;
        }
    }
}