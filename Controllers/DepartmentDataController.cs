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
    public class DepartmentDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        /// <summary>
        /// Gets a list of departments from the database along with a status code (200 OK).
        /// </summary>
        /// <returns>A list of departments including the department ID, name, and phone number.</returns>
        /// <example>
        /// GET: api/DepartmentData/ListDepartments
        /// </example>
        [ResponseType(typeof(IEnumerable<DepartmentDto>))]
        [HttpGet]
        public IHttpActionResult ListDepartments()
        {
            List<Department> departments = db.Departments.ToList();
            List<DepartmentDto> departmentDtos = new List<DepartmentDto>();

            foreach (var department in departments)
            {
                DepartmentDto NewDepartment = new DepartmentDto
                {
                    DepartmentID = department.DepartmentID,
                    DepartmentName = department.DepartmentName,
                    DepartmentPhoneNumber = department.DepartmentPhoneNumber
                };
                departmentDtos.Add(NewDepartment);
            }

            return Ok(departmentDtos);
        }

        /// <summary>
        /// Finds a specific department from the database with a 200 status code. If the department is not found returns a 404 status code.
        /// </summary>
        /// <param name="id">The department ID</param>
        /// <returns>Information about the department including department ID, name, and phone number</returns>
        /// <example>
        /// GET: api/DepartmentData/FindDepartment/3
        /// </example>
        [ResponseType(typeof(DepartmentDto))]
        [HttpGet]
        public IHttpActionResult FindDepartment(int id)
        {
            //Gets the data from the database
            Department department = db.Departments.Find(id);

            //Checks if department exists
            if (department == null)
            {
                //Returns a 404 not found status code
                return NotFound();
            }

            //Adds the data to the dto
            DepartmentDto SelectedDepartment = new DepartmentDto
            {
                DepartmentID = department.DepartmentID,
                DepartmentName = department.DepartmentName,
                DepartmentPhoneNumber = department.DepartmentPhoneNumber
            };

            //Returns status code 200 and the department dto
            return Ok(SelectedDepartment);
        }

        /// <summary>
        /// Adds a department to the database.
        /// </summary>
        /// <param name="newDepartment">A department object sent as a POST form data.</param>
        /// <returns>200 status code if successful and the newly created department's ID, 400 status code if not successful.</returns>
        /// <example>
        /// POST: api/DepartmentData/AddDepartment
        /// FORM DATA: Department JSON object
        /// </example>
        [ResponseType(typeof(Department))]
        [HttpPost]
        public IHttpActionResult AddDepartment([FromBody]Department newDepartment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Departments.Add(newDepartment);
            db.SaveChanges();

            return Ok(newDepartment.DepartmentID);
        }


        /// <summary>
        /// Deletes a department from the database
        /// </summary>
        /// <param name="id">Id of the department to delete.</param>
        /// <returns>200 status code if successful, 404 status code if not successful.</returns>
        /// <example>
        /// POST: api/DepartmentData/DeleteDepartment/3
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            return Ok();
        }

        /// <summary>
        /// Update a department in the database.
        /// </summary>
        /// <param name="id">The department ID</param>
        /// <param name="department">A department object sent as a POST form data.</param>
        /// <returns>200 status code if successful, 400 status code if not successful.</returns>
        /// <example>
        /// POST: api/DepartmentData/UpdateDepartment/1
        /// FORM DATA: Department JSON object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDepartment(int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != department.DepartmentID)
            {
                return BadRequest();
            }


            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
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
        /// Finds a department in the system.
        /// </summary>
        /// <param name="id">The department id</param>
        /// <returns>True if the department exists, false if it doesn't.</returns>
        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.DepartmentID == id) > 0;
        }

    }
}
