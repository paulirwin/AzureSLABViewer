namespace AzureSLABViewer.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SLABViewerStorageConnections",
                c => new
                    {
                        StorageConnectionID = c.Int(nullable: false, identity: true),
                        DisplayName = c.String(nullable: false, maxLength: 100),
                        ConnectionString = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.StorageConnectionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SLABViewerStorageConnections");
        }
    }
}
