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
        /// <returns></returns>
        // GET: api/CareerData/ListCareers
        [HttpGet]
        public IEnumerable<Career> ListCareers()
        {
            return db.Careers;
        }

        /// <summary>
        /// Retrieves a specific career posting by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/CareerData/GetCareer/5
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

        // POST: api/CareerData/UpdateCareer/5
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

        // POST: api/CareerData/AddCareer
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

        // DELETE: api/CareerData/5
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