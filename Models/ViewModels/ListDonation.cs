using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class ListDonation
    {
        public bool isadmin { get; set; }
        public IEnumerable<DonationDto> donations { get; set; }
    }
}