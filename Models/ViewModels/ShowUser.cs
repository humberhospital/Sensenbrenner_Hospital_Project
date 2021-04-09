using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class ShowUser
    {

        //Conditionally render the page depending on if the admin is logged in.
        public bool isadmin { get; set; }
        public UserInfoDto user { get; set; }
        //information about the team the player plays for
        public DonationDto donation { get; set; }
    }
}