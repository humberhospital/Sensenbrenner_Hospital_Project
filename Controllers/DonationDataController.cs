using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SensenbrennerHospital.Models;

namespace SensenbrennerHospital.Controllers
{
    public class DonationDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [ResponseType(typeof(IEnumerable<DonationDto>))]
        [HttpGet]
        public IHttpActionResult GetDonations()
        {
            List<Donation> donations = db.Donations.ToList();
            List<DonationDto> donationDtos = new List<DonationDto>();

            foreach (var donation in donations)
            {
                DonationDto NewDonation = new DonationDto
                {
                    DonationID = donation.DonationID,
                    DonationAmount = donation.DonationAmount,
                    DonationDate = donation.DonationDate,
                    DonationMethod = donation.DonationMethod,
                    DonationText = donation.DonationText
                };
                donationDtos.Add(NewDonation);
            }

            return Ok(donationDtos);
        }

        // GET : api/DonationData/GetDonation/7
        [ResponseType(typeof(DonationDto))]
        [HttpGet]
        public IHttpActionResult GetDonation(int id)
        {
            Donation donation = db.Donations.Find(id);
            if (donation == null)
            {
                return NotFound();
            }
            return Ok(donation);
        }

        [ResponseType(typeof(Donation))]
        [HttpPost]
        public IHttpActionResult AddDonation([FromBody] Donation newDonation)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Not valid");
                return BadRequest(ModelState);
            }
            Debug.WriteLine(newDonation);
            newDonation.DonationDate = DateTime.Now;
            db.Donations.Add(newDonation);
            db.SaveChanges();

            return Ok(newDonation.DonationID);
        }


        //POST: api/DonationData/UpdateDonation/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDonation(int id, [FromBody] Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (id != donation.DonationID)
            {
                return BadRequest();
            }


            db.Entry(donation).State = EntityState.Modified;
            // I don't know how it works 
            db.Entry(donation).Property(d => d.Id).IsModified = false;  
            db.SaveChanges();
            
            return StatusCode(HttpStatusCode.NoContent);

            }
            

            [HttpPost]
            public IHttpActionResult DeleteDonation(int id)
            {
                Donation donation = db.Donations.Find(id);
                if (donation == null)
                {
                    return NotFound();
                }

                db.Donations.Remove(donation);
                db.SaveChanges();

                return Ok(donation);
            }

        [HttpGet]
        public IHttpActionResult GetListOfDonations()
        {
            List<Donation> donations = db.Donations.ToList();
            List<DonationDto> donationDtos = new List<DonationDto>();

            foreach (var donation in donations)
            {
                DonationDto NewDonation = new DonationDto
                {
                    DonationID = donation.DonationID,
                    DonationAmount = donation.DonationAmount,
                    DonationDate = donation.DonationDate,
                    DonationMethod = donation.DonationMethod,
                    DonationText = donation.DonationText
                };
                donationDtos.Add(NewDonation);
            }

            return Ok(donationDtos);
        }
    }
}
