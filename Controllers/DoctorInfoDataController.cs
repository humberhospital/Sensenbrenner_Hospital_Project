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
    public class DoctorInfoDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [ResponseType(typeof(IEnumerable<DoctorDTO>))]
        [HttpGet]
        public IHttpActionResult GetDoctors()
        {
            List<Doctor> Doctors = db.DoctorInfos.ToList();
            List<DoctorDTO> doctorInfoDTOs = new List<DoctorDTO>();

            foreach (var Doctor in Doctors)
            {
                DoctorDTO NewDoctor = new DoctorDTO
                {
                    DoctorID = Doctor.DoctorID,
                    FirstName = Doctor.FirstName,
                    LastName = Doctor.LastName,
                    PracticeID = Doctor.PracticeID
                };
                doctorInfoDTOs.Add(NewDoctor);
            }
            return Ok(doctorInfoDTOs);
        }
    }


}
