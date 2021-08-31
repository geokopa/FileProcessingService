using System;
using System.Collections.Generic;
using System.Text;
using FileProcessingService.Application.Common.Interfaces;
using FileProcessingService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileProcessingService.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<FileProcessingContext>(options =>
                    options.UseInMemoryDatabase("FileProcessingDb"));
            }
            else
            {
                services.AddDbContext<FileProcessingContext>(options =>
                    options.UseSqlServer(
                        configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(FileProcessingContext).Assembly.FullName)));
            }

            return services;
        }
    }
}
