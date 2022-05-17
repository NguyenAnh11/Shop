using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Shop.Application.Infrastructure.Data.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void ApplyGlobalFilter<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> predicate)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(entity => typeof(T).IsAssignableFrom(entity.ClrType))
                .Select(entity => entity.ClrType)
                .ToList();

            foreach (var entity in entities)
            {
                var param = Expression.Parameter(entity);
                var body = ReplacingExpressionVisitor.Replace(predicate.Parameters.Single(), param, predicate.Body);
                modelBuilder.Entity(entity).HasQueryFilter(LambdaExpression.Lambda(body));
            }
        }
    }
}
