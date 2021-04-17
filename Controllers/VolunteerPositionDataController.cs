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

        /// <summary>
        /// Finds a particular volunteerposition in the database with a 200 status code. If the volunteerposition is not found, return 404.
        /// </summary>
        /// <param name="id">The Cvp.Id </param>
        /// <returns>Information about the volunteerposition, including volunteerposition, cvpId, name, description, and departmentid</returns>
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

        /// <summary>
        /// Finds a particular Department in the database given a volunteer position id, cvp, with a 200 status code. If the Department is not found, return 404.
        /// </summary>
        /// <param name="id">The volunteerposition id</param>
        /// <returns>Information about the Department, including Department id, Department name, department phone number</returns>
        // <example>
        // GET: api/DepartmentData/FindDepartmentForVolunteerPosition/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(DepartmentDto))]
        public IHttpActionResult FindDepartmentForVolunteerPosition(int id)
        {
            //Finds the first department which has any volunteerpositions
            //that match the input cvpid
            Department Department = db.Departments
                .Where(t => t.VolunteerPositions.Any(p => p.CvpID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Department == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            DepartmentDto DepartmentDto = new DepartmentDto
            {
                DepartmentID = Department.DepartmentID,
                DepartmentName = Department.DepartmentName,
                DepartmentPhoneNumber = Department.DepartmentPhoneNumber
            };


            //pass along data as 200 status code OK response
            return Ok(DepartmentDto);
        }



        /// <summary>
        /// Updates a volunteerposition in the database given information about the volunteerposition.
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

        // /// <summary>
        // /// Receives volunteerposition picture data, uploads it to the webserver and updates the volunteerposition's HasPic option
        // /// </summary>
        // /// <param name="id">the volunteerposition id</param>
        // /// <returns>status code 200 if successful.</returns>
        // /// <example>
        // /// curl -F articlepic=@file.jpg "https://localhost:xx/api/volunteerdata/updatevolunteerpic/2"
        // /// POST: api/VolunteerPositionData/UpdateVolunteerPic/3
        // /// HEADER: enctype=multipart/form-data
        // /// FORM-DATA: image
        // /// </example>
        // /// https://stackoverflow.com/questions/28369529/how-to-set-up-a-web-api-controller-for-multipart-form-data

        // [HttpPost]
        // public IHttpActionResult UpdateVolunteerPic(int id)
        // {

        //     bool haspic = false;
        //     string picextension;
        //     if (Request.Content.IsMimeMultipartContent())
        //     {
        //         Debug.WriteLine("Received multipart form data.");

        //         int numfiles = HttpContext.Current.Request.Files.Count;
        //         Debug.WriteLine("Files Received: " + numfiles);

        //         //Check if a file is posted
        //         if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
        //         {
        //             var VolunteerPic = HttpContext.Current.Request.Files[0];
        //             //Check if the file is empty
        //             if (VolunteerPic.ContentLength > 0)
        //             {
        //                 var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
        //                 var extension = Path.GetExtension(VolunteerPic.FileName).Substring(1);
        //                 //Check the extension of the file
        //                 if (valtypes.Contains(extension))
        //                 {
        //                     try
        //                     {
        //                         //file name is the id of the image
        //                         string fn = id + "." + extension;

        //                         //get a direct file path to ~/Content/VolunteerPositions/{id}.{extension}
        //                         string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/VolunteerPositions/"), fn);

        //                         //save the file
        //                         VolunteerPic.SaveAs(path);

        //                         //if these are all successful then we can set these fields
        //                         haspic = true;
        //                         picextension = extension;

        //                         //Update the volunteerposition haspic and picextension fields in the database
        //                         VolunteerPosition SelectedVolunteer = db.VolunteerPositions.Find(id);
        //                         SelectedVolunteer.VolunteerHasPic = haspic;
        //                         SelectedVolunteer.PicExtension = extension;
        //                         db.Entry(SelectedVolunteer).State = EntityState.Modified;

        //                         db.SaveChanges();

        //                     }
        //                     catch (Exception ex)
        //                     {
        //                         Debug.WriteLine("VolunteerPosition Image was not saved successfully.");
        //                         Debug.WriteLine("Exception:" + ex);
        //                     }
        //                 }
        //             }

        //         }
        //     }

        //     return Ok();
        // }


        /// <summary>
        /// Adds a volunteerposition to the database.
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
        /// Deletes a volunteerposition in the database
        /// </summary>
        /// <param name="id">The id of the volunteerposition to delete.</param>
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
            //also delete image from path
            string path = HttpContext.Current.Server.MapPath("~/Content/VolunteerPositions/" + id + volunteerposition.PicExtension);
            if (System.IO.File.Exists(path))
            {
                Debug.WriteLine("File exists... preparing to delete!");
                System.IO.File.Delete(path);
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
        /// Finds a volunteerposition in the system. Internal use only.
        /// </summary>
        /// <param name="id">The volunteerposition id, Cvp</param>
        /// <returns>TRUE if the volunteerposition exists, false otherwise.</returns>
        private bool VolunteerPositionExists(int id)
        {
            return db.VolunteerPositions.Count(e => e.CvpID == id) > 0;
        }
    }

}
