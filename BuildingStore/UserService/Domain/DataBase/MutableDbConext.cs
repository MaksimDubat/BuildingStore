using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Domain.DataBase
{
    /// <summary>
    /// Контекст для работы с БД.
    /// </summary>
    public class MutableDbConext : DbContext
    {
        public MutableDbConext(DbContextOptions<MutableDbConext> options) : base(options) { }

        public DbSet<User> User { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MutableDbConext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public bool HasPeddingChanges()
        {
            return ChangeTracker.HasChanges();
        }

    }
}
