using ChatBot.Logic;
using ChatBot.Logic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialChat.ChatBotStarter
{
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
