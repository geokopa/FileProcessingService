using FileProcessingService.Application.Common.Interfaces.Repository;
using FileProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProcessingService.Infrastructure.Repository
{
    public class StatusMessageRepository : Repository<StatusMessage>, IStatusMessageRepository
    {
        public StatusMessageRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<StatusMessage>> GetBySessionId(string sessionId, DateTime? dateTime)
        {
            var query = Query(x => x.SessionId == sessionId);

            if (dateTime.HasValue)
                query = query.Where(x => x.CreatedAt > dateTime.Value);

            return await query.ToListAsync();
        }
    }
}
