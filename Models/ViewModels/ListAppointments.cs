using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListAppointment
    {
        public bool isadmin { get; set; }
        public AppointmentBookingDto appointmentBookingDto { get; set; }
        public DoctorDTO doctorDto { get; set; }
    }
}