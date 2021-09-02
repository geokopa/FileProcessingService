using FileProcessingService.ConsoleApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FileProcessingService.ConsoleApp
{
    class Program
    {
        readonly static FileProcessingApiClient _apiClient = new();
        private static readonly string _sessionId = Guid.NewGuid().ToString();
        private const string BaseURL = "https://localhost:5001/api";
        private static ExtendedConsole console = new();

        static void Main(string[] args)
        {
            DisplayWelcomeScreen();

            Console.ReadKey();
        }

        private static string DisplayWelcomeScreen()
        {
            string path = "";

            console.AddOption(new Option("Specify File Path", () =>
            {
                console.ClearMainMenu();
                Console.Clear();
                Console.Write("Please paste file path you want to send: ");

                string filePath = Console.ReadLine();

                if (!string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine("Searching in directory: " + filePath);

                    if (IsDirectory(filePath))
                    {
                        if (Directory.Exists(filePath))
                        {
                            // TODO: get only xml files
                            Directory.GetFiles(filePath).Where(x => x.EndsWith("xml")).ToList().ForEach(s =>
                            {
                                console.AddOption(new Option(s, async () =>
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
                    else
                    {
                        UploadFile(filePath).Wait();
                    }
                }
            }));
            console.AddExit();
            console.Init();

            return path;
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
                    string retryAfter = DateTime.Now.ToString();
                    (string data, HttpStatusCode statusCode) response = await _apiClient.Upload($"{BaseURL}/files", filePath, _sessionId, elements);

                    if (response.statusCode == HttpStatusCode.OK)
                    {
                        (string data, HttpStatusCode statusCode) result = await _apiClient.GetDataWithPollingAsync($"{BaseURL}/files/status-info/{_sessionId}", retryAfter);

                        if (result.statusCode == HttpStatusCode.OK)
                        {
                            var messageModel = JsonConvert.DeserializeObject<IEnumerable<StatusReponseModel>>(result.data);

                            foreach (var item in messageModel)
                            {
                                if (item.Completed)
                                {
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }

                                Console.WriteLine($"{item.Message} - {item.CreatedAt} - [{item.Completed}]");
                            }
                            Console.ResetColor();

                            if (Console.ReadKey(false).Key == ConsoleKey.Enter)
                                Environment.Exit(0);
                        }
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

        static bool IsDirectory(string path)
        {
            FileAttributes attr = File.GetAttributes(path);

            return attr.HasFlag(FileAttributes.Directory);
        }
    }
}
