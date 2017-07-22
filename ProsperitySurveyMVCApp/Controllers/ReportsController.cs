using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProsperitySurveyMVCApp.Models;

namespace ProsperitySurveyMVCApp.Controllers
{
    [ProsperitySurveyMVCApp.Helper.Authorize(Roles = "admin")]
    public class ReportsController : Controller
    {

        private ProsperityContext db = new ProsperityContext();
        // GET: Reports
        public ActionResult Index()
        {
            /*List<DataPoint> dataPoints = new List<DataPoint>{
                new DataPoint(10, 22),
                new DataPoint(20, 36),
                new DataPoint(30, 42),
                new DataPoint(40, 51),
                new DataPoint(50, 46),
            };

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);*/
            var survey = db.Surveys.Where(s => s.Completed == false);
            ViewBag.SurveyCount = survey.Count();
            ViewBag.SurveyId = new SelectList(survey, "Id", "SurveyName");
            return View();
        }

        public ActionResult Generate()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Generate(FormCollection formData)
        {
            var surveys = db.Surveys.Where(s => s.Completed == false);
            ViewBag.SurveyCount = surveys.Count();
            ViewBag.SurveyId = new SelectList(surveys, "Id", "SurveyName");

            var reportData = new Report { ReportData = (ReportData)Convert.ToInt32(formData["ReportData"]) };
            var surveyId = Convert.ToInt32(formData["SurveyId"]);

            ViewBag.GraphType = "column";
            ViewBag.GraphShowLegend = 0;

            var population = (from member in db.FamilyMembers
                              join family in db.Families
                              on member.FamilyId equals family.Id
                              join survey in db.Surveys
                              on family.SurveyId equals survey.Id
                              where survey.Id == surveyId
                              select new { member });

            /*var familyCount = (from family in db.Families
                               join survey in db.Surveys
                               on family.SurveyId equals survey.Id
                               where survey.Id == surveyId
                               select new { family }).Count();*/

            ViewBag.Population = population.Count();
            ViewBag.FamilyCount = population.GroupBy(p=>p.member.FamilyId).Count();

            if (reportData.ReportData == ReportData.Income)
            {
                var familyIncome = from member in db.FamilyMembers
                                   join family in db.Families
                                   on member.FamilyId equals family.Id
                                   join survey in db.Surveys
                                   on family.SurveyId equals survey.Id
                                   where survey.Id == surveyId
                                   group member by member.FamilyId into grpFamily
                                   select new
                                   {
                                       Id = grpFamily.Key,
                                       Income = grpFamily.Sum(f => f.Income),
                                   };

                var dataPoints = from income in familyIncome
                                 group income by income.Income into grpIncome
                                 select new
                                 {
                                     x = grpIncome.Key,
                                     y = grpIncome.Count()
                                 };

                int income_0_To_8 = familyIncome.Where(f => f.Income <= 8000).Count();
                int income_8_To_16 = familyIncome.Where(f => f.Income > 8000 && f.Income <= 16000).Count();
                int income_16_To_30 = familyIncome.Where(f => f.Income > 16000 && f.Income <= 30000).Count();
                int income_30 = familyIncome.Where(f => f.Income > 30000).Count();

                ViewBag.Age_0_To_14 = Tuple.Create(income_0_To_8, ComputePercentage(population, income_0_To_8));
                ViewBag.Age_15_To_24 = Tuple.Create(income_8_To_16, ComputePercentage(population, income_8_To_16));
                ViewBag.Age_25_To_59 = Tuple.Create(income_16_To_30, ComputePercentage(population, income_16_To_30));
                ViewBag.Age_60 = Tuple.Create(income_30, ComputePercentage(population, income_30));

                ViewBag.ReportTitle = "Income Bracket";
                ViewBag.GraphTitle = "Income";
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            }
            else if (reportData.ReportData == ReportData.Age)
            {
                var dataPoints = from member in db.FamilyMembers
                                 join family in db.Families
                                 on member.FamilyId equals family.Id
                                 join survey in db.Surveys
                                 on family.SurveyId equals survey.Id
                                 where survey.Id == surveyId
                                 group member by member.Age into grpAge
                                 select new
                                 {
                                     x = grpAge.Key,
                                     y = grpAge.Count(),

                                 };
                int age_0_To_14 = GetAgeCount(surveyId, 14);
                int age_15_To_24 = GetAgeCount(surveyId, 24);
                int age_25_To_59 = GetAgeCount(surveyId, 59);
                int age_60 = GetAgeCount(surveyId, 60);

                ViewBag.Age_0_To_14 = Tuple.Create(age_0_To_14, ComputePercentage(population, age_0_To_14));
                ViewBag.Age_15_To_24 = Tuple.Create(age_15_To_24, ComputePercentage(population, age_15_To_24));
                ViewBag.Age_25_To_59 = Tuple.Create(age_25_To_59, ComputePercentage(population, age_25_To_59));
                ViewBag.Age_60 = Tuple.Create(age_60, ComputePercentage(population, age_60));

                ViewBag.ReportTitle = "Income Bracket";
                ViewBag.GraphTitle = "Age";
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            }
            else if (reportData.ReportData == ReportData.Gender)
            {
                var dataPoints = from member in db.FamilyMembers
                                 join family in db.Families
                                 on member.FamilyId equals family.Id
                                 join survey in db.Surveys
                                 on family.SurveyId equals survey.Id
                                 where survey.Id == surveyId
                                 group member by member.Gender into grpGender
                                 select new
                                 {
                                     x = grpGender.Key,
                                     y = grpGender.Count(),

                                 };
                ViewBag.GraphTitle = "Gender";
                ViewBag.GraphType = "doughnut";
                ViewBag.GraphShowLegend = 1;
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            }
            return View();

        }

        private int GetAgeCount(int surveyId, int bracket)
        {
            int ageCount = 0;
            if (bracket == 14)
            {
                ageCount = (from member in db.FamilyMembers
                            join family in db.Families
                            on member.FamilyId equals family.Id
                            join survey in db.Surveys
                            on family.SurveyId equals survey.Id
                            where survey.Id == surveyId && (member.Age <= 14)
                            select new { member }).Count();
            }
            else if (bracket == 24)
            {
                ageCount = (from member in db.FamilyMembers
                            join family in db.Families
                            on member.FamilyId equals family.Id
                            join survey in db.Surveys
                            on family.SurveyId equals survey.Id
                            where survey.Id == surveyId && (member.Age > 14 && member.Age <= 24)
                            select new { member }).Count();
            }
            else if (bracket == 59)
            {
                ageCount = (from member in db.FamilyMembers
                            join family in db.Families
                            on member.FamilyId equals family.Id
                            join survey in db.Surveys
                            on family.SurveyId equals survey.Id
                            where survey.Id == surveyId && (member.Age > 24 && member.Age <= 59)
                            select new { member }).Count();
            }
            else if (bracket == 60)
            {
                ageCount = (from member in db.FamilyMembers
                            join family in db.Families
                            on member.FamilyId equals family.Id
                            join survey in db.Surveys
                            on family.SurveyId equals survey.Id
                            where survey.Id == surveyId && (member.Age >= 60)
                            select new { member }).Count();
            }
            return ageCount;
        }

        private string ComputePercentage(int population, int value)
        {
            double result = (double)value / population * 100;
            return Math.Round(result, 2).ToString() + "%";
        }

    }
}