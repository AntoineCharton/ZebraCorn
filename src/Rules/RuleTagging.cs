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
            Boolean isRuleApplied = (appliedChannels.Contains(message.Channel.Name) ||
                                     appliedChannels.Contains(message.Channel.Id.ToString())) || applyToAllChannels;

            Boolean isBot = message.Author.IsBot;
            
            if (isBot || !isRuleApplied) return;
            Boolean isSentByMod = illegalTags.Contains(message.Author.Id.ToString());

            var channel = Program.Client.GetChannel(message.Channel.Id) as IMessageChannel;
            
            var lastMessages =  await message.Channel.GetMessageAsync(message.Id);

            foreach (String illegalTag in illegalTags)
            {
                if (!lastMessages.Content.Contains(illegalTag)) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning given to " + message.Author.Username + " to stop tagging mods.");
                Console.ForegroundColor = ConsoleColor.White;
                    
                if (channel != null)
                {
                    EmbedBuilder embed = new()
                    {
                        Title = "⚠WARNING!⚠",
                        Description = "Don't tag Mods please!!!!"
                    };

                    await channel.SendMessageAsync(embed: embed.Build());
                }

                break;
            }
        }
    }
}