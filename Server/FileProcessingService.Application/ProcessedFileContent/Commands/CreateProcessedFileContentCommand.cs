using FileProcessingService.Application.Common.Interfaces.Uow;
using FileProcessingService.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessingService.Application.ProcessedFileContent.Commands
{
    public class CreateProcessedFileContentCommand : IRequest<Unit>
    {
        public string SessionId { get; private set; }
        public string ContentText { get; private set; }
        public string ElementName { get; private set; }
        public IEnumerable<DuplicateWord> Duplicates { get; private set; }

        public CreateProcessedFileContentCommand(string sessionId, string contentText, string elementName, IEnumerable<DuplicateWord> duplicates)
        {
            SessionId = sessionId;
            ContentText = contentText;
            ElementName = elementName;
            Duplicates = duplicates;
        }
    }

    public class CreateProcessedFileContentCommandHandler : IRequestHandler<CreateProcessedFileContentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateProcessedFileContentCommandHandler(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateProcessedFileContentCommand request, CancellationToken cancellationToken)
        {
            var entity = new Domain.Entities.ProcessedFileContent
            {
                ContentText = request.ContentText,
                CreatedAt = DateTime.Now,
                ElementName = request.ElementName,
                SessionId = request.SessionId
            };

            entity.DuplicateWordStatistics.AddRange(request.Duplicates.Select(x => new Domain.Entities.DuplicateWordStatistic
            {
                CreatedAt = DateTime.Now,
                DuplicateCount = x.Count,
                DuplicateWord = x.Word,
                SessionId = request.SessionId
            }));

            await _unitOfWork.ProcessedFileContentRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            return Unit.Value;
        }
    }
}
