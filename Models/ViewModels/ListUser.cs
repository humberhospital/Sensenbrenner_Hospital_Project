using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListUser
    {

        //Our View needs to conditionally render the page based on admin or non admin.
        //Admin will see "Create New" and "Edit" links, non-admin will not see these.
        public bool isadmin { get; set; }

        public IEnumerable<UserInfoDto> users { get; set; }
    }
}