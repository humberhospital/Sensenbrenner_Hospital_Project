using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class HospitalTour
    {
        [Key]
        public int HospitalTourID { get; set; }
        public string DepartmentName { get; set; } //Do we need it?
        public string Description { get; set; }
        public string VideoPath { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department Department { get; set; }
    }
}