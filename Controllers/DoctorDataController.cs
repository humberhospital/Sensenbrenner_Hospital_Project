using SensenbrennerHospital.Models;
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
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [ResponseType(typeof(IEnumerable<DoctorDTO>))]
        [HttpGet]
        public IHttpActionResult GetDoctors()
        {
            List<Doctor> Doctors = db.Doctors.ToList();
            List<DoctorDTO> NewDoctorDTO = new List<DoctorDTO>();

            foreach (var Doctor in Doctors)
            {
                DoctorDTO NewDoctor = new DoctorDTO
                {
                    DoctorID = Doctor.DoctorID,
                    FirstName = Doctor.FirstName,
                    LastName = Doctor.LastName,
                    PracticeID = Doctor.PracticeID
                };
                NewDoctorDTO.Add(NewDoctor);
            }
            return Ok(NewDoctorDTO);
        }

        [ResponseType(typeof(DoctorDTO))]
        [HttpGet]
        public IHttpActionResult GetDoctor(int id)
        {
            Doctor Doctor = db.Doctors.Find(id);
            if (Doctor == null)
            {
                return NotFound();
            }

            return Ok(Doctor);
        }

        [ResponseType(typeof(Doctor))]
        [HttpPost]
        public IHttpActionResult AddDoctor([FromBody] Doctor NewDoctor)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Not Valid");
                return BadRequest(ModelState);
            }
            Debug.WriteLine(NewDoctor);
            db.Doctors.Add(NewDoctor);
            db.SaveChanges();

            return Ok(NewDoctor.DoctorID);
        }

        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor Doctor = db.Doctors.Find(id);
            if (Doctor == null)
            {
                return NotFound();
            }    
        }
    }


}
