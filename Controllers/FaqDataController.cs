using SensenbrennerHospital.Models;
using SensenbrennerHospital.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SensenbrennerHospital.Controllers
{
    public class FaqDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Gets a list of faqs from the database along with a status code (200 OK).
        /// </summary>
        /// <returns>A list of faqs including the faq ID, question, answer, and category</returns>
        /// <example>
        /// GET: api/Faq/ListFaqs
        /// </example>
        [ResponseType(typeof(IEnumerable<FaqDto>))]
        [HttpGet]
        public IHttpActionResult ListFaqs()
        {
            List<Faq> faqs = db.Faqs.ToList();
            List<FaqDto> faqDtos = new List<FaqDto>();

            foreach (var item in faqs)
            {
                FaqDto NewFaq = new FaqDto
                {
                    FaqID = item.FaqID,
                    Answer = item.Answer,
                    Question = item.Question,
                    CategoryID = item.CategoryID
                };
                faqDtos.Add(NewFaq);
            }

            return Ok(faqDtos);
        }

        /// <summary>
        /// Adds a faq to the database.
        /// </summary>
        /// <param name="newFaq">A faq object sent as a POST form data.</param>
        /// <returns>200 status code if successful, 400 status code if not successful.</returns>
        /// <example>
        /// POST: api/FaqData/AddFaq
        /// FORM DATA: Faq JSON object
        /// </example>
        [HttpPost]
        public IHttpActionResult AddFaq([FromBody]Faq newFaq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Faqs.Add(newFaq);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Finds a specific faq from the database with a 200 status code. If the faq is not found returns a 404 status code.
        /// </summary>
        /// <param name="id">The faq ID</param>
        /// <returns>Information about the faq including faq ID, question, answer, and categoryID</returns>
        /// <example>
        /// GET: api/FaqData/FindFaq/3
        /// </example>
        [ResponseType(typeof(FaqDto))]
        [HttpGet]
        public IHttpActionResult FindFaq(int id)
        {
            Faq faq = db.Faqs.Find(id);

            if (faq == null)
            {
                return NotFound();
            }

            FaqDto selectedFaq = new FaqDto
            {
                FaqID = faq.FaqID,
                Answer = faq.Answer,
                Question = faq.Question,
                CategoryID = faq.CategoryID
            };

            return Ok(selectedFaq);
        }

        /// <summary>
        /// Update a faq in the database.
        /// </summary>
        /// <param name="id">The faq ID</param>
        /// <param name="faq">A faq object sent as a POST form data.</param>
        /// <returns>200 status code if successful, 400 status code if not successful.</returns>
        /// <example>
        /// POST: api/FaqData/UpdateFaq/1
        /// FORM DATA: Faq JSON object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateFaq(int id, [FromBody] Faq faq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id != faq.FaqID )
            {
                return BadRequest();
            }

            db.Entry(faq).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FaqExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        /// <summary>
        /// Deletes a faq from the database
        /// </summary>
        /// <param name="id">Id of the faq to delete.</param>
        /// <returns>200 status code if successful, 404 status code if not successful.</returns>
        /// <example>
        /// POST: api/FaqData/DeleteFaq/3
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteFaq(int id)
        {
            Faq faq = db.Faqs.Find(id);
            if (faq == null)
            {
                return NotFound();
            }

            db.Faqs.Remove(faq);
            db.SaveChanges();
            return Ok();
        }

        /// <summary>
        /// Gets a list of faqs from the database based on the category ID
        /// </summary>
        /// <param name="id">Id of the category to search with</param>
        /// <returns>List of faqs associated with specified catgory</returns>
        /// <example>
        /// GET: api/FaqData/GetFaqsByCategoryId/6
        /// </example>
        [ResponseType(typeof(FaqDto))]
        [HttpGet]
        public IHttpActionResult GetFaqsByCategoryId(int id)
        {
            List<Faq> faqList = db.Faqs.Where(f => f.CategoryID == id).ToList();
            List<FaqDto> faqDtos = new List<FaqDto>();
            if (faqList == null)
            {
                return NotFound();
            }

            foreach (var item in faqList)
            {
                FaqDto NewFaq = new FaqDto
                {
                    FaqID = item.FaqID,
                    Answer = item.Answer,
                    Question = item.Question,
                    CategoryID = item.CategoryID
                };
                faqDtos.Add(NewFaq);
            }

            return Ok(faqDtos);
        }

        /// <summary>
        /// Deletes faqs based on specified category Id
        /// </summary>
        /// <param name="id">Category ID used in the search</param>
        /// <example>
        /// GET: api/FaqData/DeleteFaqsByCategoryId/6
        /// </example>
        [HttpGet]
        public IHttpActionResult DeleteFaqsByCategoryId(int id)
        {
            List<Faq> faqList = db.Faqs.Where(c => c.CategoryID == id).ToList();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            foreach (var item in faqList)
            {
                db.Faqs.Remove(item);
            }
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Finds a faq in the system.
        /// </summary>
        /// <param name="id">The faq id</param>
        /// <returns>True if the faq exists, false if it doesn't.</returns>
        private bool FaqExists(int id)
        {
            return db.Faqs.Count(e => e.FaqID == id) > 0;
        }
    }
}
