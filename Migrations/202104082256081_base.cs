namespace SensenbrennerHospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _base : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Abouts",
                c => new
                    {
                        AboutID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.AboutID);
            
            CreateTable(
                "dbo.AppointmentBookings",
                c => new
                    {
                        AppointmentID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PhoneNumber = c.String(),
                        RequestDescription = c.String(),
                        AppointmentDate = c.DateTime(nullable: false),
                        Confirmation = c.String(),
                        DoctorID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.DoctorInfoes", t => t.DoctorID, cascadeDelete: true)
                .Index(t => t.DoctorID);
            
            CreateTable(
                "dbo.DoctorInfoes",
                c => new
                    {
                        DoctorID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        PracticeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DoctorID)
                .ForeignKey("dbo.Practices", t => t.PracticeID, cascadeDelete: true)
                .Index(t => t.PracticeID);
            
            CreateTable(
                "dbo.Practices",
                c => new
                    {
                        PracticeID = c.Int(nullable: false, identity: true),
                        PracticeName = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PracticeID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                        DepartmentPhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Careers",
                c => new
                    {
                        CareerID = c.Int(nullable: false, identity: true),
                        CareerName = c.String(),
                        CareerPayRange = c.String(),
                        CareerType = c.String(),
                        CareerDescription = c.String(),
                    })
                .PrimaryKey(t => t.CareerID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        DonationID = c.Int(nullable: false, identity: true),
                        DonationAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DonationDate = c.DateTime(nullable: false),
                        UserID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DonationID)
                .ForeignKey("dbo.UserInfoes", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        StreetNumber = c.Int(nullable: false),
                        Address = c.String(),
                        PostalCode = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.DonorInfoes",
                c => new
                    {
                        DonorID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        StreetNumber = c.Int(nullable: false),
                        Address = c.String(),
                        PostalCode = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.DonorID);
            
            CreateTable(
                "dbo.FaqCategories",
                c => new
                    {
                        FaqCategoryID = c.Int(nullable: false, identity: true),
                        FaqID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FaqCategoryID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Faqs", t => t.FaqID, cascadeDelete: true)
                .Index(t => t.FaqID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Faqs",
                c => new
                    {
                        FaqID = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.FaqID);
            
            CreateTable(
                "dbo.HospitalTours",
                c => new
                    {
                        HospitalTourID = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                        Description = c.String(),
                        VideoPath = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HospitalTourID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.JobApplications",
                c => new
                    {
                        JobApplicationID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ResumePath = c.String(),
                        CoverLetterPath = c.String(),
                        CareerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JobApplicationID)
                .ForeignKey("dbo.Careers", t => t.CareerID, cascadeDelete: true)
                .Index(t => t.CareerID);
            
            CreateTable(
                "dbo.NewsBanners",
                c => new
                    {
                        NewsBannerID = c.Int(nullable: false, identity: true),
                        NewsTitle = c.String(),
                        NewsBody = c.String(),
                        ImagePath = c.String(),
                    })
                .PrimaryKey(t => t.NewsBannerID);
            
            CreateTable(
                "dbo.ReportsTables",
                c => new
                    {
                        HospitalTourID = c.Int(nullable: false, identity: true),
                        ReportName = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HospitalTourID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.VolunteerPositions",
                c => new
                    {
                        CvpID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CvpID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        VolunteerID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DepartmentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.VolunteerID)
                .ForeignKey("dbo.Departments", t => t.DepartmentID, cascadeDelete: true)
                .Index(t => t.DepartmentID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Volunteers", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.VolunteerPositions", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ReportsTables", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.JobApplications", "CareerID", "dbo.Careers");
            DropForeignKey("dbo.HospitalTours", "DepartmentID", "dbo.Departments");
            DropForeignKey("dbo.FaqCategories", "FaqID", "dbo.Faqs");
            DropForeignKey("dbo.FaqCategories", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Donations", "UserID", "dbo.UserInfoes");
            DropForeignKey("dbo.AppointmentBookings", "DoctorID", "dbo.DoctorInfoes");
            DropForeignKey("dbo.DoctorInfoes", "PracticeID", "dbo.Practices");
            DropForeignKey("dbo.Practices", "DepartmentID", "dbo.Departments");
            DropIndex("dbo.Volunteers", new[] { "DepartmentID" });
            DropIndex("dbo.VolunteerPositions", new[] { "DepartmentID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ReportsTables", new[] { "DepartmentID" });
            DropIndex("dbo.JobApplications", new[] { "CareerID" });
            DropIndex("dbo.HospitalTours", new[] { "DepartmentID" });
            DropIndex("dbo.FaqCategories", new[] { "CategoryID" });
            DropIndex("dbo.FaqCategories", new[] { "FaqID" });
            DropIndex("dbo.Donations", new[] { "UserID" });
            DropIndex("dbo.Practices", new[] { "DepartmentID" });
            DropIndex("dbo.DoctorInfoes", new[] { "PracticeID" });
            DropIndex("dbo.AppointmentBookings", new[] { "DoctorID" });
            DropTable("dbo.Volunteers");
            DropTable("dbo.VolunteerPositions");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReportsTables");
            DropTable("dbo.NewsBanners");
            DropTable("dbo.JobApplications");
            DropTable("dbo.HospitalTours");
            DropTable("dbo.Faqs");
            DropTable("dbo.FaqCategories");
            DropTable("dbo.DonorInfoes");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.Donations");
            DropTable("dbo.Categories");
            DropTable("dbo.Careers");
            DropTable("dbo.Departments");
            DropTable("dbo.Practices");
            DropTable("dbo.DoctorInfoes");
            DropTable("dbo.AppointmentBookings");
            DropTable("dbo.Abouts");
        }
    }
}
