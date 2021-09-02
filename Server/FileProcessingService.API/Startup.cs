using FileProcessingService.API.BackgroundServices;
using FileProcessingService.Application;
using FileProcessingService.Infrastructure;
using FileProcessingService.Persistence;
using FileProcessingService.Persistence.Context;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;

namespace FileProcessingService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();
            services.AddProblemDetails();

            services.AddHealthChecks().AddDbContextCheck<FileProcessingContext>();

            services.AddInfrastructure();
            services.AddPersistence(Configuration);
            services.AddApplication();
            services.AddUnitOfWork();

            services.AddCors(option =>
            {
                option.AddPolicy("CORS", x => {
                    x.AllowAnyMethod();
                    x.AllowAnyOrigin();
                    x.AllowAnyHeader();
                });
            });

            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "File Processing API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Email = "kopadze@gmail.com",
                        Name = "George Kopadze",
                        Url = new Uri("https://kopadze.ge")
                    },
                    Description = "File Processing API contains set of functionality to get content from uploaded file with statistical information"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            UpdateDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FileProcessingService.API v1"));
            }
            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            using var context = serviceScope.ServiceProvider.GetService<FileProcessingContext>();
            if (context.Database.IsSqlServer())
            {
                context.Database.EnsureCreated();
                context.Database.Migrate();
            }
        }
    }
}
