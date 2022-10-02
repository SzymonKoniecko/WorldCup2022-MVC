using Microsoft.EntityFrameworkCore;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Contexts
{
    public class MatchesContext : DbContext
    {
        public MatchesContext(DbContextOptions<MatchesContext> options)
            : base(options)
        {
        }
        public DbSet<Matches> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Matches>()
                .HasKey(m =>
                m.Id
                );
        }
    }
}
