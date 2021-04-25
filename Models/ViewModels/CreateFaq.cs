using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models.ViewModels
{
	public class CreateFaq
	{
        public FaqDto faq { get; set; }
        public IEnumerable<CategoryDto> allCategories { get; set; }
    }
}