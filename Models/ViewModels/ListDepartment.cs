using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListDepartment
    {
        public bool isadmin { get; set; }
        public IEnumerable<DepartmentDto> listOfDepartments { get; set; }
    }
}