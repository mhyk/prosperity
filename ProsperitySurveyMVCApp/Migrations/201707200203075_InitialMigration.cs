namespace ProsperitySurveyMVCApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ApplicationUserId = c.String(),
                        Created = c.DateTimeOffset(nullable: false, precision: 7),
                        Updated = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyDescription = c.String(),
                        SurveyName = c.String(nullable: false),
                        LocationId = c.Int(nullable: false),
                        SurveyDateFrom = c.DateTime(nullable: false),
                        SurveyDateTo = c.DateTime(nullable: false),
                        Completed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Baranggay = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Province = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Families",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId);
            
            CreateTable(
                "dbo.FamilyMembers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Age = c.Int(nullable: false),
                        CivilStatus = c.Int(nullable: false),
                        Gender = c.Int(nullable: false),
                        Employed = c.Boolean(nullable: false),
                        EducationalAttainment = c.Int(nullable: false),
                        Income = c.Double(nullable: false),
                        FamilyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Families", t => t.FamilyId, cascadeDelete: true)
                .Index(t => t.FamilyId);
            
            CreateTable(
                "dbo.SurveyAccounts",
                c => new
                    {
                        Survey_Id = c.Int(nullable: false),
                        Account_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Survey_Id, t.Account_Id })
                .ForeignKey("dbo.Surveys", t => t.Survey_Id, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.Account_Id, cascadeDelete: true)
                .Index(t => t.Survey_Id)
                .Index(t => t.Account_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FamilyMembers", "FamilyId", "dbo.Families");
            DropForeignKey("dbo.Families", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.Surveys", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.SurveyAccounts", "Account_Id", "dbo.Accounts");
            DropForeignKey("dbo.SurveyAccounts", "Survey_Id", "dbo.Surveys");
            DropIndex("dbo.SurveyAccounts", new[] { "Account_Id" });
            DropIndex("dbo.SurveyAccounts", new[] { "Survey_Id" });
            DropIndex("dbo.FamilyMembers", new[] { "FamilyId" });
            DropIndex("dbo.Families", new[] { "SurveyId" });
            DropIndex("dbo.Surveys", new[] { "LocationId" });
            DropTable("dbo.SurveyAccounts");
            DropTable("dbo.FamilyMembers");
            DropTable("dbo.Families");
            DropTable("dbo.Locations");
            DropTable("dbo.Surveys");
            DropTable("dbo.Accounts");
        }
    }
}
