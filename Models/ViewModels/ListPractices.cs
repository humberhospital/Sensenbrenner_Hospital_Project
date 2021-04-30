using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListPractices
    {
        public PracticeDTO Practice { get; set; }
        public DepartmentDto Department { get; set; }
    }
}