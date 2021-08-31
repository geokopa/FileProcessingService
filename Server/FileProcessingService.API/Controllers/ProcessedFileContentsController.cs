using FileProcessingService.Application.ProcessedFileContent.Queries;
using FileProcessingService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace FileProcessingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessedFileContentsController : BaseController
    {
        [HttpGet]
        [Route("{sessionId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(ProcessedFileContent))]
        public async Task<IActionResult> Get(Guid sessionId)
        {
            var processedFile = await Mediator.Send(new GetProcessedFileContentQuery { SessionId = sessionId });

            if (processedFile == null)
                return NotFound();

            return Ok(processedFile);
        }
    }
}
