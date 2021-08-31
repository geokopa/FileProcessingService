using FileProcessingService.Application.Common.Interfaces.Repository;
using FileProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FileProcessingService.Infrastructure.Repository
{
    public class ProcessedFileContentRepository : Repository<ProcessedFileContent>, IProcessedFileContentRepository
    {
        public ProcessedFileContentRepository(DbContext context) : base(context)
        {
        }

        public async Task<ProcessedFileContent> GetBySessionId(Guid id)
        {
            return await FirstOrDefaultAsync(x => x.SessionId == id);
        }
    }
}
