using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Response.Common
{
    public class APIResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public APIResponseDTO(bool success = true, string? message = null)
        {
            IsSuccess = success;
            Message = message;
        }

        public static APIResponseDTO Ok(string? message = null)
            => new APIResponseDTO(true, message);

        public static APIResponseDTO Fail(string message)
            => new APIResponseDTO(false, message);
    }

    public class APIResponseDTO<T> : APIResponseDTO
    {
        public APIResponseDTO(T? data, bool success = true, string? message = null)
        : base(success, message)
        {
            Data = data;
        }
        public T? Data { get; set; }

        public static APIResponseDTO<T> Ok(T data, string? message = null)
        => new APIResponseDTO<T>(data, true, message);

        public static APIResponseDTO<T> Fail(string message)
            => new APIResponseDTO<T>(default, false, message);
    }
}
