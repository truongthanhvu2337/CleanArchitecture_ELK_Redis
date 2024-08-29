namespace API.Configuration
{
    public static class HealthChecksConfiguration
    {
        public static IServiceCollection ConfigureHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("Redis:HostName").Get<string>();
            var sqlserver = configuration.GetSection("ConnectionStrings:local").Get<string>();
            services.AddHealthChecks()
                    .AddRedis(connectionString!)
                    .AddSqlServer(sqlserver!);

            return services;
        }
    }
}
