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
}