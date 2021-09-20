using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace ZebraCorn
{
    public static class ForbiddenStringsRule
    { 
        private const string AllChannels = "All";

        /// <summary>
        /// Enforce users to not use certains strings
        /// </summary>
        /// <param name="client">The discord client you want to process.</param>
        /// <param name="forbiddenStrings">Array of string that will trigger the rule.</param>
        /// <param name="warningMessage">The message displayed.</param>
        /// <param name="channelName">The channel name you want to filter. "all" will not target any channel.</param>
        public static void AddForbiddenStringRule(this DiscordSocketClient client, string [] forbiddenStrings, string warningMessage = "Forbidden message!!!", string channelName = AllChannels)
        {
            client.MessageReceived += (message) => MessageReceived(message, client, forbiddenStrings, warningMessage, channelName);
            Console.WriteLine("Add forbiden string rule");
        }
        
        static async Task MessageReceived(SocketMessage message, DiscordSocketClient client, string [] forbiddenStrings, string warningMessage = "Forbidden message!!!", string channelName = AllChannels)
        {
            if(message.Author.IsBot || (channelName != AllChannels && message.Channel.Name != channelName))
                return;
            var channel = client.GetChannel(message.Channel.Id) as IMessageChannel;
            var lastMessages =  await message.Channel.GetMessageAsync(message.Id);

            foreach (var forbiddenString in forbiddenStrings)
            {
                if (lastMessages.Content.Contains(forbiddenString))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning given to " + message.Author.Username + " to avoid word");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (channel != null)
                        await channel.SendMessageAsync(lastMessages.Author.Mention + "\n```WARNING !!!! \n" + warningMessage + "```");
                    break;
                }
            }
        }
    }
}