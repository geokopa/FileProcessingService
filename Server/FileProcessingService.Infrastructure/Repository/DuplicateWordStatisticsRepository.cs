using FileProcessingService.Application.Common.Interfaces.Repository;
using FileProcessingService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileProcessingService.Infrastructure.Repository
{
    public class DuplicateWordStatisticsRepository : Repository<DuplicateWordStatistic>, IDuplicateWordStatisticRepository
    {
        public DuplicateWordStatisticsRepository(DbContext context) : base(context)
        {
        }
    }
}
