namespace ParkingManagements.Server.Common
{
    public class ErrorResponse
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string CorrelationId { get; set; }
    }

    public class ServiceException : Exception
    {
        public string Code { get; set; }
        public int StatusCode { get; set; }

        public ServiceException(string code, string message, int statusCode ): base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }
    }
}
