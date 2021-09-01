namespace FileProcessingService.Domain.Entities
{
    public class StatusMessage : BaseEntity
    {
        public string SessionId { get; set; }
        public string Message { get; set; }
    }
}
