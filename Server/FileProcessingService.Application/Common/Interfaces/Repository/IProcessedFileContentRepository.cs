using System;
using System.Threading.Tasks;
using FileProcessingService.Domain.Entities;

namespace FileProcessingService.Application.Common.Interfaces.Repository
{
    public interface IProcessedFileContentRepository : IRepository<Domain.Entities.ProcessedFileContent>
    {
        Task<Domain.Entities.ProcessedFileContent> GetBySessionId(Guid id);
    }
}
