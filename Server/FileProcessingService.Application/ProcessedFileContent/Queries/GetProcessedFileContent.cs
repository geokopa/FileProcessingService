using FileProcessingService.Application.Common.Interfaces.Uow;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessingService.Application.ProcessedFileContent.Queries
{
    public class GetProcessedFileContentQuery : IRequest<IEnumerable<FileProcessingService.Domain.Entities.ProcessedFileContent>>
    {
        public string SessionId { get; set; }
    }

    public class GetProcessedFileContentQueryHandler : IRequestHandler<GetProcessedFileContentQuery, IEnumerable<FileProcessingService.Domain.Entities.ProcessedFileContent>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProcessedFileContentQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Domain.Entities.ProcessedFileContent>> Handle(GetProcessedFileContentQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.ProcessedFileContentRepository.GetBySessionId(request.SessionId);
        }
    }
}
