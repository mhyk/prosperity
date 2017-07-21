using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProsperitySurveyMVCApp.Models
{
    public class ProsperityContext : DbContext
    {
        public ProsperityContext() : base("ProsperitySurveyMVC") { }

        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Family> Families { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<FamilyMember> FamilyMembers { get; set; }

    }
}