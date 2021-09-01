using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProcessingService.API.Models
{
    public class StatusMessage
    {
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }

        public StatusMessage(string message, DateTime timeStamp)
        {
            Message = message;
            TimeStamp = timeStamp;
        }
    }
}
