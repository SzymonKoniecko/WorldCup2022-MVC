using Microsoft.EntityFrameworkCore;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Contexts
{
    public class TeamContext : DbContext
    {
        public TeamContext(DbContextOptions options) : base(options) { }
        public DbSet<Team> Team { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Team>()
                .HasKey(t =>
                t.teamId
                );
        }
    }
}
