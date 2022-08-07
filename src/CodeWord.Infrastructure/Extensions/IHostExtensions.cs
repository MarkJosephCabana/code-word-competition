using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeWord.Infrastructure.Extensions
{
    public static class IHostExtensions
    {
        public static IHost UseDBSeeding(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetService<CodeWordDBContext>();

            new CodeWordDBContextSeeder()
                .SeedAsync(context)
                .Wait();

            return host;
        }
    }
}
