using ProsperitySurveyMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProsperitySurveyMVCApp.Controllers
{
    public class SurveyRecordsController : Controller
    {

        private ProsperityContext db = new ProsperityContext();

        // GET: SurveyRecords
        public ActionResult Index()
        {
            if (Session["surveyId"] == null)
                return Redirect("~/Home");
            else
            {
                var surveyId = Convert.ToInt32(Session["surveyId"]);
                /*var model = db.Families.Join(db.FamilySurveys, f => f.Id, fs => fs.FamilyId, (f, fs) => new { Family = f, FamilySurvey = fs }).Where(s => s.FamilySurvey.SurveyId==surveyId);*/

                /*var model = from family in db.Families
                            join familySurvey in db.FamilySurveys
                            on family.Id equals familySurvey.FamilyId
                            where familySurvey.SurveyId == surveyId
                            select family;
                return View(model.ToList());*/
                var family = new Family
                {
                    SurveyId = surveyId
                };
                db.Families.Add(family);                
                db.SaveChanges();

                return Redirect($"~/SurveyRecords/AddMember/{family.Id}");                
            }                
        }

/*
        public ActionResult FamilyDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = db.Families.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            ViewBag.FamilyName = family.FamilyName;
            var model = db.FamilyMembers.Where(fm => fm.FamilyId == id);
            return View(model.ToList());
        }*/

        /*public ActionResult NewFamily()
        {
            if (Session["surveyId"] == null)
                return Redirect("~/Home");
            else
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewFamily(Family family)
        {
            db.Families.Add(family);
            FamilySurvey surveyData = new FamilySurvey
            {
                SurveyId = Convert.ToInt32(Session["surveyId"]),
                FamilyId = family.Id
            };
            db.FamilySurveys.Add(surveyData);
            db.SaveChanges();
            
            return Redirect($"~/SurveyRecords/AddMember/{family.Id}");
        }*/

        public ActionResult AddMember(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Family family = db.Families.Find(id);
            if (family == null)
            {
                return HttpNotFound();
            }
            //ViewBag.FamilyHeadExist = db.FamilyMembers.Where(f => f.FamilyId == id && f.IsFamilyHead == true).Count();
            
            ViewBag.FamilyId = id;            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(FamilyMember familyMember)
        {
            if (ModelState.IsValid)
            {
                db.FamilyMembers.Add(familyMember);
                db.SaveChanges();

                //UpdateSurveyData(familyMember.FamilyId);

                return RedirectToAction("AddMember", familyMember.FamilyId);
            }

            return View(familyMember);
            
        }
/*
        private void UpdateSurveyData(int familyId)
        {
            int surveyId;
            if (Session["surveyId"] == null)
            {
                surveyId = db.FamilySurveys.Where(fs => fs.FamilyId == familyId).FirstOrDefault().SurveyId;
            }
            else
            {
                surveyId = Convert.ToInt32(Session["surveyId"]);
            }
            
            FamilySurvey familySurvey = db.FamilySurveys.Where(f => f.FamilyId == familyId && f.SurveyId == surveyId).First();

            familySurvey.FamilyIncome = db.FamilyMembers.Where(f => f.FamilyId == familySurvey.FamilyId).Sum(f => f.Income);
            familySurvey.NumberOfElementary = db.FamilyMembers.Where(f => f.FamilyId == familySurvey.FamilyId && f.EducationalAttainment == EducationalAttainment.Elementary).Count();
            familySurvey.NumberOfHighSchool = db.FamilyMembers.Where(f => f.FamilyId == familySurvey.FamilyId && f.EducationalAttainment == EducationalAttainment.HighSchool).Count();
            familySurvey.NumberOfCollege = db.FamilyMembers.Where(f => f.FamilyId == familySurvey.FamilyId && f.EducationalAttainment == EducationalAttainment.College).Count();
            familySurvey.NumberOfEmployed = db.FamilyMembers.Where(f => f.FamilyId == familySurvey.FamilyId && f.Employed == true).Count();
            familySurvey.NumberOfDependent = db.FamilyMembers.Where(f => f.FamilyId == familySurvey.FamilyId && f.Employed == false).Count();
            db.SaveChanges();
        }*/
    }
}