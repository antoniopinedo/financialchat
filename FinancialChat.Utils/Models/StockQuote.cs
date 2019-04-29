using FileHelpers;

namespace FinancialChat.Utils.Models
{
    /// <summary>
    /// A stock quote class
    /// </summary>
    [IgnoreFirst]
    [DelimitedRecord(",")]
    public class StockQuote
    {
        /// <summary>
        /// Stock ticker symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Date of the quote
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Time of the quote
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Opening value for the stock
        /// </summary>
        public string Open { get; set; }

        /// <summary>
        /// Highest value for the stock
        /// </summary>
        public string High { get; set; }

        /// <summary>
        /// Lowest value for the stock
        /// </summary>
        public string Low { get; set; }

        /// <summary>
        /// Closing value for the stock
        /// </summary>
        public string Close { get; set; }

        /// <summary>
        /// Transactional volume for the stock
        /// </summary>
        public string Volume { get; set; }
    }
}