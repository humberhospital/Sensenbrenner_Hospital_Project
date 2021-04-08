using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class JobApplication
    {
        [Key]
        public int JobApplicationID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ResumePath { get; set; }
        public string CoverLetterPath { get; set; }

        [ForeignKey("Career")]
        public int CareerID { get; set; }
        public virtual Career Career { get; set; }
    }
}