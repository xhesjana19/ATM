using ATM.Core.Results.Messages;
using System.Collections.Generic;
using System.Linq;

namespace ATM.Core.Results
{
    /// <summary>
    /// Base class for results.
    /// </summary>
    public abstract class BaseResult
    {
        private IList<Message>? _messages;
        
        /// <summary>
        /// Determines whether the result is successful.
        /// </summary>
        public bool Success => Messages.All(m => m.Level != MessageLevel.Error);

        public string Description { get; set; } = "";

        /// <summary>
        /// List of messages on this result.
        /// </summary>
        public IList<Message> Messages
        {
            get => _messages ??= new List<Message>();
            set => _messages = value;
        }

        /// <summary>
        /// Merges multiple results into one.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="results">Results to merge.</param>
        public static TResult Merge<TResult>(params BaseResult[] results)
            where TResult : BaseResult, new()
        {
            var final = new TResult();

            foreach (var result in results)
            {
                foreach (var error in result.Messages)
                {
                    final.WithError(error);
                }
            }

            return final;
        }

        /// <summary>
        /// Merges multiple results into one.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="results">Results to merge.</param>
        public static Result<TResult> To<TResult>(params BaseResult[] results)
            where TResult : class, new()
        {
            var final = new Result<TResult>();

            foreach (var result in results)
            {
                foreach (var error in result.Messages)
                {
                    final.WithError(error);
                }
            }
            return final;
        }
    }
}