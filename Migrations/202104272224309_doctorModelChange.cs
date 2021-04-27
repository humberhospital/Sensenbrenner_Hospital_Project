namespace SensenbrennerHospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class doctorModelChange : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Departments", "DepartmentName", c => c.String(nullable: false));
            AlterColumn("dbo.Departments", "DepartmentPhoneNumber", c => c.String(nullable: false));
            AlterColumn("dbo.Categories", "CategoryName", c => c.String(nullable: false));
            AlterColumn("dbo.Faqs", "Question", c => c.String(nullable: false));
            AlterColumn("dbo.Faqs", "Answer", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Faqs", "Answer", c => c.String());
            AlterColumn("dbo.Faqs", "Question", c => c.String());
            AlterColumn("dbo.Categories", "CategoryName", c => c.String());
            AlterColumn("dbo.Departments", "DepartmentPhoneNumber", c => c.String());
            AlterColumn("dbo.Departments", "DepartmentName", c => c.String());
        }
    }
}
