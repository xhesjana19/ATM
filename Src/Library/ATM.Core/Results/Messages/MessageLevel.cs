namespace ATM.Core.Results.Messages
{
    /// <summary>
    /// Enum describing the severity of the message.
    /// </summary>
    public enum MessageLevel
    {
        /// <summary>
        /// Message is an error indicating that the request could not be completed.
        /// Requires immediate attention.
        /// </summary>
        Error = 0,
        /// <summary>
        /// Message is a warning and requires attention.
        /// </summary>
        Warn = 1,
        /// <summary>
        /// Message is just an information for the user.
        /// Can be safely ignored.
        /// </summary>
        Info = 2
    }
}