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
        public string RequestTime { get; private set; }
        public DateTime? RequestDate { get; private set; }

        public GetStatusMessageBySessionQuery(string requestTime, string sessionId)
        {
            RequestTime = requestTime;
            SessionId = sessionId;

            if (!string.IsNullOrEmpty(requestTime))
            {
                if (DateTime.TryParse(requestTime, out DateTime result))
                {
                    RequestDate = result;
                }
            }
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
            return await _unitOfWork.StatusMessageRepository.GetBySessionId(request.SessionId, request.RequestDate);
        }
    }
}
