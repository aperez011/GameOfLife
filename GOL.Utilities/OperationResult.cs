using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOL.Utilities
{
    public class OperationResult
    {
        public int statusCode = 200;
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public static OperationResult Success()
        {
            return new OperationResult
            {
                IsSuccess = true
            };
        }

        public static OperationResult Fail()
        {
            return new OperationResult
            {
                IsSuccess = false
            };
        }

        public static OperationResult Fail(string errorMessage)
        {
            return new OperationResult
            {
                IsSuccess = false,
                Message = errorMessage
            };
        }

        public static OperationResult Fail(int _statusCode, string errorMessage)
        {
            return new OperationResult
            {
                statusCode = _statusCode,
                IsSuccess = false,
                Message = errorMessage
            };
        }

    }

    public class OperationResult<T> : OperationResult
    {

        public T Data { get; set; }

        public static OperationResult<T> Success()
        {
            return new OperationResult<T>
            {
                IsSuccess = true
            };
        }

        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T>
            {
                IsSuccess = true,
                Data = data
            };
        }

        public static OperationResult<T> Fail()
        {
            return new OperationResult<T>
            {
                IsSuccess = false
            };
        }

        public static OperationResult<T> Fail(string errorMessage)
        {
            return new OperationResult<T>
            {
                IsSuccess = false,
                Message = errorMessage
            };
        }

        public static OperationResult<T> Fail(int _statusCode, string errorMessage)
        {
            return new OperationResult<T>
            {
                statusCode = _statusCode,
                IsSuccess = false,
                Message = errorMessage
            };
        }
    }
}
