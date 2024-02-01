namespace n01629153_EventManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class events1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        EventId = c.Int(nullable: false, identity: true),
                        EventName = c.String(),
                        EventDescription = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        EventLocation = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EventId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        Password = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Sponsors",
                c => new
                    {
                        SponsorId = c.Int(nullable: false, identity: true),
                        SponsorName = c.String(),
                        ContactPerson = c.String(),
                        ContactEmail = c.String(),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SponsorId)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sponsors", "EventId", "dbo.Events");
            DropForeignKey("dbo.Events", "UserId", "dbo.Users");
            DropIndex("dbo.Sponsors", new[] { "EventId" });
            DropIndex("dbo.Events", new[] { "UserId" });
            DropTable("dbo.Sponsors");
            DropTable("dbo.Users");
            DropTable("dbo.Events");
        }
    }
}
