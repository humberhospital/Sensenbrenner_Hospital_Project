using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
    public class DeleteCategory
    {
        public CategoryDto category { get; set; }
        public IEnumerable<FaqDto> faqList { get; set; }
    }
}