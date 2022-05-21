using Shop.Domain.Users;

namespace Shop.Application.Users.Services
{
    public interface IRoleService : IAbstractService<Role>
    {
        Task<Role> GetRoleByIdAsync(int id, bool tracked = false);

        Task<Role> GetRoleByNameAsync(string name, bool includeHidden = false); 

        Task<IList<Role>> GetRolesAsync(bool includeHidden = false);

        Task<IList<int>> GetRolesByUserAsync(User user, bool includeHidden = true);

        Task<bool> IsInRoleAsync(User user, string name, bool includeHidden = true);
    }
}
