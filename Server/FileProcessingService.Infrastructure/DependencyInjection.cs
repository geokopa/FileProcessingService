using FileProcessingService.Application.Common.Interfaces.Uow;
using FileProcessingService.Infrastructure.Uow;
using Microsoft.Extensions.DependencyInjection;

namespace FileProcessingService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
