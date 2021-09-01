using FileProcessingService.Application.Common.Extensions;
using FileProcessingService.Application.Common.Interfaces.Processors;
using FileProcessingService.Application.ProcessedFileContent.Commands;
using FileProcessingService.Application.StatusMessages.Commands;
using FileProcessingService.Infrastructure.Extensions;
using FileProcessingService.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace FileProcessingService.Infrastructure.Processors
{
    public class XmlDocumentProcessor : IXmlDocumentProcessor
    {
        public Dictionary<string, int> MatchingElements { get; set; }

        private readonly ILogger<XmlDocumentProcessor> _logger;
        private readonly IServiceScopeFactory scopeFactory;

        public XmlDocumentProcessor(ILogger<XmlDocumentProcessor> logger, IServiceScopeFactory scopeFactory)
        {
            MatchingElements = new Dictionary<string, int>();
            _logger = logger;
            this.scopeFactory = scopeFactory;

        }

        public async Task Process(byte[] bytes, string[] elements, string sessionId, CancellationToken ct)
        {
            MemoryStream memory = new(bytes);

            await Process(memory, elements, sessionId, ct);
        }

        public async Task Process(Stream stream, string[] elements, string sessionId, CancellationToken ct)
        {
            using var scope = scopeFactory.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            await mediator.Send(new CreateStatusMessageCommand(sessionId, ResourceTexts.ProcessStarted), ct);

            if (ct.IsCancellationRequested)
            {
                await mediator.Send(new CreateStatusMessageCommand(sessionId, ResourceTexts.Canceled), ct);
                ct.ThrowIfCancellationRequested();
            }

            XmlReaderSettings settings = new()
            {
                DtdProcessing = DtdProcessing.Parse,
                Async = true
            };

            using XmlReader reader = XmlReader.Create(stream, settings);
            while (await reader.ReadAsync())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (!string.IsNullOrWhiteSpace(reader.Name) && elements.Contains(reader.Name))
                    {
                        string key = reader.Name.ToLowerInvariant();

                        if (MatchingElements.ContainsKey(key))
                        {
                            MatchingElements[key] = MatchingElements[key] + 1;
                        }
                        else
                        {
                            if (!MatchingElements.TryAdd(key, 1))
                            {
                                _logger.LogError("Error during add {0} in memory", key);
                            }
                        }

                        var innerText = await reader.ReadElementContentAsStringAsync();
                        string cleanedInnerText = innerText.Replace("\n", " ");
                        var duplicateStatistics = cleanedInnerText.FindDuplicates().ToDuplicateWordModel();

                        if (!string.IsNullOrEmpty(innerText))
                        {
                            await mediator.Send(new CreateProcessedFileContentCommand(sessionId, innerText, key, duplicateStatistics), ct);
                        }
                        await mediator.Send(new CreateStatusMessageCommand(sessionId, $"{key} found {MatchingElements[key]} times"), ct);
                    }
                }
            }

            await mediator.Send(new CreateStatusMessageCommand(sessionId, GetMatchingElementSummery()), ct);
            await mediator.Send(new CreateStatusMessageCommand(sessionId, ResourceTexts.ProcessFinished), ct);
        }

        public string GetMatchingElementSummery()
        {
            StringBuilder builder = new();
            foreach (var item in MatchingElements)
            {
                builder.AppendLine($"Element: {item.Key} has found {item.Value} times");
            }
            return builder.ToString();
        }
    }
}