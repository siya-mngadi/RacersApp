using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace TrainingClub.Models
{
    
    public class BookingViewModel
    {[Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        [Required]
        
        public string Name { get; set; }
        [Display(Name = "Email Address")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name="Category")]
        public string category { get; set; }
        [Display(Name = "Program ID")]
        [Required]
        public int ProgramID { get; set; }
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        [Display(Name = "Number Of People")]
        [Required]
        [Range(1,50)]
        public int NumberOfPeople { get; set; }
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        public string userId { get; set; }

        public IEnumerable<SelectListItem> SelectCategory { get; set; }

        public IEnumerable<SelectListItem> SelectProgram { get; set; }
    }
}