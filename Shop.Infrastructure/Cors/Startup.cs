namespace Shop.Infrastructure.Cors
{
    public static class Startup
    {
        private const string CORS_POLICY_NAME = "*";
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            var corsConfig = Singleton<AppConfig>.Instance.Get<CorsConfig>();

            services.AddCors(option =>
            {
                option.AddPolicy(CORS_POLICY_NAME, policy =>
                {
                    policy
                     .AllowAnyHeader()
                     .AllowAnyMethod()
                     .WithOrigins(corsConfig.CorsOrigins.ToArray());
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app)
        {
            app.UseCors(CORS_POLICY_NAME);

            return app;
        }
    }
}
