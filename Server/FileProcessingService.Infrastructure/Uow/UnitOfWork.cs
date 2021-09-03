using FileProcessingService.Application.Common.Interfaces.Repository;
using FileProcessingService.Application.Common.Interfaces.Uow;
using FileProcessingService.Infrastructure.Repository;
using FileProcessingService.Persistence.Context;
using System;
using System.Threading.Tasks;

namespace FileProcessingService.Infrastructure.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        #region Private Fields
        private IProcessedFileContentRepository _processedFileContentRepository;
        private IDuplicateWordStatisticRepository _duplicateWordStatisticRepository;
        private IStatusMessageRepository _statusMessageRepository;

        private readonly FileProcessingContext _context;
        #endregion Private Fields

        public UnitOfWork(FileProcessingContext context)
        {
            _context = context;
        }

        #region Public Properties

        public IProcessedFileContentRepository ProcessedFileContentRepository
        {
            get
            {
                if(_processedFileContentRepository == null)
                {
                    _processedFileContentRepository = new ProcessedFileContentRepository(_context);
                }
                return _processedFileContentRepository;
            }
        }

        public IDuplicateWordStatisticRepository DuplicateWordStatisticRepository
        {
            get
            {
                if (_duplicateWordStatisticRepository == null)
                {
                    _duplicateWordStatisticRepository = new DuplicateWordStatisticsRepository(_context);
                }
                return _duplicateWordStatisticRepository;
            }
        }

        public IStatusMessageRepository StatusMessageRepository
        {
            get
            {
                if (_statusMessageRepository == null)
                {
                    _statusMessageRepository = new StatusMessageRepository(_context);
                }
                return _statusMessageRepository;
            }
        }
        #endregion Public Properties

        #region Methods

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

        #endregion Methods

        #region Dispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion Dispose
    }
}
