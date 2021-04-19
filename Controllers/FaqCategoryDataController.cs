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
    public class FaqCategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public IHttpActionResult AddFaqCategory(int faqID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            //db.FaqCategories.Add();
            db.SaveChanges();

            return Ok();
        }
    }
}
