namespace SensenbrennerHospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedFKfromDonation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Donations", "UserID", "dbo.UserInfoes");
            DropIndex("dbo.Donations", new[] { "UserID" });
            DropColumn("dbo.Donations", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Donations", "UserID", c => c.Int(nullable: false));
            CreateIndex("dbo.Donations", "UserID");
            AddForeignKey("dbo.Donations", "UserID", "dbo.UserInfoes", "UserID", cascadeDelete: true);
        }
    }
}
