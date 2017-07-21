using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProsperitySurveyMVCApp.Models
{
    public enum CivilStatus
    {
        Single,
        Married,
        Widowed,
        Separated
    }

    public enum EducationalAttainment
    {
        NotApplicable,
        Elementary,
        HighSchool,
        College
    }

    public enum IncomeType
    {
        Daily,
        Weekly,
        Salary,
        Others
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum ReportData
    {
        Age,
        Income,
        Gender
    }
}