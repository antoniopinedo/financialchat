using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Text;

namespace FinancialChat.Utils.Helpers
{
    /// <summary>
    /// Facade class for the Message Broker Client
    /// </summary>
    public class MessageBrokerFacade
    {
        /// <summary>
        /// The connection
        /// </summary>
        private readonly IConnection connection;

        /// <summary>
        /// Constructor of the class with parameters
        /// </summary>
        /// <param name="hostName">The host or server name</param>
        /// <param name="userName">User name</param>
        /// <param name="password">String password</param>
        public MessageBrokerFacade(string hostName, string userName, string password)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password
            };
            connection = connectionFactory.CreateConnection();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public MessageBrokerFacade()
        {
            ConnectionFactory connectionFactory = new ConnectionFactory();
            connection = connectionFactory.CreateConnection();
        }

        /// <summary>
        /// Method to publish messages into a queue
        /// </summary>
        /// <param name="queue">The queue name</param>
        /// <param name="data">Message to be published</param>
        public void Send(string queue, string data)
        {
            using (IModel channel = connection.CreateModel())
            {
                // Declare the queue
                channel.QueueDeclare(queue, false, false, false, null);

                // Publish the information in the queue
                channel.BasicPublish(string.Empty, queue, null, Encoding.UTF8.GetBytes(data));
            }
        }

        /// <summary>
        /// Method to receive messages from a queue based on a subscription
        /// </summary>
        /// <param name="queue">The queue</param>
        /// <param name="process">Action to execute on message received</param>
        public void Receive(string queue, Action<string> process)
        {
            using (var channel = connection.CreateModel())
            {
                var subscription = new Subscription(channel, queue, false);

                // Will attempt to get a new message on every loop
                while (true)
                {
                    // Blocks the cicle until a message is received
                    BasicDeliverEventArgs basicDeliveryEventArgs = subscription.Next();

                    // Get the message and execute the action
                    string messageContent = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);
                    process(messageContent);

                    // Acknowledge receiving the message so it is dequeued
                    subscription.Ack(basicDeliveryEventArgs);
                }
            }
        }
    }
}
