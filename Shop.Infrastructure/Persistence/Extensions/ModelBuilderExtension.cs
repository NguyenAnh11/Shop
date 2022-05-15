using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Shop.Infrastructure.Persistence.Extensions
{
    public static class ModelBuilderExtension
    {
        public static void ApplyGlobalFilter<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> expr)
        {
            var entities = modelBuilder.Model
                .GetEntityTypes()
                .Where(p => typeof(T).IsAssignableFrom(p.ClrType))
                .Select(p => p.ClrType)
                .ToList();

            foreach(var entity in entities)
            {
                var param = Expression.Parameter(entity);
                var body = ReplacingExpressionVisitor.Replace(expr.Parameters[0], param, expr.Body);
                modelBuilder.Entity(entity).HasQueryFilter(LabelExpression.Lambda(body, param));
            }
        }
    }
}