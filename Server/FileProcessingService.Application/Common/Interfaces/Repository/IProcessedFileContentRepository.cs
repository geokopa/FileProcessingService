using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileProcessingService.Application.Common.Interfaces.Repository
{
    public interface IProcessedFileContentRepository : IRepository<Domain.Entities.ProcessedFileContent>
    {
        Task<IEnumerable<Domain.Entities.ProcessedFileContent>> GetBySessionId(string id);
    }
}
