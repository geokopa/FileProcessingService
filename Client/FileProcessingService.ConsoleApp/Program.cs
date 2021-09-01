using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
                    using var form = new MultipartFormDataContent();
                    var Content = new ByteArrayContent(File.ReadAllBytes(filePath));
                    Content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                    form.Add(Content, "file", Path.GetFileName(filePath));
                    form.Add(new StringContent(Guid.NewGuid().ToString()), "sessionId");
                    form.Add(new StringContent(elements), "elements");
                    using HttpClient client = new();
                    var response = await client.PostAsync(@"https://localhost:5001/api/file", form);
                    
                    if (response.IsSuccessStatusCode)
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
