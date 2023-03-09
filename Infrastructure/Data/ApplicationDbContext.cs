using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlaneStore.Domain.Entities;

namespace PlaneStore.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        internal DbSet<Aircraft> Aircraft { get; set; }

        internal DbSet<Manufacturer> Manufacturers { get; set; }

        internal DbSet<Order> Orders { get; set; }
    }
}