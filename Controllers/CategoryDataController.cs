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

        //[ResponseType(typeof(CategoryDto))]
        //[HttpGet]
        //public IHttpActionResult GetCategoriesForFaq(int id)
        //{
        //    IEnumerable<FaqCategory> faqCategories = db.FaqCategories.Where(c => c.FaqID == id).ToList();
        //    List<CategoryDto> categoryDtos = new List<CategoryDto>();

        //    foreach (var faqCategory in faqCategories)
        //    {
        //        CategoryDto NewCategory = new CategoryDto
        //        {
        //            CategoryID = faqCategory.Category.CategoryID,
        //            CategoryName = faqCategory.Category.CategoryName
        //        };
        //        categoryDtos.Add(NewCategory);
        //    }
        //    return Ok(categoryDtos);
        //}

        [ResponseType(typeof(IEnumerable<CategoryDto>))]
        [HttpGet]
        public IHttpActionResult GetCategories()
        {
            List<Category> categories = db.Categories.ToList();
            List<CategoryDto> categoryDtos = new List<CategoryDto>();

            foreach (var category in categories)
            {
                CategoryDto NewCategory = new CategoryDto
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.CategoryName
                };
                categoryDtos.Add(NewCategory);
            }
            return Ok(categoryDtos);
        }
    }
}
