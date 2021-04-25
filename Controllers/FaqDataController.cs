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

        [ResponseType(typeof(Faq))]
        [HttpPost]
        public IHttpActionResult AddFaq([FromBody]Faq newFaq)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Faqs.Add(newFaq);
            db.SaveChanges();

            return Ok(newFaq.FaqID);
        }

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
