using FileProcessingService.Application.Common.Interfaces.Uow;
using FileProcessingService.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessingService.Application.DuplicateWords.Queries
{
    public class DuplicateWordStatisticQuery : IRequest<IEnumerable<DuplicateWordStatistic>>
    {
        public string SessionId { get; set; }
        public int? ProcessedFileContentId { get; set; }
    }


    public class DuplicateWordStatisticQueryHandler : IRequestHandler<DuplicateWordStatisticQuery, IEnumerable<DuplicateWordStatistic>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DuplicateWordStatisticQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DuplicateWordStatistic>> Handle(DuplicateWordStatisticQuery request, CancellationToken cancellationToken)
        {
            var query = await _unitOfWork.DuplicateWordStatisticRepository.FetchBy(x => x.SessionId == request.SessionId);

            //if (request.ProcessedFileContentId.HasValue)
            //    query = query.Where(x => x.ProcessedFileContentId == request.ProcessedFileContentId);

            return query;
        }
    }
}
