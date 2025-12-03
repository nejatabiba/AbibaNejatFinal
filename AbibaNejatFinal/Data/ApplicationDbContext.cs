using AbibaNejatFinal.Models;
using Microsoft.EntityFrameworkCore;

namespace AbibaNejatFinal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Anime> Anime { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
