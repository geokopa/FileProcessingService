using System;

namespace FileProcessingService.Domain.Entities
{
    public class DuplicateWordStatistic : BaseEntity
    {
        public string DuplicateWord { get; set; }
        public int DuplicateCount { get; set; }
        public int ProcessedFileContentId { get; set; }
        public string SessionId { get; set; }

        public virtual ProcessedFileContent ProcessedFileContent { get; set; }
    }
}
