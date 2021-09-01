using FileProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FileProcessingService.Persistence.Context
{
    public class FileProcessingContext : DbContext
    {
        public FileProcessingContext(DbContextOptions<FileProcessingContext> options) : base(options)
        {
        }

        public DbSet<ProcessedFileContent> ProcessedFileContents { get; set; }
        public DbSet<DuplicateWordStatistic> DuplicateWordStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
