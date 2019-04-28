using System.Text.RegularExpressions;

namespace FinancialChat.Helpers
{
    /// <summary>
    /// Command Parser class
    /// </summary>
    public class CommandParser
    {
        private const string regEx = @"^(\/stock=)([a-zA-Z0-9:\.^_]+)$";

        /// <summary>
        /// Parses a command string to extract the stock value
        /// </summary>
        /// <param name="text">The text to parse</param>
        /// <returns>The stock name</returns>
        public static string GetStockFromCommand(string text)
        {
            var result = "";

            Match match = Regex.Match(text, regEx, RegexOptions.Singleline);

            if (match.Success)
            {
                result = match.Groups[2].Value;
            }

            return result;
        }

        /// <summary>
        /// Check whether the passed string is a command or not based on a regular expression
        /// </summary>
        /// <param name="text">The text to check</param>
        /// <returns>A boolean value indicating if it is command</returns>
        public static bool IsCommand(string text)
        {
            return Regex.IsMatch(text, regEx, RegexOptions.Singleline);
        }
    }

    
}