using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListDonation
    {
        public bool isadmin { get; set; }
        public IEnumerable<DonationDto> donations { get; set; }
    }
}