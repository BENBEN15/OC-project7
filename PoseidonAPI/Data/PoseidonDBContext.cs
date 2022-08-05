using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PoseidonAPI.Model;

namespace PoseidonAPI.Data
{
    public class PoseidonDBContext : IdentityDbContext<IdentityUser>
    {
        public PoseidonDBContext() { }
        public PoseidonDBContext(DbContextOptions<PoseidonDBContext> options) : base(options) { }

        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<CurvePoint> CurvePoints { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Rule> Rules { get; set; }
        public virtual DbSet<Trade> Trades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Call Identity OnModelCreating first
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bid>(entity =>
            {
                entity.HasKey(b => b.BidId);
            });

            modelBuilder.Entity<CurvePoint>(entity =>
            {
                entity.HasKey(c => c.CurvePointId);
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(r => r.RatingId);
            });

            modelBuilder.Entity<Rule>(entity =>
            {
                entity.HasKey(r => r.RuleId);
            });

            modelBuilder.Entity<Trade>(entity =>
            {
                entity.HasKey(t => t.TradeId);
            });
        }
    }
}
