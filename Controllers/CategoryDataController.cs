using SensenbrennerHospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SensenbrennerHospital.Controllers
{
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [ResponseType(typeof(Department))]
        [HttpGet]
        public IHttpActionResult GetCategoriesForFaq(int id)
        {
            IEnumerable<FaqCategory> faqCategories = db.FaqCategories.Where(c => c.FaqID == id);
            List<CategoryDto> categoryDtos = new List<CategoryDto>();

            foreach (var faqCategory in faqCategories)
            {
                CategoryDto NewCategory = new CategoryDto
                {
                    CategoryID = faqCategory.Category.CategoryID,
                    CategoryName = faqCategory.Category.CategoryName
                };
                categoryDtos.Add(NewCategory);
            }
            return Ok(categoryDtos);
        }
    }
}
