using FileProcessingService.Application.Common.Interfaces.Repository;
using FileProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileProcessingService.Infrastructure.Repository
{
    public class ProcessedFileContentRepository : Repository<ProcessedFileContent>, IProcessedFileContentRepository
    {
        public ProcessedFileContentRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ProcessedFileContent>> GetBySessionId(string id)
        {
            return await Query(x => x.SessionId == id).Include(x=>x.DuplicateWordStatistics).ToListAsync();
        }
    }
}
