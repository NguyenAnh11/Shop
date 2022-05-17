using Shop.Domain.Users;

namespace Shop.Application.Users.Services
{
    public interface IRoleService : IAbstractService<Role>
    {
        Task<Role> GetRoleByNameAsync(string name, bool includeHidden = false); 

        Task<IList<Role>> GetRolesAsync(bool includeHidden = false);

        Task<IList<int>> GetRolesByUserAsync(User user, bool includeHidden = false);

        Task<bool> IsInRoleAsync(User user, string name, bool includeHidden = false);

        Task<bool> IsAdminAsync(User user);

        Task<bool> IsRegisterAsync(User user);

        Task<bool> IsVendorAsync(User user);
    }
}
