using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProsperitySurveyMVCApp.Models
{
    public class Account
    {
        public Account()
        {
            Surveys = new HashSet<Survey>();
            Created = DateTimeOffset.Now;
            Updated = DateTimeOffset.Now;
        }

        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [ScaffoldColumn(false)]
        public string ApplicationUserId { get; set; }

        [ScaffoldColumn(false)]
        public DateTimeOffset Created { get; set; }

        [ScaffoldColumn(false)]
        public DateTimeOffset Updated { get; set; }


        public virtual ICollection<Survey> Surveys { get; set; }
    }
    
}