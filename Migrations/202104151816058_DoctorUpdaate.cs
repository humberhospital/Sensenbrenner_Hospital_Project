namespace SensenbrennerHospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DoctorUpdaate : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DoctorInfoes", newName: "Doctors");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Doctors", newName: "DoctorInfoes");
        }
    }
}
