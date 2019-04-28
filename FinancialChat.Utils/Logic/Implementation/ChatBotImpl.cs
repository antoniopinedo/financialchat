using FileHelpers;
using FinancialChat.Models;
using System;
using System.Net;

namespace ChatBot.Logic.Implementation
{
    class ChatBotImpl : IChatBot
    {
        //public void 

        public string GetQuoteForStock(string stockSymbol)
        {
            //TODO: Move this to the bot logic
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

        public void StartBot()
        {
            throw new NotImplementedException();
        }
    }
}
