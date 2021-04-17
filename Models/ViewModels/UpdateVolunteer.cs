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
        //Needed for a dropdownlist which presents the choice of departments for the coluntter to be part of
        public IEnumerable<DepartmentDto> alldepartments { get; set; }
    }
}