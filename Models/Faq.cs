using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }

        //An Faq belongs to one category
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }
    }

    public class FaqDto
    {
        public int FaqID { get; set; }
        [Required(ErrorMessage = "Please enter the question.")]
        public string Question { get; set; }
        [Required(ErrorMessage = "Please enter the answer.")]
        public string Answer { get; set; }
        [DisplayName("Category")]
        public int CategoryID { get; set; }
    }
}