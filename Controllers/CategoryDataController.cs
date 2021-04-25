using SensenbrennerHospital.Models;
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
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [ResponseType(typeof(CategoryDto))]
        [HttpGet]
        public IHttpActionResult FindCategory(int id)
        {
            Debug.WriteLine(id);
            Category category = db.Categories.Find(id);

            CategoryDto categoryDto = new CategoryDto
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName
            };

            return Ok(categoryDto);
        }

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

        [HttpPost]
        public IHttpActionResult CreateCategory([FromBody]Category NewCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(NewCategory);
            db.SaveChanges();
            
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            return Ok();
        }

        public IHttpActionResult UpdateCategory(int id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Finds a category in the system.
        /// </summary>
        /// <param name="id">The category id</param>
        /// <returns>True if the category exists, false if it doesn't.</returns>
        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryID == id) > 0;
        }
    }
}
