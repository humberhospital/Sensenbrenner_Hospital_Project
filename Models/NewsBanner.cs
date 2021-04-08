using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class NewsBanner
    {
        [Key]
        public int NewsBannerID { get; set; }
        public string NewsTitle { get; set; }
        public string NewsBody { get; set; }
        public string ImagePath { get; set; }
    }
}