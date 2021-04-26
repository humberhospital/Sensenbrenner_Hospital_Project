using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ShowDepartment
    {
        public DepartmentDto department { get; set; }
        public IEnumerable<Practice> listOfPractices { get; set; }
    }
}