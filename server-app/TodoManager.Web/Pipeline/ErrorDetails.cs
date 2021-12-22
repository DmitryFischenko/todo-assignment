namespace TodoManager.Web.Pipeline
{
    public class ErrorDetails
    {
        public string Message { get; set; }
    }
    
    public class BusinessExceptionErrorDetails
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }
}