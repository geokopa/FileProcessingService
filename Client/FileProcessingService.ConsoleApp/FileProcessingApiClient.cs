using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace FileProcessingService.ConsoleApp
{
    public class FileProcessingApiClient
    {
        private static readonly HttpClient client = new();

        public static async Task<(string response, HttpStatusCode status)> Upload(string url, string path, string sessionId, string elements)
        {
            var client = new RestClient(url)
            {
                Timeout = -1
            };

            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFile("file", path, "text/xml");
            request.AddParameter("sessionId", sessionId);
            request.AddParameter("elements", elements);
            IRestResponse response = await client.ExecuteAsync(request);
            var data = response.Content;

            return (response: data, status: response.StatusCode);
        }

        public static async Task<(string response, HttpStatusCode statusCode)> GetDataWithPollingAsync(string url, string retryAfter)
        {
            bool completed = false;
            int requestCount = 0;

            do
            {
                using HttpRequestMessage request = new(HttpMethod.Get, url);

                if (requestCount > 0)
                {
                    request.Headers.Add("statusAfter", retryAfter);
                }

                TimeSpan delay = default;
                using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
                {
                    if (response.Headers.TryGetValues("Completed", out var values))
                    {
                        var completedHeader = values.FirstOrDefault();

                        if (completedHeader != null)
                        {
                            if (bool.TryParse(completedHeader, out completed))
                            {
                                if (!response.IsSuccessStatusCode || !completed)
                                {
                                    delay = response.Headers.RetryAfter.Delta ?? TimeSpan.FromSeconds(1);
                                    requestCount++;
                                    continue;
                                }
                                else
                                {
                                    requestCount++;
                                    return (await response.Content.ReadAsStringAsync().ConfigureAwait(false), response.StatusCode);
                                }
                            }
                        }
                    }
                }
                await Task.Delay(delay);
            
            } while (!completed);
            throw new Exception("Failed to get data from server");
        }
    }
}
