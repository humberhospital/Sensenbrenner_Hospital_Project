using SensenbrennerHospital.Models;
using SensenbrennerHospital.Models.ViewModels;
using System;
using System.Collections.Generic;
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
                    Question = item.Question
                };
                faqDtos.Add(NewFaq);
            }

            return Ok(faqDtos);
        }

        [ResponseType(typeof(Faq))]
        [HttpPost]
        public IHttpActionResult AddFaq([FromBody]Faq newFaq)
        {
            Debug.WriteLine(newFaq);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Faqs.Add(newFaq);
            db.SaveChanges();

            return Ok(newFaq.FaqID);
        }
    }
}
