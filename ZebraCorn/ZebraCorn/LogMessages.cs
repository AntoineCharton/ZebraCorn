using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace ZebraCorn
{
    public static class LogMessages
    {
        /// <summary>
        /// Log messages
        /// </summary>
        /// <param name="client">The discord client you want to process.</param>
        public static void AddLogMessages(this DiscordSocketClient client)
        {
            client.MessageReceived += (message) => MessageReceived(message);
            client.MessageUpdated += MessageUpdated;
            Console.WriteLine("Add logging messages in console");
        }
        
        static async Task MessageReceived(SocketMessage message)
        {
            Console.WriteLine($"#{message.Channel} {message.Author} {message.Timestamp} \n{message.Content} \n");
        }
        
        static async Task MessageUpdated(Cacheable<IMessage, ulong> before, SocketMessage after, ISocketMessageChannel channel)
        {
            // If the message was not in the cache, downloading it will result in getting a copy of `after`.
            var message = await before.GetOrDownloadAsync();
            Console.WriteLine("Message modified by" + after.Author.Username + " " + after.Timestamp);
            Console.WriteLine($"{message} -> {after}");
        }
    }
}