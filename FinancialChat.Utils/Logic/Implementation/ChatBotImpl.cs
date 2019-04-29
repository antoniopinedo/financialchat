using FileHelpers;
using FinancialChat.Utils.Models;
using FinancialChat.Utils.Helpers;
using System;
using System.Net;
using Newtonsoft.Json;

namespace ChatBot.Logic.Implementation
{
    /// <summary>
    /// Implementation class for the chatbot interface
    /// </summary>
    public class ChatBotImpl : IChatBot
    {
        private readonly MessageBrokerFacade MQClient = new MessageBrokerFacade();

        /// <summary>
        /// Gets a quote for the stock symbol provided
        /// </summary>
        /// <param name="stockSymbol"></param>
        /// <returns></returns>
        public string GetQuoteForStock(string stockSymbol)
        {
            var engine = new FileHelperEngine<StockQuote>();
            WebClient client = new WebClient();
            var csvContent = client.DownloadString("https://stooq.com/q/l/?s=" + stockSymbol + "&f=sd2t2ohlcv&h&e=csv");
            var record = engine.ReadString(csvContent)[0];
            var quoteMessage = "";
            if (record.Close.Equals("N/D"))
            {
                quoteMessage = stockSymbol + " is not a valid Stock Code.";
            }
            else
            {
                quoteMessage = stockSymbol + " quote is " + record.Close + "$ per share";
            }

            return quoteMessage;
        }

        public void ProcessMessage(string message)
        {
            StockMessage messageObject = JsonConvert.DeserializeObject<StockMessage>(message);

            var quote = GetQuoteForStock(messageObject.Text);

            messageObject.Text = quote;

            MQClient.Send("stockResponse", JsonConvert.SerializeObject(messageObject));
        }

        public void StartBot()
        {
            MQClient.Receive("stockRequest", ProcessMessage);
        }
    }
}
