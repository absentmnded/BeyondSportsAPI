using Microsoft.EntityFrameworkCore;
using BeyondSportsAPI.Models;

namespace BeyondSportsAPI.Data
{
    public class BeyondSportsDbContext : DbContext
    {

        public BeyondSportsDbContext(DbContextOptions<BeyondSportsDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Player> Players { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().ToTable("Players");
            modelBuilder.Entity<Team>().ToTable("Teams");

        }
    }
}
