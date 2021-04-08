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
        public virtual DoctorInfo DoctorInfo { get; set; }
    }
}