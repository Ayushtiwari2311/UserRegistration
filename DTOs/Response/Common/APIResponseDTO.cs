using DataTransferObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Response.Common
{
    public class APIResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public APIResponseDTO() { }

        public APIResponseDTO(bool success = true, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            IsSuccess = success;
            Message = message;
            StatusCode = statusCode;
        }

        public static APIResponseDTO Ok(string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new APIResponseDTO(true, message, statusCode);

        public static APIResponseDTO Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new APIResponseDTO(false, message, statusCode);
    }

    public class APIResponseDTO<T> : APIResponseDTO
    {
        public T? Data { get; set; }

        public APIResponseDTO() { }

        public APIResponseDTO(T? data, bool success = true, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
            : base(success, message, statusCode)
        {
            Data = data;
        }

        public static APIResponseDTO<T> Ok(T data, string? message = null, HttpStatusCode statusCode = HttpStatusCode.OK)
            => new APIResponseDTO<T>(data, true, message, statusCode);

        public static APIResponseDTO<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            => new APIResponseDTO<T>(default, false, message, statusCode);
    }

}
