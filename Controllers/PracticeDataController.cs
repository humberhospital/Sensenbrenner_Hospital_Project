using SensenbrennerHospital.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
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

        [ResponseType(typeof(IEnumerable<PracticeDTO>))]
        [HttpGet]
        public IHttpActionResult GetPractices()
        {
            List<Practice> Practices = DB.Practices.ToList();
            List<PracticeDTO> NewPracticeDTO = new List<PracticeDTO>();

            foreach (var Practice in Practices)
            {
                PracticeDTO NewPractice = new PracticeDTO
                {
                    PracticeID = Practice.PracticeID,
                    PracticeName = Practice.PracticeName,
                    DepartmentID = Practice.DepartmentID
                };
                NewPracticeDTO.Add(NewPractice);
            }
            return Ok(NewPracticeDTO);
        }

        [ResponseType(typeof(PracticeDTO))]
        [HttpGet]
        public IHttpActionResult GetPractice(int id)
        {
            Practice Practice = DB.Practices.Find(id);
            
            if (Practice == null)
            {
                return NotFound();
            }
            return Ok(Practice);
        }

        [ResponseType(typeof(PracticeDTO))]
        [HttpGet]
        public IHttpActionResult FindPractice(int ID)
        {
            Practice Practice = DB.Practices.Find(ID);

            if (Practice == null)
            {
                return NotFound();
            }

            PracticeDTO SelectedPractice = new PracticeDTO
            {
                PracticeID = Practice.PracticeID,
                PracticeName = Practice.PracticeName,
                DepartmentID = Practice.DepartmentID
            };

            return Ok(SelectedPractice);
        }

        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdatePractice(int ID, [FromBody] Practice Practice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ID != Practice.PracticeID)
            {
                return BadRequest();
            }

            DB.Entry(Practice).State = System.Data.Entity.EntityState.Modified;

            try
            {
                DB.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PracticeExists(ID))
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

        [ResponseType(typeof(Practice))]
        [HttpPost]
        public IHttpActionResult AddPractice([FromBody] Practice NewPractice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            DB.Practices.Add(NewPractice);
            DB.SaveChanges();

            return Ok(NewPractice.PracticeID);
        }

        [HttpPost]
        public IHttpActionResult DeletePractice(int ID)
        {
            Practice Practice = DB.Practices.Find(ID);

            if (Practice == null)
            {
                return NotFound();
            }

            DB.Practices.Remove(Practice);
            DB.SaveChanges();

            return Ok(Practice);
        }

        [HttpGet]
        public IHttpActionResult GetListOfPractices()
        {
            List<Practice> Practices = DB.Practices.ToList();
            List<PracticeDTO> PracticeDTOs = new List<PracticeDTO>();

            foreach (var Practice in Practices)
            {
                PracticeDTO NewPractice = new PracticeDTO
                {
                    PracticeID = Practice.PracticeID,
                    PracticeName = Practice.PracticeName,
                    DepartmentID = Practice.DepartmentID
                };
                PracticeDTOs.Add(NewPractice);
            }
            return Ok(PracticeDTOs);
        }



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

        private bool PracticeExists(int ID)
        {
            return DB.Practices.Count(Practice => Practice.PracticeID == ID) > 0;
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
