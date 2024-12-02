namespace ATM.Core.Results.Messages
{
    /// <summary>
    /// Result message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Creates a new message (with severity level of error).
        /// </summary>
        /// <param name="text">Text of the message.</param>
        public Message(string text)
        {
            Code = string.Empty;
            Text = text;
            Level = MessageLevel.Error;
        }

        /// <summary>
        /// Creates a new message (with severity level of error).
        /// </summary>
        /// <param name="code">Code of the message.</param>
        /// <param name="text">Text of the message.</param>
        public Message(string code, string text)
        {
            Code = code;
            Text = text;
            Level = MessageLevel.Error;
        }

        /// <summary>
        /// Creates a new message.
        /// </summary>
        /// <param name="code">Code of the message.</param>
        /// <param name="text">Text of the message.</param>
        /// <param name="level">Severity of the message.</param>
        public Message(string code, string text, MessageLevel level)
        {
            Code = code;
            Text = text;
            Level = level;
        }

        /// <summary>
        /// Optional code to identify who threw this message.
        /// For example, a property name that is not validating successfully.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Text describing why this message occurred.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the severity level of the message.
        /// </summary>
        public MessageLevel Level { get; set; }

        public override string ToString()
        {
            return $"[({Level}): {Code}] - {Text}]";
        }
    }
}