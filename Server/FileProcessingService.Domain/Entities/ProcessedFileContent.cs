using System;

namespace FileProcessingService.Domain.Entities
{
    public class ProcessedFileContent : BaseEntity
    {
        public Guid SessionId { get; set; }
        public string ContentText { get; set; }
        public string ElementName { get; set; }
        public string DuplicateWord { get; set; }
        public int DuplicateCount { get; set; }
    }
}
