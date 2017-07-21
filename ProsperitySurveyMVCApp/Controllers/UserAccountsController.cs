using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProsperitySurveyMVCApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ProsperitySurveyMVCApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserAccountsController : Controller
    {
        private ProsperityContext db = new ProsperityContext();

        // GET: UserAccounts
        public ActionResult Index()
        {

            
            return View(db.Accounts.ToList());
        }

        // GET: UserAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: UserAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection forms)
        {
            var userDb = new ApplicationDbContext();
            
            IdentityResult IdUserResult;
            var usrMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userDb));
            var appUser = new ApplicationUser
            {
                UserName = forms["Email"],
                Email = forms["Email"]
            };
            IdUserResult = usrMgr.Create(appUser, forms["AutoPassword"]);

            if (!usrMgr.IsInRole(usrMgr.FindByEmail(forms["Email"]).Id, forms["Role"]))
            {
                IdUserResult = usrMgr.AddToRole(usrMgr.FindByEmail(forms["Email"]).Id, forms["Role"]);
            }

            Account account = new Account
            {                
                Email = forms["Email"],
                FirstName = forms["FirstName"],
                LastName = forms["LastName"],
                ApplicationUserId = usrMgr.FindByEmail(forms["Email"]).Id
            };
            db.Accounts.Add(account);
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
