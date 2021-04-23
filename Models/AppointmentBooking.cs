using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class AppointmentBooking
    {
        [Key]
        public int AppointmentID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string RequestDescription { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Confirmation { get; set; }

        [ForeignKey("DoctorInfo")]
        public int DoctorID { get; set; }
        public virtual Doctor DoctorInfo { get; set; }
    }
    public class AppointmentBookingDto
    {
        public int AppointmentID { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Request Description")]
        public string RequestDescription { get; set; }
        [Display(Name = "Appointment Date")]
        public DateTime AppointmentDate { get; set; }
        public string Confirmation { get; set; }
        [Display(Name = "Doctor")]
        public int DoctorID { get; set; }
    }
}