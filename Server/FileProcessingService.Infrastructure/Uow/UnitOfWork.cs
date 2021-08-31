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
        private IProcessedFileContentRepository _processedFileContentRepository;
        private readonly FileProcessingContext _context;

        public UnitOfWork(FileProcessingContext context)
        {
            _context = context;
        }

        public IProcessedFileContentRepository ProcessedFileContentRepository => _processedFileContentRepository ?? new ProcessedFileContentRepository(_context);

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }

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
