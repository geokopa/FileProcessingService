using FileProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileProcessingService.Persistence.Configurations
{
    public class ProcessedFileContentConfiguration : IEntityTypeConfiguration<ProcessedFileContent>
    {
        public void Configure(EntityTypeBuilder<ProcessedFileContent> builder)
        {
            builder.Property(t => t.ContentText)
                .IsRequired();
            builder.Property(t => t.ElementName)
                .HasMaxLength(350) //TODO: not sure that is enough
                .IsRequired();
            builder.Property(t => t.SessionId)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(t => t.CreatedAt)
                .IsRequired();

            builder.HasMany(x => x.DuplicateWordStatistics).WithOne(x => x.ProcessedFileContent).HasForeignKey(x => x.ProcessedFileContentId);
        }
    }
}
