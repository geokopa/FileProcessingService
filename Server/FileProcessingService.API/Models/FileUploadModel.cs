using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FileProcessingService.API.Models
{
    public class FileUploadModel
    {
        [Required]
        public string SessionId { get; set; }
        [Required]
        public string Elements { get; set; }
        
    }
}
