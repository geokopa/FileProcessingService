using FileProcessingService.Application.Common.Interfaces.Uow;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessingService.Application.ProcessedFileContent.Queries
{
    public class GetProcessedFileContentQuery : IRequest<FileProcessingService.Domain.Entities.ProcessedFileContent>
    {
        public Guid SessionId { get; set; }
    }

    public class GetProcessedFileContentQueryHandler : IRequestHandler<GetProcessedFileContentQuery, FileProcessingService.Domain.Entities.ProcessedFileContent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProcessedFileContentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Domain.Entities.ProcessedFileContent> Handle(GetProcessedFileContentQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ProcessedFileContentRepository.GetBySessionId(request.SessionId);
        }
    }
}
