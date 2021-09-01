using FileProcessingService.API.BackgroundServices;
using FileProcessingService.Application.Common.Interfaces.Processors;
using FileProcessingService.Application.StatusMessages.Queries;
using FileProcessingService.Infrastructure.Extensions;
using FileProcessingService.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileProcessingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly ISender _sender;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IXmlDocumentProcessor _documentProcessor;

        public FilesController(IBackgroundTaskQueue backgroundTaskQueue, IXmlDocumentProcessor documentProcessor, ISender sender)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _documentProcessor = documentProcessor;
            _sender = sender;
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
            if (files.Length == 0 || files.Any(x => !IsValidXmlFile(x)))
                return BadRequest();

            foreach (var file in files)
            {
                _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
                {
                    var fileBytes = await AsMemoryByteArray(file);
                    
                    await _documentProcessor.Process(fileBytes, elements.AsCleanedArray(), sessionId, token);
                });
            }
            return Ok(ResourceTexts.FileReceivedToProcess);
        }

        [HttpGet]
        [Route("status-info/{sessionId}")]
        public async Task<IActionResult> StatusInfo([Required] string sessionId, DateTime? statusAfter)
        {
            return Ok(await _sender.Send(new GetStatusMessageBySessionQuery(statusAfter, sessionId)));
        }

        #region Private Methods
        private static bool IsValidXmlFile(IFormFile file)
        {
            return file.ContentType == "text/xml" || file.ContentType == "application/xml";
        }

        private async Task<byte[]> AsMemoryByteArray(IFormFile file)
        {
            var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            return stream.ToArray();
        }
        #endregion
    }
}
