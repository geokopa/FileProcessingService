using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileProcessingService.Application.Common.Interfaces.Processors
{
    public interface IXmlDocumentProcessor : IDocumentProcessor
    {
        Dictionary<string, int> MatchingElements { get; set; }
        /// <summary>
        /// Opens specified stream, parse specified stream and persist into database.
        /// It function like an orchestrator
        /// </summary>
        /// <param name="stream"></param>
        /// <returns>System.Threading.Task</returns>
        Task Process(Stream stream, string[] elements, string sessionId);
        string GetMatchingElementSummery();
    }
}
