using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProsperitySurveyMVCApp.Models
{
    public class Survey
    {
        public Survey()
        {            
            Accounts = new HashSet<Account>();
            Completed = false;            
        }

        public int Id { get; set; }

        [Display(Name = "Description")]
        public string SurveyDescription { get; set; }

        [Required, Display(Name = "Survey Name")]
        public string SurveyName { get; set; }

        [Display(Name = "Baranggay")]
        public int LocationId { get; set; }
                
        [Display(Name = "Date From")]
        public DateTime SurveyDateFrom { get; set; }

        [Display(Name = "Date To")]
        public DateTime SurveyDateTo { get; set; }

        public bool Completed { get; set; }

        [ForeignKey("LocationId")]
        public virtual Location Location { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

    }
}