using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListFaq
    {
        //Admin will see administrative options while normal users will not
        public bool isadmin { get; set; }
        public FaqDto faq { get; set; }
        public IEnumerable<CategoryDto> category { get; set; }
    }
}