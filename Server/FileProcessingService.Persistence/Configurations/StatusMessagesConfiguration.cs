using FileProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileProcessingService.Persistence.Configurations
{
    public class StatusMessagesConfiguration : IEntityTypeConfiguration<StatusMessage>
    {
        public void Configure(EntityTypeBuilder<StatusMessage> builder)
        {
            builder.Property(x => x.Message).HasMaxLength(2500).IsRequired();
            builder.Property(x => x.SessionId).HasMaxLength(50).IsRequired();
        }
    }
}
