using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class CreatePractice
    {
        public Practice Practice { get; set; }
        public IEnumerable<DepartmentDto> AllDepartments { get; set; }
    }
}