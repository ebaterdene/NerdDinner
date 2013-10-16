namespace NerdDinner.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dinners",
                c => new
                    {
                        DinnerID = c.Int(nullable: false, identity: false),
                        Title = c.String(nullable: false),
                        EventDate = c.DateTime(nullable: false),
                        Address = c.String(),
                        HostedBy = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        ContactPhone = c.String(nullable: false),
                        Country = c.String(),
                        Latitude = c.String(),
                        Longtitude = c.String(),
                    })
                .PrimaryKey(t => t.DinnerID);
            
            CreateTable(
                "dbo.RSVPs",
                c => new
                    {
                        RsvpID = c.Int(nullable: false, identity: true),
                        DinnerID = c.Int(nullable: false),
                        AttendeeEmail = c.String(),
                    })
                .PrimaryKey(t => t.RsvpID)
                .ForeignKey("dbo.Dinners", t => t.DinnerID, cascadeDelete: true)
                .Index(t => t.DinnerID);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.RSVPs", new[] { "DinnerID" });
            DropForeignKey("dbo.RSVPs", "DinnerID", "dbo.Dinners");
            DropTable("dbo.RSVPs");
            DropTable("dbo.Dinners");
        }
    }
}
