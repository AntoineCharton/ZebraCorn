using System;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

namespace ZebraCorn
{
    public static class RuleTagging
    {
        public static void AddRuleTagging(this DiscordSocketClient client, String[] illegalTags, Boolean applyToAllChannels = false, params String[] appliedChannels)
        {
            client.MessageReceived += (message) => MessageReceived(message, illegalTags, applyToAllChannels, appliedChannels);
            Console.WriteLine("ADDED: Tagging rule");
        }

        static async Task MessageReceived(SocketMessage message, String[] illegalTags, Boolean applyToAllChannels,  String[] appliedChannels)
        {
            //if(message.Author.Id == client.CurrentUser.Id || (channelName != AllChannels && message.Channel.Name != channelName)) return;
            Boolean isRuleApplied = (appliedChannels.Contains(message.Channel.Name) ||
                                     appliedChannels.Contains(message.Channel.Id.ToString())) || applyToAllChannels;

            Boolean sentBySelf = (message.Author.Id == Program.Client.CurrentUser.Id);
            
            if (sentBySelf || !isRuleApplied) return;

            var channel = Program.Client.GetChannel(message.Channel.Id) as IMessageChannel;
            
            var lastMessages =  await message.Channel.GetMessageAsync(message.Id);

            foreach (String illegalTag in illegalTags)
            {
                if (lastMessages.Content.Contains(illegalTag))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning given to " + message.Author.Username + " to avoid word");
                    Console.ForegroundColor = ConsoleColor.White;
                    
                    if (channel != null)
                    {
                        //await
                        EmbedBuilder embed = new()
                        {
                            // Embed property can be set within object initializer
                            Title = "⚠WARNING!⚠",
                            Description = "Don't tag Mods please!!!!"
                        };

                        await channel.SendMessageAsync(embed: embed.Build());
                    } 
                    //await channel.SendMessageAsync(lastMessages.Author.Mention + "\n```WARNING !!!! \n" + warningMessage + "```");
                        
                    break;
                }
            }
        }
    }
}