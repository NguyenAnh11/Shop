using Shop.Domain.Users;

namespace Shop.Application.Users.Services
{
    public class RoleService : AbstractService<Role>, IRoleService
    {
        public RoleService(ShopDbContext context) : base(context)
        {
        }

        public async Task<Role> GetRoleByNameAsync(string name, bool includeHidden = false)
        {
            if (name.IsEmpty())
                return null;

            var role = await Table
                .AsNoTracking()
                .ApplyActiveFilter(includeHidden)
                .FirstOrDefaultAsync(p => p.Name == name);

            return role;
        }

        public async Task<IList<Role>> GetRolesAsync(bool includeHidden = false)
        {
            var roles = await Table
                .AsNoTracking()
                .ApplyActiveFilter(includeHidden)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            return roles;
        }

        public async Task<IList<int>> GetRolesByUserAsync(User user, bool includeHidden = false)
        {
            Guard.IsNotNull(user, nameof(user));

            var query = Table.ApplyActiveFilter(includeHidden);

            var ids = await _context.Set<UserRole>()
                .Where(p => p.UserId == user.Id)
                .Join(
                  query,
                  ur => ur.RoleId,
                  r => r.Id,
                  (ur, r) => r
                )
                .OrderBy(p => p.DisplayOrder)
                .Select(p => p.Id)
                .ToListAsync();

            return ids;
        }

        public async Task<bool> IsInRoleAsync(User user, string name, bool includeHidden = false)
        {
            Guard.IsNotNull(user, nameof(user));
            Guard.IsNotNullOrEmpty(name, nameof(name));

            var role = await Table
                .AsNoTracking()
                .ApplyActiveFilter(includeHidden)
                .FirstOrDefaultAsync(p => p.Name == name);

            if(role == null)
                return false;

            return await _context.Set<UserRole>().AnyAsync(p => p.UserId == user.Id && p.RoleId == role.Id);
        }

        public async Task<bool> IsAdminAsync(User user)
            => await IsInRoleAsync(user, "ADMINSTRATOR");

        public async Task<bool> IsRegisterAsync(User user)
            => await IsInRoleAsync(user, "REGISTER");

        public async Task<bool> IsVendorAsync(User user)
            => await IsInRoleAsync(user, "VENDOR");
    }
}
