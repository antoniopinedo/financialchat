using System.Collections.Generic;
using System.Linq;
using FinancialChat.Utils.Helpers;
using FinancialChat.Models.Chat;
using Microsoft.AspNet.SignalR;
using System;
using FinancialChat.Utils.Models;
using Newtonsoft.Json;

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
        static readonly List<ChatUser> ConnectedUsers = new List<ChatUser>();

        /// <summary>
        /// The messages cache
        /// </summary>
        static readonly List<Message> MessagesCache = new List<Message>();

        /// <summary>
        /// The message broker client
        /// </summary>
        static readonly MessageBrokerFacade MQClient = new MessageBrokerFacade();

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
        public void ProcessInput(string userName, string message)
        {
            var time = DateTime.Now.ToString("yyyy-MM-dd, hh:mm:ss");
            var userId = Context.ConnectionId;

            if (CommandParser.IsCommand(message))
            {
                // This is a command for the bot
                var stockValue = CommandParser.GetStockFromCommand(message);

                // Build the message for the queue
                StockMessage messageObject = new StockMessage
                {
                    Text = stockValue,
                    UserId = userId,
                    UserName = userName
                };

                // Write the request in the outgoing message queue
                MQClient.Send("stockRequest", JsonConvert.SerializeObject(messageObject));

                var onlineClient = Clients.Client(userId);
                onlineClient.printMessage(userName, message, "command", time);
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
                Clients.All.printMessage(userName, message, "normal", time);
            }            
        }

        /// <summary>
        /// Publish a message coming from the queue
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static void PublishMessageFromQueue(string message)
        {
            StockMessage messageObject = JsonConvert.DeserializeObject<StockMessage>(message);
            var time = DateTime.Now.ToString("yyyy-MM-dd, hh:mm:ss");

            var context = GlobalHost.ConnectionManager.GetHubContext<ChatRoom>();

            var onlineClient = context.Clients.Client(messageObject.UserId);
            onlineClient.printMessage("ChatBot", messageObject.Text, "result", time);
        }
        
        /// <summary>
        /// Initializes the Chatroom messages receiver
        /// </summary>
        public static void Initialize()
        {
            MQClient.Receive("stockResponse", PublishMessageFromQueue);
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
    }
}