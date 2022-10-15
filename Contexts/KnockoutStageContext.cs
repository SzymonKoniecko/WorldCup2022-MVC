using Microsoft.EntityFrameworkCore;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Contexts
{
    public class KnockoutStageContext :DbContext
    {
        public KnockoutStageContext(DbContextOptions<KnockoutStageContext> options)
            : base(options)
        {
        }
        public DbSet<KnockoutStage> KnocoutStage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KnockoutStage>()
                .HasKey(m =>
                m.KnockoutStageId
                );
        }
    }
}
