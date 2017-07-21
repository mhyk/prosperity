using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProsperitySurveyMVCApp.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string Baranggay { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Province { get; set; }

    }
}