using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }

        public ResponseDTO(bool success = true, string? message = null)
        {
            IsSuccess = success;
            Message = message;
        }

        public static ResponseDTO Ok(string? message = null)
            => new ResponseDTO(true, message);

        public static ResponseDTO Fail(string message)
            => new ResponseDTO(false, message);
    }

    public class ResponseDTO<T> : ResponseDTO
    {
        public ResponseDTO(T? data, bool success = true, string? message = null)
        : base(success, message)
        {
            Data = data;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T? Data { get; set; }

        public static ResponseDTO<T> Ok(T data, string? message = null)
        => new ResponseDTO<T>(data, true, message);

        public static ResponseDTO<T> Fail(string message)
            => new ResponseDTO<T>(default, false, message);
    }
}
