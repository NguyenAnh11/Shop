using Shop.Domain.Users;

namespace Shop.Application.Users.Services
{
    public class UserFieldService : AbstractService<UserFied>, IUserFieldService
    {
        public UserFieldService(ShopDbContext context) : base(context)
        {
        }

        protected async Task<UserFied> GetUserFieldAsync(int userId, string field)
            => await Table.FirstOrDefaultAsync(p => p.UserId == userId && p.Field == field);

        public async Task<TType> GetFieldAsync<TType>(User user, string field)
        {
            Guard.IsNotNull(user, nameof(user));
            Guard.IsNotNullOrEmpty(field, nameof(field));

            var uf = await GetUserFieldAsync(user.Id, field);

            if (uf == null)
                return default;

            return uf.Value.ChangeType<TType>();
        }

        public async Task SaveFieldAsync<TType>(User user, TType value, string field)
        {
            Guard.IsNotNull(user, nameof(user));
            Guard.IsNotNullOrEmpty(field, nameof(field));

            var valueStr = value.ChangeType<string>();

            var uf = await GetUserFieldAsync(user.Id, field);

            if(uf != null)
            {
                if (valueStr.IsEmpty())
                    Table.Remove(uf);

                else if (valueStr.Equals(uf.Value))
                    return;

                else
                    uf.Value = valueStr;
            }
            else
            {
                if (valueStr.IsEmpty())
                    return;

                uf = new UserFied
                {
                    UserId = user.Id,
                    Field = field,
                    Value = valueStr
                };

                await Table.AddAsync(uf);
            }

            await _context.SaveChangesAsync();
        }
    }
}
