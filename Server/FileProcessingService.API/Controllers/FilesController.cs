using FileProcessingService.API.BackgroundServices;
using FileProcessingService.API.Models;
using FileProcessingService.Application.Common.Interfaces.Processors;
using FileProcessingService.Application.StatusMessages.Commands;
using FileProcessingService.Application.StatusMessages.Queries;
using FileProcessingService.Infrastructure.Extensions;
using FileProcessingService.Shared;
using FileProcessingService.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace FileProcessingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : BaseController
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IXmlDocumentProcessor _documentProcessor;

        public FilesController(IBackgroundTaskQueue backgroundTaskQueue, IXmlDocumentProcessor documentProcessor)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _documentProcessor = documentProcessor;
        }

        [HttpGet]
        public IActionResult Get()
        {
            throw new Exception("Sample exception to test global exception middleware");
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult Process([Required] IFormFile[] files, [Required] string sessionId, [Required] string elements)
        {
            if (files.Length == 0 || files.Any(x=>!IsValidXmlFile(x)))
                return BadRequest();

            Mediator.Send(new CreateStatusMessageCommand(sessionId, ResourceTexts.ProcessStarted));

            foreach (var file in files)
            {
                var stream = file.OpenReadStream();

                _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
                {
                    await _documentProcessor.Process(stream, elements.AsCleanedArray(), sessionId);
                });

                Mediator.Send(new CreateStatusMessageCommand(sessionId, _documentProcessor.GetMatchingElementSummery()));
                Mediator.Send(new CreateStatusMessageCommand(sessionId, ResourceTexts.ProcessFinished));
            }
            return Ok(ResourceTexts.FileReceivedToProcess);
        }

        [HttpGet]
        [Route("status-info/{sessionId}")]
        public IActionResult StatusInfo([Required] string sessionId, DateTime? statusAfter)
        {
            return Ok(Mediator.Send(new GetStatusMessageBySessionQuery(statusAfter, sessionId)));
        }

        #region Private Methods
        private static bool IsValidXmlFile(IFormFile file)
        {
            return file.ContentType == "text/xml" || file.ContentType == "application/xml";
        }
        #endregion
    }
}
