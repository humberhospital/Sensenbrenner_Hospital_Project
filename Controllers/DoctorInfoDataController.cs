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

        [ResponseType(typeof(IEnumerable<DoctorInfoDTO>))]
        [HttpGet]
        public IHttpActionResult GetDoctors()
        {
            List<DoctorInfo> Doctors = db.DoctorInfos.ToList();
            List<DoctorInfoDTO> doctorInfoDTOs = new List<DoctorInfoDTO>();

            foreach (var Doctor in Doctors)
            {
                DoctorInfoDTO NewDoctor = new DoctorInfoDTO
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
