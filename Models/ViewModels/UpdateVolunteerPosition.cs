using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    //The View Model required to update a volunteer
    public class UpdateVolunteerPosition
    {
        //Information about the volunteer position
        public VolunteerPositionDto volunteerposition { get; set; }

        //For a dropdownlist which presents the choice of departments that the volunteer position will be part of
        public IEnumerable<DepartmentDto> alldepartments { get; set; }
    }
}