using Microsoft.EntityFrameworkCore;
using WorldCup2022_MVC.Models;

namespace WorldCup2022_MVC.Contexts
{
    public class GroupStageContext : DbContext
    {
        public GroupStageContext(DbContextOptions options) : base(options) { }
        public DbSet<GroupStage> GroupStage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupStage>()
                .HasKey(gs =>
                gs.groupStageId
                );
        }
    }
}
