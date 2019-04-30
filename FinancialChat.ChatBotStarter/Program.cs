using ChatBot.Logic;
using ChatBot.Logic.Implementation;
using System;

namespace FinancialChat.ChatBotStarter
{
    /// <summary>
    /// Chat bot started application
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            IChatBot bot = new ChatBotImpl();
            Console.WriteLine("Starting financial chat bot...");
            bot.StartBot();
        }
    }
}
