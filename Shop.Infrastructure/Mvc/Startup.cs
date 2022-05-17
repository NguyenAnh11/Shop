using Newtonsoft.Json.Serialization;
using Shop.Infrastructure.Mvc.Filters;

namespace Shop.Infrastructure.Mvc
{
    public static class Startup
    {
        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services
                .AddControllersWithViews(option =>
                {
                    option.Filters.Add(new ExceptionFilter());
                })
                .ConfigureApiBehaviorOptions(option =>
                {
                    option.SuppressModelStateInvalidFilter = true;
                })
                .AddNewtonsoftJson(option =>
                {
                    option.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                })
                .AddFluentValidation(option =>
                {
                    option.AutomaticValidationEnabled = true;
                    option.ImplicitlyValidateChildProperties = true;

                    var assemblies = Singleton<ITypeFinder>.Instance.Assemblies();
                    option.RegisterValidatorsFromAssemblies(assemblies);
                });


            return services;
        }
    }
}
