using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Systel.Shared.ResponseEntity
{
    public class ResponseEntity<T>
    {
        public T? Data { get; set; }
        public int StatusCode { get; set; }
        public string StatusDescription { get; set; } = string.Empty;

        // Factory methods for cleaner code in your ServiceImpl
        public static ResponseEntity<T> Success(T data, string message = "Request processed successfully.")
        {
            return new ResponseEntity<T>
            {
                Data = data,
                StatusCode = 200,
                StatusDescription = message
            };
        }

        public static ResponseEntity<T> Failure(string message, int code = 400)
        {
            return new ResponseEntity<T>
            {
                Data = default,
                StatusCode = code,
                StatusDescription = message
            };
        }
    }
}
