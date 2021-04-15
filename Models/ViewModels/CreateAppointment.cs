using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class CreateAppointment
    {
        //Conditionally render update/delete links if admin
        public bool isAdmin { get; set; }

        //conditionally render 'add donation' if user
        public bool isUser { get; set; }

        public AppointmentBooking appointmentBooking { get; set; }
        public IEnumerable<DoctorDTO> doctorDTOs { get; set; }

    }
}