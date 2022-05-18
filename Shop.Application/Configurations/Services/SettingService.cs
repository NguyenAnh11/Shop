using Shop.Domain.Configuration;

namespace Shop.Application.Configurations.Services
{
    public class SettingService : AbstractService<Setting>, ISettingService
    {
        public SettingService(ShopDbContext context) : base(context)
        {
        }
    }
}
