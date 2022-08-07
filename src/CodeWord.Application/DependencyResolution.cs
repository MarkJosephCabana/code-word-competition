using Microsoft.Extensions.DependencyInjection;
using MediatR;
using System.Reflection;
using CodeWord.Application.Common.Behaviours;

namespace CodeWord.Application
{
    public static class DependencyResolution
    {
        public static IServiceCollection AddAppication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

            return services;
        }
    }
}
