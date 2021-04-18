using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ShowVolunteerPosition
    {

        public VolunteerPositionDto volunteerposition { get; set; }
        //information about the department the volunteer position is in
        public DepartmentDto department { get; set; }
    }
}