using FileProcessingService.Application.Common.Interfaces.Uow;
using FileProcessingService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessingService.Application.StatusMessages.Queries
{
    public class GetStatusMessageBySessionQuery : IRequest<IEnumerable<StatusMessage>>
    {
        public string SessionId { get; private set; }
        public DateTime? RequestTime { get; private set; }

        public GetStatusMessageBySessionQuery(DateTime? requestTime, string sessionId)
        {
            RequestTime = requestTime;
            SessionId = sessionId;
        }
    }

    public class GetStatusMessageBySessionQueryHandler : IRequestHandler<GetStatusMessageBySessionQuery, IEnumerable<StatusMessage>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStatusMessageBySessionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<StatusMessage>> Handle(GetStatusMessageBySessionQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.StatusMessageRepository.GetBySessionId(request.SessionId, request.RequestTime);
        }
    }
}
