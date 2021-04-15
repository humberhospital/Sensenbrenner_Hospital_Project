using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("Practice")]
        public int PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
    }

    public class DoctorDTO
    {
        public int DoctorID { get; set; }
        [Display(Name = "Doctor First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Doctor Last Name")]
        public string LastName { get; set; }
        public int PracticeID { get; set; }
    }
}