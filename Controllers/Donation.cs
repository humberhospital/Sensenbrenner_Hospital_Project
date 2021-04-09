using SensenbrennerHospital.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SensenbrennerHospital.Controllers
{
    public class DonationtController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Donation
        public ActionResult Index()
        {
            return View();
        }

        public IEnumerable<DonationDto> GetDonationss()
        {
            List<Donation> Donations = db.Donations.ToList();
            List<DonationDto> DonationDtos = new List<DonationDto>();

            foreach (var donation in Donations)
            {
                DonationDto NewDonation = new DonationDto
                {
                    DonationID = donation.DonationID,
                    DonationAmount = donation.DonationAmount,
                    DonationDate = donation.DonationDate
                };
                DonationDtos.Add(NewDonation);
            }

            return DonationDtos;
        }
    }
}
