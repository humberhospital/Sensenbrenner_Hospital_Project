using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    //The View Model required to update a volunteer
    public class UpdateVolunteer
    {
        //Information about the volunteer
        public VolunteerDto volunteer { get; set; }

        //For a dropdownlist which presents the choice of departments for the Volunteer to be part of
        public IEnumerable<DepartmentDto> alldepartments { get; set; }
    }
}