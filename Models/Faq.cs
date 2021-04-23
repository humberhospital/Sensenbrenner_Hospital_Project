using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SensenbrennerHospital.Models
{
    public class Faq
    {
        [Key]
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        //An Faq belongs to one category
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }

    public class FaqDto
    {
        public int FaqID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}