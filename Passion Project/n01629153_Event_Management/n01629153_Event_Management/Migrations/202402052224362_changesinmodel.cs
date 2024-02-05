namespace n01629153_Event_Management.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesinmodel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Events", "EventName", c => c.String());
            AlterColumn("dbo.Events", "EventDescription", c => c.String());
            AlterColumn("dbo.Events", "EventLocation", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "EventLocation", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "EventDescription", c => c.String(nullable: false));
            AlterColumn("dbo.Events", "EventName", c => c.String(nullable: false));
        }
    }
}
