namespace ATM.Core.Results.Messages
{
    /// <summary>
    /// Extension methods for the message class.
    /// </summary>
    public static class MessageExtensions
    {
        /// <summary>
        /// Sets the message.
        /// </summary>
        /// <param name="message">Message to set.</param>
        /// <param name="text">Text.</param>
        public static Message WithText(this Message message, string text)
        {
            message.Text = text;
            return message;
        }

        /// <summary>
        /// Sets the message.
        /// </summary>
        /// <param name="message">Message to set.</param>
        /// <param name="key">Code of the message.</param>
        /// <param name="text">Text.</param>
        public static Message WithText(this Message message, string key, string text)
        {
            message.Code = key;
            message.Text = text;
            return message;
        }
    }
}
