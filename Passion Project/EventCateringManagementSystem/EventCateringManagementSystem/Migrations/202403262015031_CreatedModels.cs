namespace EventCateringManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedModels : DbMigration
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
                    })
                .PrimaryKey(t => t.EventId);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        MenuID = c.Int(nullable: false, identity: true),
                        MenuTitle = c.String(),
                        MenuDescription = c.String(),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MenuID)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.MenuxFoods",
                c => new
                    {
                        MenuxFoodID = c.Int(nullable: false, identity: true),
                        MenuID = c.Int(nullable: false),
                        FoodID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MenuxFoodID)
                .ForeignKey("dbo.Foods", t => t.FoodID, cascadeDelete: true)
                .ForeignKey("dbo.Menus", t => t.MenuID, cascadeDelete: true)
                .Index(t => t.MenuID)
                .Index(t => t.FoodID);
            
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        FoodID = c.Int(nullable: false, identity: true),
                        FoodName = c.String(),
                        FoodDescription = c.String(),
                        FoodPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.FoodID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuxFoods", "MenuID", "dbo.Menus");
            DropForeignKey("dbo.MenuxFoods", "FoodID", "dbo.Foods");
            DropForeignKey("dbo.Menus", "EventId", "dbo.Events");
            DropIndex("dbo.MenuxFoods", new[] { "FoodID" });
            DropIndex("dbo.MenuxFoods", new[] { "MenuID" });
            DropIndex("dbo.Menus", new[] { "EventId" });
            DropTable("dbo.Foods");
            DropTable("dbo.MenuxFoods");
            DropTable("dbo.Menus");
            DropTable("dbo.Events");
        }
    }
}
