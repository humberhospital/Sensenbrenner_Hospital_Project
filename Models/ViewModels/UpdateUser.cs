using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class UpdateUser
    {

        //Information about the user
        public UserInfoDto user { get; set; }

        public IEnumerable<DonationDto> alldonations { get; set; }

    }
}