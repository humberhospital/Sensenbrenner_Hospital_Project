using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListDoctor
    {
        public DoctorDTO Doctor { get; set; }
        public PracticeDTO Practice { get; set; }
    }
}