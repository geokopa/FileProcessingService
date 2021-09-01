using FileProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileProcessingService.Persistence.Configurations
{
    class DuplicateWordStatisticsConfiguration : IEntityTypeConfiguration<DuplicateWordStatistic>
    {
        public void Configure(EntityTypeBuilder<DuplicateWordStatistic> builder)
        {
            builder.Property(x => x.DuplicateWord).HasMaxLength(250).IsRequired();
            builder.Property(x => x.DuplicateCount).IsRequired();
            builder.Property(x => x.SessionId).HasMaxLength(50).IsRequired();

        }
    }
}
