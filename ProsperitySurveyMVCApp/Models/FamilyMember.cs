using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProsperitySurveyMVCApp.Models
{
    public class FamilyMember
    {
        public FamilyMember()
        {            
            Income = 0;
            Employed = false;
            EducationalAttainment = EducationalAttainment.NotApplicable;
        }

        public int Id { get; set; }

        public int Age { get; set; }

        [Display(Name = "Civil Status")]
        public CivilStatus CivilStatus { get; set; }

        public Gender Gender { get; set; }
        public bool Employed { get; set; }

        [Display(Name = "Educational Attainment")]
        public EducationalAttainment EducationalAttainment { get; set; }

        public double Income { get; set; }

        [ScaffoldColumn(false)]
        public int FamilyId { get; set; }
                
        [ForeignKey("FamilyId")]
        public virtual Family Family { get; set; }
    }
}