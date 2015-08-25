namespace TimeManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthDatas",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Login = c.String(),
                        Password = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AuthTokens", "AuthDataID", c => c.Int(nullable: false));
            AddColumn("dbo.Companies", "AuthDataID", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "AuthDataID", c => c.Int(nullable: false));
            CreateIndex("dbo.AuthTokens", "AuthDataID");
            CreateIndex("dbo.Companies", "AuthDataID");
            CreateIndex("dbo.Users", "AuthDataID");
            AddForeignKey("dbo.AuthTokens", "AuthDataID", "dbo.AuthDatas", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Companies", "AuthDataID", "dbo.AuthDatas", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Users", "AuthDataID", "dbo.AuthDatas", "ID", cascadeDelete: false);
            DropColumn("dbo.AuthTokens", "Type");
            DropColumn("dbo.AuthTokens", "LoginID");
            DropColumn("dbo.Companies", "Login");
            DropColumn("dbo.Companies", "Password");
            DropColumn("dbo.Users", "Login");
            DropColumn("dbo.Users", "Password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Password", c => c.String());
            AddColumn("dbo.Users", "Login", c => c.String());
            AddColumn("dbo.Companies", "Password", c => c.String());
            AddColumn("dbo.Companies", "Login", c => c.String());
            AddColumn("dbo.AuthTokens", "LoginID", c => c.Int(nullable: false));
            AddColumn("dbo.AuthTokens", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.Users", "AuthDataID", "dbo.AuthDatas");
            DropForeignKey("dbo.Companies", "AuthDataID", "dbo.AuthDatas");
            DropForeignKey("dbo.AuthTokens", "AuthDataID", "dbo.AuthDatas");
            DropIndex("dbo.Users", new[] { "AuthDataID" });
            DropIndex("dbo.Companies", new[] { "AuthDataID" });
            DropIndex("dbo.AuthTokens", new[] { "AuthDataID" });
            DropColumn("dbo.Users", "AuthDataID");
            DropColumn("dbo.Companies", "AuthDataID");
            DropColumn("dbo.AuthTokens", "AuthDataID");
            DropTable("dbo.AuthDatas");
        }
    }
}
