namespace POSTmanager
{
    using Microsoft.EntityFrameworkCore;
    using POSTmanager.Models;

    internal class PmDbContext : DbContext
    {
        public DbSet<Rest> Rests;
        public DbSet<User> Users;
        public DbSet<UserRest> UserRests;

        public PmDbContext() 
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = post_manager_base.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<UserRest>()
                .HasKey(sc => new {sc.UserId, sc.RestId});

        }
    }
}
