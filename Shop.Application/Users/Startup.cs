using Shop.Application.Users.Services;

namespace Shop.Application.Users
{
    public static class Startup
    {
        public static IServiceCollection AddUsersModuleService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserFieldService, UserFieldService>();

            return services;
        }
    }
}
