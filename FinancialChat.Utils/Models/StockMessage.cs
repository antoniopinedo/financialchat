using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.Utils.Models
{
    /// <summary>
    /// A stock message class
    /// </summary>
    public class StockMessage
    {
        /// <summary>
        /// The message text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The user name of the sender
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The user id of the sender
        /// </summary>
        public string UserId { get; set; }
    }
}
