using ATM.Core.Results.Messages;

namespace ATM.Core.Results
{
    public static class ResultExtensions
    {
        #region "Messages"

        /// <summary>
        /// Adds an message to a result.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="result">Parent result.</param>
        /// <param name="message">Message to add.</param>
        public static TResult WithError<TResult>(this TResult result, Message message)
            where TResult : BaseResult
        {
            result.Messages.Add(message);
            return result;
        }

        /// <summary>
        /// Adds an message to a result.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="result">Parent result.</param>
        /// <param name="message">Text of the message to add.</param>
        public static TResult WithError<TResult>(this TResult result, string message)
            where TResult : BaseResult
        {
            result.Messages.Add(new Message(message));
            return result;
        }

        /// <summary>
        /// Adds an message to a result.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <param name="result">Parent result.</param>
        /// <param name="key">Code of the message to add.</param>
        /// <param name="message">Text of the message to add.</param>
        public static TResult WithError<TResult>(this TResult result, string key, string message)
            where TResult : BaseResult
        {
            result.Messages.Add(new Message(key, message));
            return result;
        }

        /// <summary>
        /// Adds an message to a result.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <typeparam name="TError">Message type.</typeparam>
        /// <param name="result">Parent result.</param>
        public static TResult WithError<TResult, TError>(this TResult result)
            where TResult : BaseResult
            where TError : Message, new()
        {
            result.Messages.Add(new TError());
            return result;
        }

        /// <summary>
        /// Adds an message to a result.
        /// </summary>
        /// <typeparam name="TResult">Result type.</typeparam>
        /// <typeparam name="TError">Message type.</typeparam>
        /// <param name="result">Parent result.</param>
        /// <param name="error">Message to add.</param>
        public static TResult WithError<TResult, TError>(this TResult result, TError error)
            where TResult : BaseResult
            where TError : Message
        {
            result.Messages.Add(error);
            return result;
        }

        #endregion
    }
}