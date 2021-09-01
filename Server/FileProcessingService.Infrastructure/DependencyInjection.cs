using FileProcessingService.Application.Common.Interfaces.Processors;
using FileProcessingService.Application.Common.Interfaces.Uow;
using FileProcessingService.Infrastructure.Processors;
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

        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IXmlDocumentProcessor, XmlDocumentProcessor>();
            return services;
        }
    }
}
