using System.Net;

namespace MVC.Models
{
    public class ApiResponseResult<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
