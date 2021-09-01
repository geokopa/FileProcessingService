using FileProcessingService.Application.Common.Interfaces.Uow;
using FileProcessingService.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessingService.Application.StatusMessages.Commands
{
    public class CreateStatusMessageCommand : IRequest<Unit>
    {
        public string Message { get; private set; }
        public string SessionId { get; private set; }
        
        public CreateStatusMessageCommand(string sessionId, string message)
        {
            Message = message;
            SessionId = sessionId;
        }
    }

    public class CreateStatusMessageCommandHandler : IRequestHandler<CreateStatusMessageCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStatusMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateStatusMessageCommand request, CancellationToken cancellationToken)
        {

            await _unitOfWork.StatusMessageRepository.AddAsync(new StatusMessage { CreatedAt = DateTime.Now, Message = request.Message, SessionId = request.SessionId });
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
