namespace SensenbrennerHospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class databaseFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Volunteers", "VolunteerHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Volunteers", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Volunteers", "PicExtension");
            DropColumn("dbo.Volunteers", "VolunteerHasPic");
        }
    }
}
