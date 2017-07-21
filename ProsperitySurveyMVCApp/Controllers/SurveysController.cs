using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProsperitySurveyMVCApp.Models;

namespace ProsperitySurveyMVCApp.Controllers
{
    [ProsperitySurveyMVCApp.Helper.Authorize(Roles = "admin")]
    public class SurveysController : Controller
    {
        private ProsperityContext db = new ProsperityContext();

        // GET: Surveys
        public ActionResult Index()
        {
            var surveys = db.Surveys.Include(s => s.Location);
            return View(surveys.ToList());
        }

        // GET: Surveys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            return View(survey);
        }

        // GET: Surveys/Create
        public ActionResult Create()
        {
            var baranggays = db.Locations.Select(baranggay => new { Id = baranggay.Id, Baranggay = baranggay.Baranggay + ", " + baranggay.City });
            ViewBag.LocationId = new SelectList(baranggays, "Id", "Baranggay");
            return View();
        }

        // POST: Surveys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SurveyDescription,SurveyName,LocationId,SurveyDateFrom,SurveyDateTo,Completed")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Surveys.Add(survey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Baranggay", survey.LocationId);
            return View(survey);
        }

        // GET: Surveys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Survey survey = db.Surveys.Find(id);
            if (survey == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Baranggay", survey.LocationId);
            return View(survey);
        }

        // POST: Surveys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SurveyDescription,SurveyName,LocationId,SurveyDateFrom,SurveyDateTo,Completed")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Entry(survey).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "Id", "Baranggay", survey.LocationId);
            return View(survey);
        }
        

        public ActionResult AssignSurvey()
        {
            var survey = db.Surveys.Where(s => s.Completed == false);
            ViewBag.SurveyCount = survey.Count();
            ViewBag.SurveyId = new SelectList(survey, "Id", "SurveyName");
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Email");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignSurvey(FormCollection forms)
        {
            int surveyId = Convert.ToInt32(forms["SurveyId"]);
            int accountId = Convert.ToInt32(forms["AccountId"]);
            Survey survey = db.Surveys.Find(surveyId);
            Account account = db.Accounts.Find(accountId);
            survey.Accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
