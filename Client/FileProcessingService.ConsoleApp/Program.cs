using RestSharp;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace FileProcessingService.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "";
            ExtendedConsole console = new();
            console.AddOption(new Option("Specify File Path", () =>
            {
                console.ClearMainMenu();
                Console.Clear();
                Console.Write("Please paste file path you want to send: ");

                string filePath = Console.ReadLine();

                if (!string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("Searching in directory: " + filePath);
                    if (Directory.Exists(filePath))
                    {
                        Directory.GetFiles(filePath).ToList().ForEach(s =>
                        {
                            console.AddOption(new Option(s, async ()  =>
                            {
                                path = s;
                                await UploadFile(path);
                            }));
                        });
                        console.AddExit();
                        console.WriteMenu(console.Options, console.Options[0]);
                    }
                    else
                        Console.WriteLine("path not found");
                }
            }));
            console.AddMainMenu();
            console.Init();

            Console.ReadKey();
        }


        static async Task UploadFile(string filePath)
        {
            Console.Clear();
            Console.WriteLine($"Choosed file path: {filePath}");

            Console.Write("Please enter xml element you want to find in file. Sepetarete with semicolon (;): ");
            string elements = Console.ReadLine();

            if (!string.IsNullOrEmpty(elements))
            {
                Console.WriteLine("Uploading file started..");
                if (!File.Exists(filePath))
                {
                    return;
                }
                try
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

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine("Success");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("XML element is required!");
                Console.ResetColor();
            }
        }
    }
}
