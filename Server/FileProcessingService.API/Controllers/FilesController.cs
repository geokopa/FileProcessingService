using FileProcessingService.API.BackgroundServices;
using FileProcessingService.API.Models;
using FileProcessingService.Application.Common.Interfaces.Processors;
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
        private readonly IMemoryCache _cache;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IXmlDocumentProcessor _documentProcessor;

        public FilesController(ILogger<FilesController> logger, IMemoryCache cache, IBackgroundTaskQueue backgroundTaskQueue, IXmlDocumentProcessor documentProcessor)
        {
            _cache = cache;
            _backgroundTaskQueue = backgroundTaskQueue;
            _documentProcessor = documentProcessor;
        }

        /*
         * TODO: Configure timeouts and file upload max size
         */

        [HttpPost]
        public IActionResult Process([Required] IFormFile[] files, [Required] string sessionId, [Required] string elements)
        {
            if (files.Length == 0)
                return BadRequest();

            //SetStatusMessage(sessionId, "Process Started");

            foreach (var file in files)
            {
                var stream = file.OpenReadStream();

                _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
                {
                    await _documentProcessor.Process(stream, elements.AsCleanedArray(), sessionId);
                });
            }

            StringBuilder builder = new();
            foreach (var item in _documentProcessor.MatchingElements)
            {
                builder.AppendLine($"Element: {item.Key} has found {item.Value} times");
            }

            //SetStatusMessage(sessionId, builder.ToString());
            //SetStatusMessage(sessionId, "Process Finished");

            return Ok("File processing request has been received!");
        }



        [HttpGet]
        [Route("{sessionId}")]
        public IActionResult GetStatus(string sessionId, DateTime? statusAfter)
        {
            List<StatusMessage> messages = new();

            if (_cache.TryGetValue(sessionId, out messages))
            {
                if (statusAfter.HasValue)
                {
                    messages = messages.Where(x => x.TimeStamp > statusAfter.Value).ToList();
                }
            }

            if (!messages.Any())
                return NotFound();

            return Ok(messages);
        }



        #region Private Methods

        private void SetStatusMessage(string sessionId, string message)
        {
            List<StatusMessage> currentStatusMessages;
            if (!_cache.TryGetValue(sessionId, out currentStatusMessages))
            {
                currentStatusMessages = new List<StatusMessage>();
            }

            currentStatusMessages.Add(new StatusMessage(message, DateTime.Now));
            _cache.Set(sessionId, currentStatusMessages);
        }
        
        #endregion

    }




}
