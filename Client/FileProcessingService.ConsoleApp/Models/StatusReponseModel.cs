using System;

namespace FileProcessingService.ConsoleApp.Models
{
    public class StatusReponseModel
    {
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Completed { get; set; }
    }
}
