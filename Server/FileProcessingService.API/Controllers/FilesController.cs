using FileProcessingService.API.BackgroundServices;
using FileProcessingService.API.Models;
using FileProcessingService.Application.Common.Interfaces.Processors;
using FileProcessingService.Application.ProcessedFileContent.Queries;
using FileProcessingService.Application.StatusMessages.Queries;
using FileProcessingService.Domain.Entities;
using FileProcessingService.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly IConfiguration _configuration;

        public FilesController(IBackgroundTaskQueue backgroundTaskQueue, IXmlDocumentProcessor documentProcessor, ISender sender, IConfiguration configuration)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _documentProcessor = documentProcessor;
            _sender = sender;
            _configuration = configuration;
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Process(IFormFile file, [FromForm] FileUploadModel model)
        {
            if (file == null || !IsValidXmlFile(file))
                return BadRequest("Please specify files.");

            var fileBytes = await AsMemoryByteArray(file);
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                await _documentProcessor.Process(fileBytes, model.Elements.AsCleanedArray(), model.SessionId, token);
            });

            return Ok();
        }

        [HttpGet]
        [Route("status-info/{sessionId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<StatusMessage>))]
        public async Task<IActionResult> StatusInfo([Required] string sessionId)
        {
            string statusAfter = string.Empty;

            if (Request.Headers.TryGetValue("statusAfter", out var dateValue))
            {
                statusAfter = dateValue;
            }

            var returnData = await _sender.Send(new GetStatusMessageBySessionQuery(statusAfter, sessionId));

            bool completed = returnData.Any(x => x.Completed);
            SetRetryHeader(completed);

            if (!returnData.Any())
                return NotFound();

            return Ok(returnData);
        }

        [HttpGet]
        [Route("processed/{sessionId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IEnumerable<ProcessedFileContent>))]
        public async Task<IActionResult> Processed(string sessionId)
        {
            var processedFile = await _sender.Send(new GetProcessedFileContentQuery { SessionId = sessionId });

            if (!processedFile.Any())
                return NotFound();

            return Ok(processedFile);
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

        private void SetRetryHeader(bool completed)
        {
            Response.Headers.Add("Retry-After", _configuration.GetValue<int>("Headers:RetryAfter").ToString());
            Response.Headers.Add("Completed", completed.ToString());
        }
        #endregion
    }
}
