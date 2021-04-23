using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
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

        /// <summary>
        /// Returns a list of all appointment bookings in database along with a status code
        /// </summary>
        /// <returns>IEnumerable<AppointmentBookingDto></returns>
        /// <example>
        /// GET: api/AppointmentBookingData/ListAppointmentBookings
        /// </example>
        [ResponseType(typeof(IEnumerable<AppointmentBookingDto>))]
        [HttpGet]
        public IHttpActionResult ListAppointmentBookings()
        {
            List<AppointmentBooking> appointmentBookings = db.AppointmentBookings.ToList();
            List<AppointmentBookingDto> appointmentBookingDtos = new List<AppointmentBookingDto> { };

            foreach(var appointmentBooking in appointmentBookings)
            {
                AppointmentBookingDto appointmentBookingDto = new AppointmentBookingDto
                {
                    AppointmentID = appointmentBooking.AppointmentID,
                    FirstName = appointmentBooking.FirstName,
                    LastName = appointmentBooking.LastName,
                    PhoneNumber = appointmentBooking.PhoneNumber,
                    DoctorID = appointmentBooking.DoctorID,
                    RequestDescription = appointmentBooking.RequestDescription,
                    AppointmentDate = appointmentBooking.AppointmentDate
                };
                appointmentBookingDtos.Add(appointmentBookingDto);
            }
            return Ok(appointmentBookingDtos);
        }
        /// <summary>
        /// Finds an appointment booking associated with a particular id, along with status code
        /// </summary>
        /// <param name="id">Input appointment id</param>
        /// <returns>Appointment booking associated with id</returns>
        /// <example>
        /// GET: api/AppointmentBookingData/GetAppointmentBooking/5
        /// </example>
        [ResponseType(typeof(AppointmentBooking))]
        [HttpGet]
        public IHttpActionResult GetAppointmentBooking(int id)
        {
            AppointmentBooking appointmentBooking = db.AppointmentBookings.Find(id);
            if (appointmentBooking == null)
            {
                return NotFound();
            }

            return Ok(appointmentBooking);
        }
        /// <summary>
        /// Finds all appointment associated with a doctor, along with a status code
        /// </summary>
        /// <param name="id">Input doctor id</param>
        /// <returns>List of appointments associated with a doctor</returns>
        /// <example>
        /// GET: api/AppointmentBookingData/GetAppointmentsForDoctor/2
        /// </example>
        [ResponseType(typeof(IEnumerable<AppointmentBookingDto>))]
        [HttpGet]
        public IHttpActionResult GetAppointmentsForDoctor(int id)
        {
            List<AppointmentBooking> appointmentBookings = db
                .AppointmentBookings
                .Where(a => a.DoctorID == id)
                .ToList();
            List<AppointmentBookingDto> appointmentBookingDtos = new List<AppointmentBookingDto> { };

            foreach(var appointment in appointmentBookings)
            {
                AppointmentBookingDto appointmentBookingDto = new AppointmentBookingDto
                {
                    AppointmentID = appointment.AppointmentID,
                    AppointmentDate = appointment.AppointmentDate,
                    FirstName = appointment.FirstName,
                    LastName = appointment.LastName,
                    PhoneNumber = appointment.PhoneNumber,
                    RequestDescription = appointment.RequestDescription,
                    Confirmation = appointment.Confirmation,
                    DoctorID = appointment.DoctorID
                };
                appointmentBookingDtos.Add(appointmentBookingDto);
            }
            return Ok(appointmentBookingDtos);
        }

        /// <summary>
        /// Updates a specific appointment booking in the database when provided with an appointment ID
        /// </summary>
        /// <param name="id">Input appointment id</param>
        /// <param name="appointmentBooking">Edited appointment booking object</param>
        /// <returns>HTTP status code</returns>
        /// <example>
        /// POST: api/AppointmentBookingData/UpdateAppointmentBooking/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateAppointmentBooking(int id, [FromBody]AppointmentBooking appointmentBooking)
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

        /// <summary>
        /// Adds an appointment to the database
        /// </summary>
        /// <param name="appointmentBooking">Input appointment booking data from form</param>
        /// <returns>Status code</returns>
        /// <example>
        /// POST: api/AppointmentBookingData/AddAppointmentBooking
        /// </example>
        [ResponseType(typeof(AppointmentBooking))]
        [HttpPost]
        public IHttpActionResult AddAppointmentBooking([FromBody]AppointmentBooking appointmentBooking)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Debug.WriteLine(appointmentBooking);
            db.AppointmentBookings.Add(appointmentBooking);
            db.SaveChanges();

            return Ok(appointmentBooking.AppointmentID);
        }

        /// <summary>
        /// Deletes specified appointment booking
        /// </summary>
        /// <param name="id">Input appointment booking id</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/AppointmentBookingData/DeleteAppointmentBooking/5
        /// </example>
        [ResponseType(typeof(AppointmentBooking))]
        [HttpPost]
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