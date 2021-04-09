using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool VolunteerHasPic { get; set; }
        public string PicExtension { get; set; }

        //Volunteers will be associated with one department
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
    }

    //Transfer Object for Volunteer
    public class VolunteerDto
    {
        public int VolunteerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool VolunteerHasPic { get; set; }
        public string PicExtension { get; set; }
        public int DepartmentID { get; set; }

    }
}