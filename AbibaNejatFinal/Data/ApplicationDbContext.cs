using AbibaNejatFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace AbibaNejatFinal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountAnime> AccountAnimes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountAnime>()
                .HasKey(aa => new { aa.AccountId, aa.MalId });

            modelBuilder.Entity<AccountAnime>()
                .HasOne(aa => aa.Account)
                .WithMany(a => a.AccountAnimes)
                .HasForeignKey(aa => aa.AccountId);
        }
    }
}
