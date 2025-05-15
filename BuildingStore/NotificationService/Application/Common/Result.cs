namespace NotificationService.Application.Common
{
    /// <summary>
    /// Result pattern.
    /// </summary>
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        protected Result(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Success(string message)
        {
            return new Result(true, message);
        }

        public static Result Failure(string message)
        {
            return new Result(false, message);
        }
    }

    public class Result<T> : Result
    {
        public T Data { get; private set; }

        private Result(bool isSuccess, string message, T data)
            : base(isSuccess, message)
        {
            Data = data;
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T>(true, message, data);
        }

        public static new Result<T> Failure(string message)
        {
            return new Result<T>(false, message, default);
        }
    }
}
