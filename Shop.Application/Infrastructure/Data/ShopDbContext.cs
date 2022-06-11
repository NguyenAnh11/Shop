using Shop.Application.Infrastructure.Data.Extensions;

namespace Shop.Application.Infrastructure.Data
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors();

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var typeFinder = Singleton<ITypeFinder>.Instance;

            var entities = typeFinder.FindClassOfType<BaseEntity>();

            foreach (var entity in entities)
            {
                modelBuilder.Entity(entity);
            }

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(p => p.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopDbContext).Assembly);

            //seed data
            modelBuilder.SeedData();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry is ISoftDelete _)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Property(nameof(ISoftDelete.IsDelete)).CurrentValue = false;
                            break;

                        case EntityState.Deleted:
                            entry.Property(nameof(ISoftDelete.IsDelete)).CurrentValue = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }

                if (entry is IClock _)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Property(nameof(IClock.CreateUtc)).CurrentValue = DateTime.UtcNow;
                            break;

                        case EntityState.Modified:
                            entry.Property(nameof(IClock.UpdateUtc)).CurrentValue = DateTime.UtcNow;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
