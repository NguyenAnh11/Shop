using System.Diagnostics;

namespace Shop.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var typeFinder = new AppTypeFinder();
            Singleton<ITypeFinder>.Instance = typeFinder;

            var configs = typeFinder
                .FindClassOfType<IConfig>()
                .Select(p => (IConfig)Activator.CreateInstance(p))
                .ToList();

            foreach (var config in configs)
                configuration.GetSection(config.Name).Bind(config);

            var appConfig = new AppConfig(configs);
            Singleton<AppConfig>.Instance = appConfig;

            services.AddHttpContextAccessor();

            services
                .AddCustomCors()
                .AddCustomMvc()
                .AddSecurityService();

            return services;
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            return app
                .UseCustomExceptionHandler(env)
                .UseCustomCors()
                .UseRouting()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = async (context) =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>();
                    var error = exception.Error;
                    if (error != null)
                    {
                        context.Response.ContentType = "application/json+problem";
                        var problem = new ProblemDetails()
                        {
                            Status = StatusCodes.Status500InternalServerError
                        };
                        if (env.IsDevelopment())
                        {
                            problem.Title = error.Message;
                            problem.Detail = error.StackTrace;
                        }
                        else
                        {
                            problem.Title = "Internal Server Error";
                            problem.Detail = null;
                        }
                        var tracedId = Activity.Current?.Id ?? context.TraceIdentifier;
                        if (tracedId != null)
                        {
                            problem.Extensions["TraceId"] = tracedId;
                        }
                        await context.Response.WriteAsJsonAsync(problem);
                    }
                }
            });

            return app;
        }
    }
}
