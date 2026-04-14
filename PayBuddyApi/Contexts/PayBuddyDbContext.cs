using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PayBuddyApi.Models;

namespace PayBuddyApi.Contexts
{
    public class PayBuddyDbContext : IdentityDbContext<AppUser>
    {
        public PayBuddyDbContext(DbContextOptions<PayBuddyDbContext> options) : base(options)
        {
            
        }

        public DbSet<Debt> Debts { get; set; }
        public DbSet<Friendship> Friendships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.Friend)
                .WithMany()
                .HasForeignKey(f => f.FriendId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Debt>()
                .HasOne(d => d.Creditor)
                .WithMany()
                .HasForeignKey(d => d.CreditorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Debt>()
                .HasOne(d => d.Debtor)
                .WithMany()
                .HasForeignKey(d => d.DebtorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Debt>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);
        }
    }
}
