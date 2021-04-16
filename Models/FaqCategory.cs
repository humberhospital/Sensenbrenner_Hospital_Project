using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class FaqCategory
    {
        [Key]
        public int FaqCategoryID { get; set; }

        [ForeignKey("Faq")]
        public int FaqID { get; set; }
        public virtual Faq Faq { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }

    public class FaqCategoryDto
    {
        public int FaqCategoryID { get; set; }
        public int FaqID { get; set; }
        public int CategoryID { get; set; }
    }
}