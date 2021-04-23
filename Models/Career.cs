using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class Career
    {
        [Key]
        public int CareerID { get; set; }
        public string CareerName { get; set; }
        public string CareerPayRange { get; set; }
        public string CareerType { get; set; }
        public string CareerDescription { get; set; }
    }
    public class CareerDto
    {
        public int CareerID { get; set; }
        [Display(Name = "Title")]
        public string CareerName { get; set; }
        [Display(Name = "Salary Range")]
        public string CareerPayRange { get; set; }
        [Display(Name = "Category")]
        public string CareerType { get; set; }
        [Display(Name = "Description")]
        public string CareerDescription { get; set; }
    }
}