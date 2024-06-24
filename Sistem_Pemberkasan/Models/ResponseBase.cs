namespace Sistem_Pemberkasan.Models
{
    public class ResponseBase
    {
        public StatusEnum Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public object Tag { get; set; } = new();
    }

    public enum StatusEnum
    {
        Error,
        Success

    }
}
