using FileProcessingService.Application.Common.Interfaces.Repository;
using System;
using System.Threading.Tasks;

namespace FileProcessingService.Application.Common.Interfaces.Uow
{
    public interface IUnitOfWork : IDisposable
    {
        #region Properties
        IProcessedFileContentRepository ProcessedFileContentRepository { get; }
        IDuplicateWordStatisticRepository DuplicateWordStatisticRepository { get; }
        IStatusMessageRepository StatusMessageRepository{ get; }
        #endregion Properties

        #region Methods
        Task CompleteAsync();
        #endregion Methods
    }
}
