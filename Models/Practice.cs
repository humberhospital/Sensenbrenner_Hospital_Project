using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class Practice
    {
        [Key]
        public int PracticeID { get; set; }
        public string PracticeName { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
    }

    public class PracticeDTO
    {
        public int PracticeID { get; set; }
        [Display(Name = "Practice Name")]
        public string PracticeName { get; set; }
        public int DepartmentID { get; set; }
    }
}