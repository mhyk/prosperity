namespace ProsperitySurveyMVCApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ProsperitySurveyMVCApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProsperitySurveyMVCApp.Models.ProsperityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProsperitySurveyMVCApp.Models.ProsperityContext context)
        {
            if (context.Locations.Count() == 0)
            {
                GetLocations().ForEach(i => context.Locations.Add(i));
            }

            if (context.Surveys.Count() == 0)
            {
                GetSurveys().ForEach(i => context.Surveys.Add(i));
            }

            if (context.Families.Count() == 0)
            {
                GetFamilies().ForEach(i => context.Families.Add(i));

            }

            if (context.FamilyMembers.Count() == 0)
            {
                GetFamilyMembers().ForEach(i => context.FamilyMembers.Add(i));

            }

            AddDefaultUser();


            if (context.Accounts.Count() == 0)
            {
                GetAccounts().ForEach(i => context.Accounts.Add(i));
            }

        }

        private static List<Family> GetFamilies()
        {
            var family = new List<Family>();
            for (int i = 1; i <= 100; i++)
            {
                family.Add(new Family { Id = i, SurveyId = 4 });
            }
            return family;
        }

        private static List<FamilyMember> GetFamilyMembers()
        {
            var familyMember = new List<FamilyMember>();

            using (var reader = new StreamReader(@"C:\Users\C#SUC-2017\Downloads\MVC_720\MVC_720\prosperity_test_data.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    Random rnd = new Random((int)DateTime.Now.Ticks);


                    var member = new FamilyMember
                    {
                        FamilyId = Convert.ToInt32(values[0]),
                        Gender = (values[1].Trim() == "Male") ? Gender.Male : Gender.Female,
                        Age = Convert.ToInt32(values[2]),
                        CivilStatus = GetStatus(values[3].Trim()),
                        Employed = (values[4].Trim() == "yes") ? true : false,
                        Income = (values[4].Trim() == "yes") ? Convert.ToInt32(values[5]) * 1000 : 0
                    };
                    familyMember.Add(member);
                }
            }

            return familyMember;

        }

        private static CivilStatus GetStatus(string value)
        {
            if (value == "Single")
                return CivilStatus.Single;
            else if (value == "Married")
                return CivilStatus.Married;
            else if (value == "Widowed")
                return CivilStatus.Widowed;
            else if (value == "Separated")
                return CivilStatus.Separated;
            else
                return CivilStatus.Single;

        }

        private static List<Location> GetLocations()
        {
            var location = new List<Location>()
            {
                new Location{Baranggay = "Prosperity", City="Quezon City", Province="NCR"}
            };
            return location;
        }

        private static List<Survey> GetSurveys()
        {
            DateTime from = new DateTime(2017, 2, 1);
            var survey = new List<Survey>()
            {
                new Survey
                {
                    Id = 4,
                    LocationId =1,
                    SurveyDescription="Test Survey Data",
                    SurveyDateFrom = new DateTime(2017, 2, 1),
                    SurveyDateTo = new DateTime(2017, 2, 28),
                    SurveyName = "Test Survey Data"
                }
            };
            return survey;

        }

        private static List<Account> GetAccounts()
        {
            var userDb = new ApplicationDbContext();
            var usrMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userDb));
            var adminUser = usrMgr.FindByEmail("admin@prosperityapp.com");
            var staffUser = usrMgr.FindByEmail("staff@prosperityapp.com");
            var account = new List<Account>()
            {
                new Account
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    ApplicationUserId = adminUser.Id,
                    Email = adminUser.Email
                },
                new Account
                {
                    FirstName = "Staff",
                    LastName = "Staff",
                    ApplicationUserId = staffUser.Id,
                    Email = staffUser.Email
                }

            };
            return account;
        }

        private static void AddDefaultUser()
        {
            var userDb = new ApplicationDbContext();

            IdentityResult IdRoleResult;
            IdentityResult IdUserResult;

            var roleStore = new RoleStore<IdentityRole>(userDb);

            var roleMgr = new RoleManager<IdentityRole>(roleStore);

            if (!roleMgr.RoleExists("staff"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "staff" });
            }

            if (!roleMgr.RoleExists("admin"))
            {
                IdRoleResult = roleMgr.Create(new IdentityRole { Name = "admin" });
            }

            if (userDb.Users.Count() == 0)
            {
                var usrMgr = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userDb));
                var appUser = new ApplicationUser
                {
                    UserName = "admin@prosperityapp.com",
                    Email = "admin@prosperityapp.com"
                };
                IdUserResult = usrMgr.Create(appUser, "adminpass");

                if (!usrMgr.IsInRole(usrMgr.FindByEmail("admin@prosperityapp.com").Id, "admin"))
                {
                    IdUserResult = usrMgr.AddToRole(usrMgr.FindByEmail("admin@prosperityapp.com").Id, "admin");
                }

                appUser = new ApplicationUser
                {
                    UserName = "staff@prosperityapp.com",
                    Email = "staff@prosperityapp.com"
                };
                IdUserResult = usrMgr.Create(appUser, "staffpass");

                if (!usrMgr.IsInRole(usrMgr.FindByEmail("staff@prosperityapp.com").Id, "staff"))
                {
                    IdUserResult = usrMgr.AddToRole(usrMgr.FindByEmail("staff@prosperityapp.com").Id, "staff");
                }

            }

        }
    }
}
