using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ZebraCorn.StringRules;

namespace ZebraCorn
{
    public static class RuleGrouping
    {
        public static void AddRuleGrouping(this DiscordSocketClient client, Int32 repetitions = 3, Int32 timeInSeconds = 60, Boolean applyToAllChannels = false, IStringRule [] ruleExceptions = null, params String[] appliedChannels)
        {
            client.MessageReceived += (message) => OnMessageReceived(message, repetitions, timeInSeconds, applyToAllChannels, ruleExceptions, appliedChannels);
            Console.WriteLine("ADDED: Grouping rule");
        }
        
        private static async Task OnMessageReceived(SocketMessage message, Int32 repetitions, Int32 timeInSeconds, Boolean applyToAllChannels, IStringRule [] ruleExceptions, String[] appliedChannels)
        {
            Boolean isRuleApplied = (appliedChannels.Contains(message.Channel.Name) ||
                                     appliedChannels.Contains(message.Channel.Id.ToString())) || applyToAllChannels;

            Boolean sentBySelf = (message.Author.Id == Program.Client.CurrentUser.Id);
            
            if (sentBySelf || !isRuleApplied) return;
            
            var channel = Program.Client.GetChannel(message.Channel.Id) as IMessageChannel;
            
            var lastMessages =  await message.Channel.GetMessagesAsync(message.Id, Direction.Before, repetitions).FlattenAsync();

            var author = message.Author;
            var offenceCount = 0;
            
            foreach (IMessage lastMessage in lastMessages)
            {
                var timeSpan = (message.Timestamp - lastMessage.Timestamp ).TotalSeconds;
                if (author != lastMessage.Author || timeSpan > timeInSeconds) break;

                foreach (var ruleException in ruleExceptions)
                {
                    if (ruleException.IsValid(lastMessage.Content))
                        return;
                }
               
                
                if (author == lastMessage.Author && offenceCount == repetitions - 2)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning given to " + author.Username + " to group messages");
                    Console.ForegroundColor = ConsoleColor.White;

                    if (channel != null)
                    {
                        //await channel.SendMessageAsync(lastMessage.Author.Mention + "\n```WARNING !!!! \nGroup your messages!```");
                        EmbedBuilder embed = new()
                        {
                            // Embed property can be set within object initializer
                            Title = "⚠WARNING!⚠",
                            Description = "Group your messages!!!"
                        };

                        await channel.SendMessageAsync(embed: embed.Build());
                    }
                }

                offenceCount++;
            }
        }
    }
}