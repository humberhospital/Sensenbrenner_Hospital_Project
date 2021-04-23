using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListAppointments
    {
        public bool isadmin { get; set; }
        public AppointmentBookingDto appointment { get; set; }
        public IEnumerable<CategoryDto> category { get; set; }
    }
}