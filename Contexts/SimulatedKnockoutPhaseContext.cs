using Microsoft.EntityFrameworkCore;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Contexts
{
    public class SimulatedKnockoutPhaseContext : DbContext
    {
        public SimulatedKnockoutPhaseContext(DbContextOptions<SimulatedKnockoutPhaseContext> options)
            : base(options)
        {
        }
        public DbSet<SimulatedKnockoutPhase> SimulatedKnockoutPhase { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SimulatedKnockoutPhase>()
                .HasKey(m =>
                m.Id
                );
        }
    }
}
