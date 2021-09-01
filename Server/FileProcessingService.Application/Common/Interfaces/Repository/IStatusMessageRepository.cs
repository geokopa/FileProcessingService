using FileProcessingService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileProcessingService.Application.Common.Interfaces.Repository
{
    public interface IStatusMessageRepository : IRepository<StatusMessage>
    {
        Task<IEnumerable<StatusMessage>> GetBySessionId(string sessionId, DateTime? dateTime);
    }
}
