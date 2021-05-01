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
    public class VolunteerDataController : ApiController
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>Needs more...
        /// Gets a list or volunteers in the database alongside a status code (200 OK).
        /// </summary>
        /// <returns>A list of volunteers including ..........</returns>
        /// <example>
        /// GET: api/VolunteerData/GetVolunteers
        /// </example>
        [ResponseType(typeof(IEnumerable<VolunteerDto>))]
        public IHttpActionResult GetVolunteers()
        {
            List<Volunteer> Volunteers = db.Volunteers.ToList();
            List<VolunteerDto> VolunteerDtos = new List<VolunteerDto> { };


            foreach (var Volunteer in Volunteers)
            {
                VolunteerDto NewVolunteer = new VolunteerDto
                {
                    VolunteerID = Volunteer.VolunteerID,
                    FirstName = Volunteer.FirstName,
                    LastName = Volunteer.LastName,
                    VolunteerHasPic = Volunteer.VolunteerHasPic,
                    PicExtension = Volunteer.PicExtension,
                    DepartmentID = Volunteer.DepartmentID
                };
                VolunteerDtos.Add(NewVolunteer);
            }

            return Ok(VolunteerDtos);
        }

        /// <summary>Needs more...
        /// Finds a particular volunteer in the database with a 200 status code. If the volunteer is not found, return 404.
        /// </summary>
        /// <param name="id">The volunteer id</param>
        /// <returns>Information about the volunteer, including volunteer id, first and last name, and departmentid</returns>
        // <example>
        // GET: api/VolunteerData/FindVolunteer/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(VolunteerDto))]
        public IHttpActionResult FindVolunteer(int id)
        {
            //Find the data
            Volunteer Volunteer = db.Volunteers.Find(id);
            //if not found, return 404 status code.
            if (Volunteer == null)
            {
                return NotFound();
            }


            VolunteerDto VolunteerDto = new VolunteerDto
            {
                VolunteerID = Volunteer.VolunteerID,
                FirstName = Volunteer.FirstName,
                LastName = Volunteer.LastName,
                DepartmentID = Volunteer.DepartmentID,
                VolunteerHasPic = Volunteer.VolunteerHasPic,
                PicExtension = Volunteer.PicExtension

            };


            //pass along data as 200 status code OK response
            return Ok(VolunteerDto);
        }



        /// <summary> ADD MORE...
        /// Updates a volunteer in the database given information about the volunteer.
        /// </summary>
        /// <param name="id">The volunteer id</param>
        /// <param name="volunteer">A volunteer object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/VolunteerData/UpdateVolunteer/5
        /// FORM DATA: Volunteer JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateVolunteer(int id, [FromBody] Volunteer volunteer)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != volunteer.VolunteerID)
            {
                return BadRequest();
            }


            db.Entry(volunteer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VolunteerExists(id))
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
        /// Receives volunteer picture data, uploads it to the webserver and updates the volunteer's HasPic option
        /// </summary>
        /// <param name="id">the volunteer id</param>
        /// <returns>status code 200 if successful.</returns>
        /// <example>
        /// curl -F articlepic=@file.jpg "https://localhost:xx/api/volunteerdata/updatevolunteerpic/2"
        /// POST: api/VolunteerData/UpdateVolunteerPic/3
        /// HEADER: enctype=multipart/form-data
        /// FORM-DATA: image
        /// </example>
        /// 

        [HttpPost]
        public IHttpActionResult UpdateVolunteerPic(int id)
        {

            bool haspic = false;
            string picextension;
            if (Request.Content.IsMimeMultipartContent())
            {
                Debug.WriteLine("Received multipart form data.");

                int numfiles = HttpContext.Current.Request.Files.Count;
                Debug.WriteLine("Files Received: " + numfiles);

                //Check if a file is posted
                if (numfiles == 1 && HttpContext.Current.Request.Files[0] != null)
                {
                    var VolunteerPic = HttpContext.Current.Request.Files[0];
                    //Check if the file is empty
                    if (VolunteerPic.ContentLength > 0)
                    {
                        var valtypes = new[] { "jpeg", "jpg", "png", "gif" };
                        var extension = Path.GetExtension(VolunteerPic.FileName).Substring(1);
                        //Check the extension of the file
                        if (valtypes.Contains(extension))
                        {
                            try
                            {
                                //file name is the id of the image
                                string fn = id + "." + extension;

                                //get a direct file path to ~/Content/Volunteers/{id}.{extension}
                                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Volunteers/"), fn);

                                //save the file
                                VolunteerPic.SaveAs(path);
                                Debug.WriteLine("Path:" + path);

                                //if these are all successful then we can set these fields
                                haspic = true;
                                picextension = extension;
                                Debug.WriteLine("fn" + fn);

                                //Update the volunteer haspic and picextension fields in the database
                                Volunteer SelectedVolunteer = db.Volunteers.Find(id);
                                SelectedVolunteer.VolunteerHasPic = haspic;
                                SelectedVolunteer.PicExtension = extension;
                                db.Entry(SelectedVolunteer).State = EntityState.Modified;

                                db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Volunteer Image was not saved successfully.");
                                Debug.WriteLine("Exception:" + ex);
                            }
                        }
                    }

                }
            }

            return Ok();
        }


        /// <summary>ADD MORE...
        /// Adds a volunteer to the database.
        /// </summary>
        /// <param name="volunteer">A volunteer object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/VolunteerData/AddVolunteer
        ///  FORM DATA: Volunteer JSON Object
        /// </example>
        [ResponseType(typeof(Volunteer))]
        [HttpPost]
        public IHttpActionResult AddVolunteer([FromBody] Volunteer volunteer)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Volunteers.Add(volunteer);
            db.SaveChanges();

            return Ok(volunteer.VolunteerID);
        }

        /// <summary>
        /// Deletes a volunteer in the database
        /// </summary>
        /// <param name="id">The id of the volunteer to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/VolunteerData/DeleteVolunteer/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteVolunteer(int id)
        {
            Volunteer volunteer = db.Volunteers.Find(id);
            if (volunteer == null)
            {
                return NotFound();
            }
            //also delete image from path
            string path = HttpContext.Current.Server.MapPath("~/Content/Volunteers/" + id + volunteer.PicExtension);
            if (System.IO.File.Exists(path))
            {
                Debug.WriteLine("File exists... preparing to delete!");
                System.IO.File.Delete(path);
            }

            db.Volunteers.Remove(volunteer);
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
        /// Finds a volunteer in the system. Internal use only.
        /// </summary>
        /// <param name="id">The volunteer id</param>
        /// <returns>TRUE if the volunteer exists, false otherwise.</returns>
        private bool VolunteerExists(int id)
        {
            return db.Volunteers.Count(e => e.VolunteerID == id) > 0;
        }
    }

}
