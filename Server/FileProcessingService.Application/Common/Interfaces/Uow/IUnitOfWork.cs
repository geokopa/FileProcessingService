using FileProcessingService.Application.Common.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace FileProcessingService.Application.Common.Interfaces.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        IProcessedFileContentRepository ProcessedFileContentRepository { get; }
        IDuplicateWordStatisticRepository DuplicateWordStatisticRepository { get; }

        Task CompleteAsync();
    }
}
