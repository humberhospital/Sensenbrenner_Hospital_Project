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
    public class CareerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Retrieves a list of all career postings
        /// </summary>
        /// <returns>IEnumerable<Career></returns>
        /// <example>
        /// GET: api/CareerData/ListCareers
        /// </example>
        [HttpGet]
        public IEnumerable<Career> ListCareers()
        {
            return db.Careers;
        }

        /// <summary>
        /// Retrieves a career posting specified by its id
        /// </summary>
        /// <param name="id">Career posting id</param>
        /// <returns>Career posting object</returns>
        /// <example>
        /// GET: api/CareerData/GetCareer/5
        /// </example>
        [ResponseType(typeof(Career))]
        [HttpGet]
        public IHttpActionResult GetCareer(int id)
        {
            Career career = db.Careers.Find(id);

            if (career == null)
            {
                return NotFound();
            }

            return Ok(career);
        }

        /// <summary>
        /// Updates a career posting with the given form data
        /// </summary>
        /// <param name="id">Input career posting id</param>
        /// <param name="career">Career object, recieved as POST data</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/CareerData/UpdateCareer/5
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCareer(int id, [FromBody]Career career)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != career.CareerID)
            {
                return BadRequest();
            }

            db.Entry(career).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CareerExists(id))
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
        /// Adds a career posting to the database using form data
        /// </summary>
        /// <param name="career">Career object, recieved as POST data</param>
        /// <returns>status code, along with id associated with the new posting</returns>
        /// <example>
        /// POST: api/CareerData/AddCareer
        /// </example>
        [ResponseType(typeof(Career))]
        [HttpPost]
        public IHttpActionResult AddCareer([FromBody]Career career)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Careers.Add(career);
            db.SaveChanges();

            return Ok(career.CareerID);
        }

        /// <summary>
        /// Deletes a career posting associated with the specified id
        /// </summary>
        /// <param name="id">Career posting id</param>
        /// <returns></returns>
        /// <example>
        /// DELETE: api/CareerData/5
        /// </example>
        [ResponseType(typeof(Career))]
        [HttpPost]
        public IHttpActionResult DeleteCareer(int id)
        {
            Career career = db.Careers.Find(id);
            if (career == null)
            {
                return NotFound();
            }

            db.Careers.Remove(career);
            db.SaveChanges();

            return Ok(career);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CareerExists(int id)
        {
            return db.Careers.Count(e => e.CareerID == id) > 0;
        }
    }
}