using System;
using System.Collections;
using System.Collections.Generic;

namespace FileProcessingService.Domain.Entities
{
    public class ProcessedFileContent : BaseEntity
    {
        public ProcessedFileContent()
        {
            DuplicateWordStatistics = new List<DuplicateWordStatistic>();
        }

        public string SessionId { get; set; }
        public string ContentText { get; set; }
        public string ElementName { get; set; }

        public virtual List<DuplicateWordStatistic> DuplicateWordStatistics { get; set; }
    }
}
