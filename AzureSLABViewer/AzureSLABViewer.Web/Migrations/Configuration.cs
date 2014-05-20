using AzureSLABViewer.Web.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace AzureSLABViewer.Web.Migrations
{    
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "AzureSLABViewer.Web.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            context.StorageConnections.AddOrUpdate(i => i.ConnectionString, new StorageConnection { DisplayName = "Development Storage", ConnectionString = "UseDevelopmentStorage=true" });
            context.SaveChanges();
        }
    }
}
