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
    public class PracticeDataController : ApiController
    {
        private ApplicationDbContext DB = new ApplicationDbContext();

        [ResponseType(typeof(PracticeDTO))]
        [HttpGet]
        public IHttpActionResult GetPracticesByDepartmentId(int Id)
        {
            List<Practice> PracticeList = DB.Practices.Where(p => p.DepartmentID == Id).ToList();
            List<PracticeDTO> PracticeDtos = new List<PracticeDTO>();

            if (PracticeList == null)
            {
                return NotFound();
            }

            foreach (var Practice in PracticeList)
            {
                PracticeDTO NewPractice = new PracticeDTO
                {
                    PracticeID = Practice.PracticeID,
                    PracticeName = Practice.PracticeName,
                    DepartmentID = Practice.DepartmentID
                };
                PracticeDtos.Add(NewPractice);
            }
            return Ok(PracticeDtos);
        }
    }
}
