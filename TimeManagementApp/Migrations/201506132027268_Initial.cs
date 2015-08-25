namespace TimeManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        FullName = c.String(),
                        CompanyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: false)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.JoinUserProjects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: false)
                .Index(t => t.ProjectID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        CompanyID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Companies", t => t.CompanyID, cascadeDelete: true)
                .Index(t => t.CompanyID);
            
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BeginTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        Status = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        TagID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.RecordTags", t => t.TagID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.TagID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.RecordTags",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Color = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JoinUserProjects", "UserID", "dbo.Users");
            DropForeignKey("dbo.Records", "UserID", "dbo.Users");
            DropForeignKey("dbo.Records", "TagID", "dbo.RecordTags");
            DropForeignKey("dbo.Records", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.JoinUserProjects", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CompanyID", "dbo.Companies");
            DropForeignKey("dbo.Users", "CompanyID", "dbo.Companies");
            DropIndex("dbo.JoinUserProjects", new[] { "UserID" });
            DropIndex("dbo.Records", new[] { "UserID" });
            DropIndex("dbo.Records", new[] { "TagID" });
            DropIndex("dbo.Records", new[] { "ProjectID" });
            DropIndex("dbo.JoinUserProjects", new[] { "ProjectID" });
            DropIndex("dbo.Projects", new[] { "CompanyID" });
            DropIndex("dbo.Users", new[] { "CompanyID" });
            DropTable("dbo.RecordTags");
            DropTable("dbo.Records");
            DropTable("dbo.Projects");
            DropTable("dbo.JoinUserProjects");
            DropTable("dbo.Users");
            DropTable("dbo.Companies");
        }
    }
}
