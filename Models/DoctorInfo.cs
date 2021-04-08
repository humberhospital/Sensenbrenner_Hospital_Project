using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class DoctorInfo
    {
        [Key]
        public int DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [ForeignKey("Practice")]
        public int PracticeID { get; set; }
        public virtual Practice Practice { get; set; }
    }
}