using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class CreateAppointment
    {
        //Conditionally render update/delete links if admin
        public bool isAdmin { get; set; }

        //conditionally render 'add donation' if user
        public bool isUser { get; set; }

        public AppointmentBooking appointmentBooking { get; set; }
        public List<SelectListItem> doctorSelectList { get; set; }

    }
}