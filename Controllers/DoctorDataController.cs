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
    public class DoctorDataController : ApiController
    {
        private ApplicationDbContext DB = new ApplicationDbContext();

        [ResponseType(typeof(IEnumerable<DoctorDTO>))]
        [HttpGet]
        public IHttpActionResult GetDoctors()
        {
            List<Doctor> Doctors = DB.Doctors.ToList();
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
            Doctor Doctor = DB.Doctors.Find(id);
            if (Doctor == null)
            {
                return NotFound();
            }

            return Ok(Doctor);
        }

        [ResponseType(typeof(DoctorDTO))]
        [HttpGet]
        public IHttpActionResult FindDoctor(int id)
        {
            Doctor Doctor = DB.Doctors.Find(id);

            if (Doctor == null)
            {
                return NotFound();
            }

            DoctorDTO SelectedDoctor = new DoctorDTO
            {
                DoctorID = Doctor.DoctorID,
                FirstName = Doctor.FirstName,
                LastName = Doctor.LastName,
                PracticeID = Doctor.PracticeID
            };

            return Ok(SelectedDoctor);
        }

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDoctor(int ID, [FromBody] Doctor Doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ID != Doctor.DoctorID)
            {
                return BadRequest();
            }

            DB.Entry(Doctor).State = EntityState.Modified;

            try
            {
                DB.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoctorExists(ID))
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
            DB.Doctors.Add(NewDoctor);
            DB.SaveChanges();

            return Ok(NewDoctor.DoctorID);
        }

        [HttpPost]
        public IHttpActionResult DeleteDoctor(int id)
        {
            Doctor Doctor = DB.Doctors.Find(id);
            if (Doctor == null)
            {
                return NotFound();
            }

            DB.Doctors.Remove(Doctor);
            DB.SaveChanges();

            return Ok(Doctor);
        }

        [HttpGet]
        public IHttpActionResult GetListOfDoctors()
        {
            List<Doctor> Doctors = DB.Doctors.ToList();
            List<DoctorDTO> doctorDTOs = new List<DoctorDTO>();

            foreach (var Doctor in Doctors)
            {
                DoctorDTO NewDoctor = new DoctorDTO
                {
                    DoctorID = Doctor.DoctorID,
                    FirstName = Doctor.FirstName,
                    LastName = Doctor.LastName,
                    PracticeID = Doctor.PracticeID
                };
                doctorDTOs.Add(NewDoctor);
            }
            return Ok(doctorDTOs);
        }

        private bool DoctorExists(int id)
        {
            return DB.Doctors.Count(Doctor => Doctor.DoctorID == id) > 0;
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DB.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
