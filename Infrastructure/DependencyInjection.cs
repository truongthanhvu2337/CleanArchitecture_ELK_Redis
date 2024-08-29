using Domain.Repository;
using Domain.Repository.UnitOfWork;
using Infrastructure.Caching;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //Add DBcontext
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("local"),
                    b =>
                    {
                        b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        b.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    });
                options.UseLazyLoadingProxies();
            });

            //Add redis
            services.AddStackExchangeRedisCache(options =>
            {
                var redisConnection = configuration["Redis:HostName"];
                //var redisPassword = configuration["Redis:Password"];
                //options.Configuration = $"{redisConnection},password={redisPassword}";
                options.Configuration = redisConnection;
            });
            services.AddDistributedMemoryCache();

            //Add JWTconfig
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration.GetSection("JWTSetting:Issuer").Get<string>(),
                    ValidAudience = configuration.GetSection("JWTSetting:Audience").Get<string>(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWTSetting:Securitykey").Get<string>())),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    //ClockSkew = TimeSpan.Zero
                };
            });


            services.AddSingleton<IRedisCaching,  RedisCaching>();
            services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUserRepo, CustomerRepository>();
            services.AddScoped<IOrderRepo, OrderRepository>();

            return services;
        }
    }
}
