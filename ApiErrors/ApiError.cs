namespace MedicineStorage.ApiErrors
{
    public class ApiError
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string? Details { get; set; }
        public ApiError(int statusCode, string message, string? details)
        {
            StatusCode = statusCode;
            Message = message;
            Details = details;
        }
    }
}
