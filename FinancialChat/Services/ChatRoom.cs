using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using FileHelpers;
using FinancialChat.Helpers;
using FinancialChat.Models.Chat;
using Microsoft.AspNet.SignalR;

namespace FinancialChat.Services
{
    /// <summary>
    /// ChatRoom class definition
    /// </summary>
    public class ChatRoom : Hub
    {
        #region Public Static Attributes

        /// <summary>
        /// A List of connected users
        /// </summary>
        static List<ChatUser> ConnectedUsers = new List<ChatUser>();

        /// <summary>
        /// The messages cache
        /// </summary>
        static List<Message> MessagesCache = new List<Message>();

        #endregion

        #region Server Methods

        /// <summary>
        /// Initial method called to stablish a connection to the hub
        /// </summary>
        /// <param name="userName">The user name to be added to the connected users</param>
        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            if (!ConnectedUsers.Any(x => x.ConnectionId == id))
            {
                // Creates the new chat user object
                ConnectedUsers.Add(new ChatUser { ConnectionId = id, UserName = userName });

                // Refresh the screen for the caller
                Clients.Caller.refreshScreen(userName, ConnectedUsers, MessagesCache);

                // Notify all other clients about new user
                Clients.AllExcept(id).notifyNewUserConnected(id, userName);
            }
        }

        /// <summary>
        /// Receives a message to be sent to all the audience or a command for the bot
        /// </summary>
        /// <param name="userName">The user initiating the action</param>
        /// <param name="message">The message to be delivered or the command</param>
        /// <param name="time">String representation of the time when the message was submitted</param>
        public void ProcessInput(string userName, string message, string time)
        {

            if (CommandParser.IsCommand(message))
            {
                // This is a command for the bot
                var stockValue = CommandParser.GetStockFromCommand(message);

                // Write the request in the outgoing message queue

                // Triggers the bot process in background

                //TODO: Move this to the bot logic
                var engine = new FileHelperEngine<StockQuote>();
                WebClient client = new WebClient();
                var csvContent = client.DownloadString("https://stooq.com/q/l/?s=" + stockValue + "&f=sd2t2ohlcv&h&e=csv");
                var record = engine.ReadString(csvContent)[0];
                var quoteMessage = "";
                if (record.Close.Equals("N/D"))
                {
                    quoteMessage = record.Symbol + " is not a valid Stock Code.";
                }
                else
                {
                    quoteMessage = record.Symbol + " quote is " + record.Close + "$ per share";
                }

                Clients.All.printMessage("ChatBot", quoteMessage, time);
            }
            else
            {
                // This is a message for the audience

                // Add new message to the cache
                MessagesCache.Add(new Message { Owner = userName, TextMessage = message, Time = time });

                // Controls the messages cache not growing over 50 elements
                if (MessagesCache.Count > 50)
                {
                    MessagesCache.RemoveAt(0);
                }

                // Persist message to DB?

                // Broad cast message
                Clients.All.printMessage(userName, message, time);
            }            
        }

        #endregion

        #region Hub Methods

        /// <summary>
        /// Actions to execute when the current session is disconnected
        /// </summary>
        /// <param name="stopCalled"></param>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == id);
            if (item != null)
            {
                ConnectedUsers.Remove(item);
                Clients.All.notifyUserDisconnected(id);
            }
            return base.OnDisconnected(stopCalled);
        }

        #endregion

        [IgnoreFirst]
        [DelimitedRecord(",")]
        public class StockQuote
        {
            public string Symbol { get; set; }

            public string Date { get; set; }

            public string Time { get; set; }

            public string Open { get; set; }

            public string High { get; set; }

            public string Low { get; set; }

            public string Close { get; set; }

            public string Volume { get; set; }

        }
    }
}