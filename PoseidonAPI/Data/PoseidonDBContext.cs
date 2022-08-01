using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoseidonAPI.Model;

namespace PoseidonAPI.Data
{
    public class PoseidonDBContext : IdentityDbContext<IdentityUser>
    {
        public PoseidonDBContext(DbContextOptions<PoseidonDBContext> options) : base(options) { }

        public virtual DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Call Identity OnModelCreating first
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(b => b.BidId);
            });
        }
    }
}
