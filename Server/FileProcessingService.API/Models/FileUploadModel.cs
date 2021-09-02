using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FileProcessingService.API.Models
{
    public class FileUploadModel
    {
        /// <summary>
        /// Process Uniqueue Identificator (CorrelationID)
        /// </summary>
        [Required]
        public string SessionId { get; set; }
        /// <summary>
        /// XML elements like [p;li;a]
        /// </summary>
        [Required]
        public string Elements { get; set; }
        
    }
}
