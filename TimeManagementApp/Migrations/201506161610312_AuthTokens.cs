namespace TimeManagementApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuthTokens : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuthTokens",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Token = c.String(),
                        Type = c.Int(nullable: false),
                        LoginID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuthTokens");
        }
    }
}
