using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class ListFaq
    {
        public FaqDto faq { get; set; }
        public IEnumerable<CategoryDto> category { get; set; }
    }
}