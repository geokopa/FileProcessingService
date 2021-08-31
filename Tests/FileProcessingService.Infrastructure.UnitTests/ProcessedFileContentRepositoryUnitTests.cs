using FileProcessingService.Application.Common.Interfaces.Uow;
using FileProcessingService.Infrastructure.Uow;
using FileProcessingService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FileProcessingService.Infrastructure.UnitTests
{
    public class ProcessedFileContentRepositoryUnitTests
    {
        private readonly UnitOfWork _uow;

        public ProcessedFileContentRepositoryUnitTests()
        {
            var options = new DbContextOptionsBuilder<FileProcessingContext>()
                .UseInMemoryDatabase(databaseName: "FileProcessingDb").Options;
            _uow = new UnitOfWork(new Persistence.Context.FileProcessingContext(options));
        }

        [Fact]
        public void AddAsyncShouldCreateNewEntity()
        {
            _uow.ProcessedFileContentRepository.AddAsync(new Domain.Entities.ProcessedFileContent { SessionId = Guid.NewGuid(), ContentText = "Sample Text", ElementName = "P", DuplicateCount = 0, DuplicateWord = String.Empty, CreatedAt = DateTime.Now });
        }

    }
}
