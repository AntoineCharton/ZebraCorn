using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace ZebraCorn
{
    public static class GroupRule
    {
        private const string AllChannels = "All";
    
        /// <summary>
        /// Enforce users to groupe their messages
        /// </summary>
        /// <param name="client">The discord client you want to process.</param>
        /// <param name="channelName">The channel name you want to filter. "all" will not target any channel</param>
        /// <param name="repetitions">The number of repetitions before the warning occurs. Should be more than 2.</param>
        /// <param name="timeInSeconds">The time in seconds between messages to trigger the warning</param>
        public static void AddGroupingRule(this DiscordSocketClient client, string channelName = AllChannels, int repetitions = 3, int timeInSeconds = 60)
        {
            client.MessageReceived += (message) => MessageReceived(message, client,repetitions, channelName, timeInSeconds);
            Console.WriteLine("Add grouping rule");
        }
        
        static async Task MessageReceived(SocketMessage message, DiscordSocketClient client, int repetitions, string channelName, int timeInSeconds = 60)
        {
            //Ignore bot messages
            if(message.Author.Id == client.CurrentUser.Id && (channelName != AllChannels) || (channelName != AllChannels && message.Channel.Name != channelName))
                return;
            
            var channel = client.GetChannel(message.Channel.Id) as IMessageChannel;
            var lastMessages =  await message.Channel.GetMessagesAsync(message.Id, Direction.Before, repetitions).FlattenAsync();

            var author = message.Author;
            var offenceCount = 0;
            foreach (var lastMessage in lastMessages)
            {
                var timeSpan = (message.Timestamp - lastMessage.Timestamp ).TotalSeconds;
                if(author != lastMessage.Author || timeSpan > timeInSeconds)
                    break;
                if (author == lastMessage.Author && offenceCount == repetitions - 2)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning given to " + author.Username + " to group messages");
                    Console.ForegroundColor = ConsoleColor.White;
                    if (channel != null)
                        await channel.SendMessageAsync(lastMessage.Author.Mention + "\n```WARNING !!!! \nGroup your messages!```");
                }

                offenceCount++;
            }
        }
    }
}