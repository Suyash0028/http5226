namespace n01629153_Event_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForeignKey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Events", "SponsorId", c => c.Int(nullable: false));
            CreateIndex("dbo.Events", "UserId");
            CreateIndex("dbo.Events", "SponsorId");
            AddForeignKey("dbo.Events", "UserId", "dbo.Users", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Events", "SponsorId", "dbo.Sponsors", "SponsorId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Events", "SponsorId", "dbo.Sponsors");
            DropForeignKey("dbo.Events", "UserId", "dbo.Users");
            DropIndex("dbo.Events", new[] { "SponsorId" });
            DropIndex("dbo.Events", new[] { "UserId" });
            DropColumn("dbo.Events", "SponsorId");
            DropColumn("dbo.Events", "UserId");
        }
    }
}
