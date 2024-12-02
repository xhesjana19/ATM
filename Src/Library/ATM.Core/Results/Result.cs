using ATM.Core.Results.Messages;

namespace ATM.Core.Results
{
    public class Result : BaseResult
    {
        #region "Static constructors"

        public static Result Ok()
        {
            return new Result();
        }

        public static Result<TValue> Ok<TValue>(TValue value)
        {
            return new Result<TValue>(value);
        }

        public static Result Fail(Message message)
        {
            return new Result()
                .WithError(message);
        }

        public static Result Fail(string message)
        {
            return new Result()
                .WithError(message);
        }

        public static Result Fail(string key, string message)
        {
            return new Result()
                .WithError(key, message);
        }

        public static Result<TValue> Fail<TValue>(Message message)
        {
            return new Result<TValue>()
                .WithError(message);
        }

        public static Result<TValue> Fail<TValue>(string message)
        {
            return new Result<TValue>()
                .WithError(message);
        }

        public static Result<TValue> Fail<TValue>(string key, string message)
        {
            return new Result<TValue>()
                .WithError(key, message);
        }

        public static Result<TValue> Fail<TValue>(TValue value, string key, string message)
        {
            var result = new Result<TValue>();
            result.WithError(key, message);
            result.Value = value;
            return result;
        }

        #endregion
    }

    public class Result<TValue> : BaseValueResult<TValue>
    {
        public Result()
        {
        }
        /// <summary>
        /// test x001 + x002 + x003 + x004
        /// </summary>
        /// <param name="value"></param>
        public Result(TValue value)
        {
            Value = value;
        }
    }
}
