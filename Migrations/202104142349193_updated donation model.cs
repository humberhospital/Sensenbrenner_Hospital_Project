namespace SensenbrennerHospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateddonationmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Donations", "DonationMethod", c => c.String());
            AddColumn("dbo.Donations", "DonationText", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Donations", "DonationText");
            DropColumn("dbo.Donations", "DonationMethod");
        }
    }
}
