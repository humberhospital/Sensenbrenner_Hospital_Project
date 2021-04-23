using System;
using System.IO;
using System.Web;
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
using System.Diagnostics;

namespace SensenbrennerHospital.Controllers
{
    public class VolunteerPositionDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>Needs more
        /// Gets a list or volunteerpositions in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of volunteerpositions including ..........</returns>
        /// <example>
        /// GET: api/VolunteerPositionData/GetVolunteerPositions
        /// </example>
        [ResponseType(typeof(IEnumerable<VolunteerPositionDto>))]
        public IHttpActionResult GetVolunteerPositions()
        {
            List<VolunteerPosition> VolunteerPositions = db.VolunteerPositions.ToList();
            List<VolunteerPositionDto> VolunteerPositionDtos = new List<VolunteerPositionDto> { };


            foreach (var VolunteerPosition in VolunteerPositions)
            {
                VolunteerPositionDto NewVolunteerPosition = new VolunteerPositionDto
                {
                    CvpID = VolunteerPosition.CvpID,
                    Name = VolunteerPosition.Name,
                    Description = VolunteerPosition.Description,
                    DepartmentID = VolunteerPosition.DepartmentID
                };
                VolunteerPositionDtos.Add(NewVolunteerPosition);
            }

            return Ok(VolunteerPositionDtos);
        }

        /// <summary> Needs more...
        /// Finds a particular volunteer position in the database with a 200 status code. If the volunteer position is not found, return 404.
        /// </summary>
        /// <param name="id">The volunteer position id, CvpID </param>
        /// <returns>Information about the volunteer position, including the cvpId, name, description, and departmentid</returns>
        // <example>
        // GET: api/VolunteerPositionData/FindVolunteerPosition/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(VolunteerPositionDto))]
        public IHttpActionResult FindVolunteerPosition(int id)
        {
            //Find the data
            VolunteerPosition VolunteerPosition = db.VolunteerPositions.Find(id);
            //if not found, return 404 status code.
            if (VolunteerPosition == null)
            {
                return NotFound();
            }


            VolunteerPositionDto VolunteerPositionDto = new VolunteerPositionDto
            {
                CvpID = VolunteerPosition.CvpID,
                Name = VolunteerPosition.Name,
                Description = VolunteerPosition.Description,
                DepartmentID = VolunteerPosition.DepartmentID
            };


            //pass along data as 200 status code OK response
            return Ok(VolunteerPositionDto);
        }

        ///// <summary> I might need this?
        ///// Finds a particular Department in the database given a volunteer position id, cvp, with a 200 status code. If the Department is not found, return 404.
        ///// </summary>
        ///// <param name="id">The volunteerposition id</param>
        ///// <returns>Information about the Department, including Department id, Department name, department phone number</returns>
        //// <example>
        //// GET: api/DepartmentData/FindDepartmentForVolunteerPosition/5
        //// </example>
        //[HttpGet]
        //[ResponseType(typeof(DepartmentDto))]
        //public IHttpActionResult FindDepartmentForVolunteerPosition(int id)
        //{
        //    //Finds the first department which has any volunteerpositions
        //    //that match the input cvpid
        //    Department Department = db.Departments
        //        .Where(t => t.VolunteerPositions.Any(p => p.CvpID == id))
        //        .FirstOrDefault();
        //    //if not found, return 404 status code.
        //    if (Department == null)
        //    {
        //        return NotFound();
        //    }

        //    //put into a 'friendly object format'
        //    DepartmentDto DepartmentDto = new DepartmentDto
        //    {
        //        DepartmentID = Department.DepartmentID,
        //        DepartmentName = Department.DepartmentName,
        //        DepartmentPhoneNumber = Department.DepartmentPhoneNumber
        //    };


        //    //pass along data as 200 status code OK response
        //    return Ok(DepartmentDto);
        //}



        /// <summary>
        /// Updates a volunteer position in the database given information about the volunteer position.
        /// </summary>
        /// <param name="id">The volunteerposition id, CVP</param>
        /// <param name="volunteerposition">A volunteerposition object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/VolunteerPositionData/UpdateVolunteerPosition/5
        /// FORM DATA: VolunteerPosition JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateVolunteerPosition(int id, [FromBody] VolunteerPosition volunteerposition)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != volunteerposition.CvpID)
            {
                return BadRequest();
            }


            db.Entry(volunteerposition).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerPositionExists(id))
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

        /// <summary>
        /// Adds a volunteer position to the database.
        /// </summary>
        /// <param name="volunteerposition">A volunteerposition object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/VolunteerPositionData/AddVolunteerPosition
        ///  FORM DATA: VolunteerPosition JSON Object
        /// </example>
        [ResponseType(typeof(VolunteerPosition))]
        [HttpPost]
        public IHttpActionResult AddVolunteerPosition([FromBody] VolunteerPosition volunteerposition)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.VolunteerPositions.Add(volunteerposition);
            db.SaveChanges();

            return Ok(volunteerposition.CvpID);
        }

        /// <summary>
        /// Deletes a volunteer position in the database
        /// </summary>
        /// <param name="id">The id of the volunteer position to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/VolunteerPositionData/DeleteVolunteerPosition/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteVolunteerPosition(int id)
        {
            VolunteerPosition volunteerposition = db.VolunteerPositions.Find(id);
            if (volunteerposition == null)
            {
                return NotFound();
            }

            db.VolunteerPositions.Remove(volunteerposition);
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
        /// Finds a volunteer position in the system. Internal use only.
        /// </summary>
        /// <param name="id">The volunteerposition id, Cvp</param>
        /// <returns>TRUE if the volunteerposition exists, false otherwise.</returns>
        private bool VolunteerPositionExists(int id)
        {
            return db.VolunteerPositions.Count(e => e.CvpID == id) > 0;
        }
    }

}
