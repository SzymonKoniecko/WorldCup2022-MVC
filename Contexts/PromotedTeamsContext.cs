using Microsoft.EntityFrameworkCore;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Contexts
{
    public class PromotedTeamsContext : DbContext
    {
        public PromotedTeamsContext(DbContextOptions<PromotedTeamsContext> options)
            : base(options)
        {
        }
        public DbSet<PromotedTeams> PromotedTeams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PromotedTeams>()
                .HasKey(m =>
                m.Id
                );
        }
    }
}
