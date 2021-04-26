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

        /// <summary>
        /// Finds a specific category from the database with a 200 status code. If the category is not found returns a 404 status code.
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <returns>Information about the category including category ID, and name. </returns>
        /// <example>
        /// GET: api/CategoryData/FindCategory/3
        /// </example>
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

        /// <summary>
        /// Gets a list of categories from the database along with a status code (200 OK).
        /// </summary>
        /// <returns>A list of categories including the category ID, and name. </returns>
        /// <example>
        /// GET: api/CategoryData/ListCategories
        /// </example>
        [ResponseType(typeof(IEnumerable<CategoryDto>))]
        [HttpGet]
        public IHttpActionResult ListCategories()
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

        /// <summary>
        /// Adds a category to the database.
        /// </summary>
        /// <param name="NewCategory">A category object sent as a POST form data.</param>
        /// <returns>200 status code if successful, 400 status code if not successful.</returns>
        /// <example>
        /// POST: api/CategoryData/AddCategory
        /// FORM DATA: Category JSON object
        /// </example>
        [HttpPost]
        public IHttpActionResult AddCategory([FromBody]Category NewCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(NewCategory);
            db.SaveChanges();
            
            return Ok();
        }

        /// <summary>
        /// Deletes a category from the database
        /// </summary>
        /// <param name="id">Id of the category to delete.</param>
        /// <returns>200 status code if successful, 404 status code if not successful.</returns>
        /// <example>
        /// POST: api/CategoryData/DeleteCategory/3
        /// </example>
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

        /// <summary>
        /// Update a category in the database.
        /// </summary>
        /// <param name="id">The category ID</param>
        /// <param name="department">A category object sent as a POST form data.</param>
        /// <returns>200 status code if successful, 400 status code if not successful.</returns>
        /// <example>
        /// POST: api/CategoryData/UpdateCategory/1
        /// FORM DATA: Category JSON object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
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
