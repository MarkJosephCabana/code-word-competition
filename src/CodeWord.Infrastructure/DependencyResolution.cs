using CodeWord.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CodeWord.Infrastructure
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CodeWordDBContext>(cfg =>
            {
                cfg.UseSqlServer(configuration.GetConnectionString(nameof(CodeWordDBContext)),
                    sqlServerOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly("CodeWord.API");
                        sqlOptions.MigrationsHistoryTable("__EFMirgationsHistory", "efcore");
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });

            }, ServiceLifetime.Scoped);

            services.AddScoped<ICompetitionRepository, CompetitionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
