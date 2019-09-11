using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetCommon
{
    public class Result
    {
        public bool IsSuccess { get; }
        //Message is used for both (succes and failure)
        public string Message { get; }
        public bool IsFailure => !IsSuccess;

        //Only static public contructors 
        protected Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
        public static Result Ok(string message = "")
        {
            return new Result(true, message);
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Ok<T>(T value, string message = "")
        {
            return new Result<T>(value, true, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.IsFailure)
                    return result; //return first failured result (if any)
            }

            return Ok();
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _value;
            }
        }

        protected internal Result(T value, bool isSuccess, string message)
                : base(isSuccess, message)
        {
            _value = value;
        }
    }
}
