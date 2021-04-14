using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }
        public decimal DonationAmount { get; set; }
        public string DonationMethod { get; set; }
        public string DonationText { get; set; }

        public DateTime DonationDate { get; set; }

        [ForeignKey("UserInfo")]
        public int UserID { get; set; }
        public virtual UserInfo UserInfo { get; set; }

        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }


    }

    public class DonationDto
    {
        public int DonationID { get; set; }


        [DisplayName("Donation amount")]
        public decimal DonationAmount { get; set; }

        [DisplayName("Donation Date")]
        public DateTime DonationDate { get; set; }

        [DisplayName("Donation Method")]
        public string DonationMethod { get; set; }

        [DisplayName("Donation Text")]
        public string DonationText { get; set; }


    }
}