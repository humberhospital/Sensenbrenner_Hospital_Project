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
using SensenbrennerHospital.Models;

namespace SensenbrennerHospital.Controllers
{
    public class AppointmentBookingDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/AppointmentBookingData
        public IQueryable<AppointmentBooking> GetAppointmentBookings()
        {
            return db.AppointmentBookings;
        }

        // GET: api/AppointmentBookingData/5
        [ResponseType(typeof(AppointmentBooking))]
        public IHttpActionResult GetAppointmentBooking(int id)
        {
            AppointmentBooking appointmentBooking = db.AppointmentBookings.Find(id);
            if (appointmentBooking == null)
            {
                return NotFound();
            }

            return Ok(appointmentBooking);
        }

        // PUT: api/AppointmentBookingData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppointmentBooking(int id, AppointmentBooking appointmentBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appointmentBooking.AppointmentID)
            {
                return BadRequest();
            }

            db.Entry(appointmentBooking).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentBookingExists(id))
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

        // POST: api/AppointmentBookingData
        [ResponseType(typeof(AppointmentBooking))]
        public IHttpActionResult PostAppointmentBooking(AppointmentBooking appointmentBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AppointmentBookings.Add(appointmentBooking);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = appointmentBooking.AppointmentID }, appointmentBooking);
        }

        // DELETE: api/AppointmentBookingData/5
        [ResponseType(typeof(AppointmentBooking))]
        public IHttpActionResult DeleteAppointmentBooking(int id)
        {
            AppointmentBooking appointmentBooking = db.AppointmentBookings.Find(id);
            if (appointmentBooking == null)
            {
                return NotFound();
            }

            db.AppointmentBookings.Remove(appointmentBooking);
            db.SaveChanges();

            return Ok(appointmentBooking);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppointmentBookingExists(int id)
        {
            return db.AppointmentBookings.Count(e => e.AppointmentID == id) > 0;
        }
    }
}