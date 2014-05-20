namespace AzureSLABViewer.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SLABViewerRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SLABViewerUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.SLABViewerRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.SLABViewerUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.SLABViewerUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.SLABViewerUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SLABViewerUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SLABViewerUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.SLABViewerUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SLABViewerUserRoles", "UserId", "dbo.SLABViewerUsers");
            DropForeignKey("dbo.SLABViewerUserLogins", "UserId", "dbo.SLABViewerUsers");
            DropForeignKey("dbo.SLABViewerUserClaims", "UserId", "dbo.SLABViewerUsers");
            DropForeignKey("dbo.SLABViewerUserRoles", "RoleId", "dbo.SLABViewerRoles");
            DropIndex("dbo.SLABViewerUserLogins", new[] { "UserId" });
            DropIndex("dbo.SLABViewerUserClaims", new[] { "UserId" });
            DropIndex("dbo.SLABViewerUsers", "UserNameIndex");
            DropIndex("dbo.SLABViewerUserRoles", new[] { "RoleId" });
            DropIndex("dbo.SLABViewerUserRoles", new[] { "UserId" });
            DropIndex("dbo.SLABViewerRoles", "RoleNameIndex");
            DropTable("dbo.SLABViewerUserLogins");
            DropTable("dbo.SLABViewerUserClaims");
            DropTable("dbo.SLABViewerUsers");
            DropTable("dbo.SLABViewerUserRoles");
            DropTable("dbo.SLABViewerRoles");
        }
    }
}
