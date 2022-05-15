using Microsoft.EntityFrameworkCore;
using Shop.Application.Infrastructure;
using Shop.Application.Infrastructure.TypeFinder;
using Shop.SharedKernel;
using Shop.SharedKernel.Interfaces;
using Shop.Infrastructure.Persistence.Extensions;

namespace Shop.Infrastructure.Persistence.Context
{
    public class ShopContext : DbContext
    {
        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
                modelBuilder.Entity(entity);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(p => p.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShopContext).Assembly);

            modelBuilder.ApplyGlobalFilter<ISoftDelete>(p => !p.IsDelete);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is ISoftDelete _)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Property("IsDelete").CurrentValue = false;
                            break;
                        case EntityState.Deleted:
                            entry.Property("IsDelete").CurrentValue = true;
                            entry.State = EntityState.Modified;
                            break;
                    }
                }

                if (entry.Entity is IClock _)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Property("CreateUtc").CurrentValue = DateTime.UtcNow;
                            break;
                        case EntityState.Modified:
                            entry.Property("UpdateUtc").CurrentValue = DateTime.UtcNow;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
