using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ShowVolunteer
    {
        public VolunteerDto volunteer { get; set; }

        //information about the department the volunteer is in
        public DepartmentDto department { get; set; }
 
    }
}