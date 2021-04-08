using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class DonorInfo
    {
        [Key]
        public int DonorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StreetNumber { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}