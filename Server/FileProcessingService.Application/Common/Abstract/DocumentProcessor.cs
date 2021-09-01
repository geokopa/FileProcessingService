using FileProcessingService.Application.Common.Interfaces.Processors;
using System.IO;
using System.Threading.Tasks;

namespace FileProcessingService.Application.Common.Abstract
{
    public abstract class DocumentProcessor : IDocumentProcessor
    {
        public abstract Task Process(Stream stream, string[] elements);
    }
}
