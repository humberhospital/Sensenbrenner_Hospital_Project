namespace SensenbrennerHospital.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class faqchange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FaqCategories", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.FaqCategories", "FaqID", "dbo.Faqs");
            DropIndex("dbo.FaqCategories", new[] { "FaqID" });
            DropIndex("dbo.FaqCategories", new[] { "CategoryID" });
            AddColumn("dbo.Faqs", "CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Faqs", "CategoryID");
            AddForeignKey("dbo.Faqs", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
            DropTable("dbo.FaqCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FaqCategories",
                c => new
                    {
                        FaqCategoryID = c.Int(nullable: false, identity: true),
                        FaqID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FaqCategoryID);
            
            DropForeignKey("dbo.Faqs", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Faqs", new[] { "CategoryID" });
            DropColumn("dbo.Faqs", "CategoryID");
            CreateIndex("dbo.FaqCategories", "CategoryID");
            CreateIndex("dbo.FaqCategories", "FaqID");
            AddForeignKey("dbo.FaqCategories", "FaqID", "dbo.Faqs", "FaqID", cascadeDelete: true);
            AddForeignKey("dbo.FaqCategories", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
        }
    }
}
