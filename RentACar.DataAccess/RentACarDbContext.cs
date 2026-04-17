using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RentACar.Entities;
using System.Reflection;

namespace RentACar.DataAccess
{
    public class RentACarDbContext : IdentityDbContext<IdentityUser>
    {
        public RentACarDbContext(DbContextOptions<RentACarDbContext> options)
            : base(options) { }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<SiteContact> SiteContacts { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<AboutContent> AboutContents { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<ContractDocument> ContractDocuments { get; set; }
        public DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}