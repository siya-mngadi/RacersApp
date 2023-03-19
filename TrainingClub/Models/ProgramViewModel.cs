using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrainingClub.Models
{
    public class ProgramViewModel
    {[Key]
        public int Id { get; set; }
        [Required]
        [Display(Name ="Program Name")]
        public string ProgramName { get; set; }
        [Required]
        [Display(Name = "Program Fee")]
        
        public decimal ProgramFee { get; set; }
        [Required]
        [Display(Name = "Available Space")]
        [Range(1,50)]
        public int NumberOfStudents { get; set; }
     
        [StringLength(10000)]
        public string Descirption { get; set; }
    }
}