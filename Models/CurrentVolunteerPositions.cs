using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class CurrentVolunteerPositions
    {
        [Key]
        public int CvpID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        //Volunteer positions will be associated with one department
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }

        public virtual Department Department { get; set; }
    }
}