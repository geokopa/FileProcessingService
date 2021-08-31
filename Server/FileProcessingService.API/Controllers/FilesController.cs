using FileProcessingService.Application.ProcessedFileContent.Queries;
using FileProcessingService.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;

namespace FileProcessingService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : BaseController
    {
        private readonly ConcurrentDictionary<Guid, string> _inMemoryCache;

        public FilesController()
        {
            _inMemoryCache = new ConcurrentDictionary<Guid, string>();
        }

        [HttpGet]
        [Route("{sessionId}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(string))]
        public IActionResult Get(Guid sessionId)
        {
            if (!_inMemoryCache.ContainsKey(sessionId))
                return NotFound();

            if (_inMemoryCache.TryGetValue(sessionId, out string status))
            {
                return Ok(status);
            }

            return BadRequest();
        }

    }
}
