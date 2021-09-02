using RestSharp;
using System.Threading.Tasks;
using Xunit;

namespace FileProcessingService.UnitTests
{
    public class FileUploadUnitTests
    {
        [Fact]
        public async Task UploadFileSuccesfully()
        {
            var client = new RestClient("https://localhost:5001/api/files")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFile("file", @"C:\Users\George Kopadze\Downloads\Compathy_Manual_ContentGrasp SX_en.xml", "text/xml");
            request.AddParameter("sessionId", "5a6b9ec8-dc46-4a11-8068-e7bb25ec8119");
            request.AddParameter("elements", "li;p");
            IRestResponse response = await client.ExecuteAsync(request);
            var data = response.Content;

            Assert.Equal("200", response.StatusCode.ToString());
        }
    }
}