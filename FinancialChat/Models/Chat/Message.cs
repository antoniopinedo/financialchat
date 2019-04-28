namespace FinancialChat.Models.Chat
{
    /// <summary>
    /// Representation of a message in the chat system
    /// </summary>
    public class Message
    {     
        /// <summary>
        /// Message owner or sender
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// The message text content
        /// </summary>
        public string TextMessage { get; set; }

        /// <summary>
        /// String representation of the time when the message is submitted
        /// </summary>
        public string Time { get; set; }
    }
}