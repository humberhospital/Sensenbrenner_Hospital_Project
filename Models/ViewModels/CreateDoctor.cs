using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class CreateDoctor
    {
        public List<PracticeDTO> Practice { get; set; }
        public DoctorDTO Doctor { get; set; }
    }


}