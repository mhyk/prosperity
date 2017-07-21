using ProsperitySurveyMVCApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProsperitySurveyMVCApp.Controllers
{
    public class HomeController : Controller
    {

        private ProsperityContext db = new ProsperityContext();

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();                
                var account = db.Accounts.Where(a => a.ApplicationUserId == userId);
                if(account.Count() != 0)
                {
                    var surveys = db.Surveys.Where(s => s.Accounts.Any(a => a.Id == account.FirstOrDefault().Id));
                    ViewBag.SurveyCount = surveys.Count();
                    var model = surveys.OrderByDescending(s => s.Id);
                    return View(model);
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
                
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Surveys/Details/5
        public ActionResult SelectSurvey(int? id)
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
            Session.Add("surveyId", id);
            return RedirectToAction("Index", "SurveyRecords");
        }

        public ActionResult UnAuthorized()
        {
            return View();
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