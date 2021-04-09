using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class ShowDonation
    {

    
            //Conditionally render update/delete links if admin
            public bool isadmin { get; set; }

            //conditionally render 'add donation' if user
            public bool isuser{ get; set; }

            

            //Information about the team
            public DonationDto donation { get; set; }

            //Information about all players on that team
            public IEnumerable<UserInfoDto> donationdonors { get; set; }

            
        }
    }

