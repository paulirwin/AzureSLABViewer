using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AzureSLABViewer.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<StorageConnection> StorageConnections { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("SLABViewerUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("SLABViewerRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("SLABViewerUserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("SLABViewerUserLogins");
            modelBuilder.Entity<IdentityUserRole>().ToTable("SLABViewerUserRoles");
            modelBuilder.Entity<StorageConnection>().ToTable("SLABViewerStorageConnections");
        }
    }
}