using System.Threading.Tasks;

namespace FileProcessingService.Application.Common.Interfaces.Processors
{
    public interface IDocumentParser
    {
        Task Parse();
    }
}
