using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ZebraCorn.Rules.MessagesRules;

namespace ZebraCorn.Rules
{
    public static class RuleFormatCode
    {
        public static void AddRuleFormatCode(this DiscordSocketClient client, Boolean applyToAllChannels = false, params String[] appliedChannels)
        {
            client.MessageReceived += (message) =>
                OnMessageReceived(message, applyToAllChannels, appliedChannels);
            Console.WriteLine("ADDED: Grouping rule");
        }

        private static async Task OnMessageReceived(SocketMessage message, Boolean applyToAllChannels, String[] appliedChannels)
        {
            Boolean isRuleApplied = (appliedChannels.Contains(message.Channel.Name) ||
                                     appliedChannels.Contains(message.Channel.Id.ToString())) || applyToAllChannels;

            if(message.Author.IsBot || message.Content.Length < 100 && !isRuleApplied)
                return;

            var codeScore = 0;
            var contains = new string[] {"MonoBehaviour", "Using", "UnityEngine", "0f", "System.Collections", "System.Collections.Generic", "public", "private", "internal", "float", "void", "Update", "Quaternion", "class", "-=", "+="};
            
            foreach (var value in contains)
            {
                if (message.Content.Contains(value))
                    codeScore += 1 ;
            }
            
            foreach (var character in message.Content)
            {
                if (
                    character == '{' ||
                    character == '}' ||
                    character == '(' ||
                    character == ')' ||
                    character == '=' ||
                    character == '+' ||
                    character == '-' ||
                    character == '*' ||
                    character == '\n'
                )
                    codeScore++;
            }
            
            var normalizedCodeScore = ((float) codeScore / message.Content.Length) * 100;
            Console.WriteLine(normalizedCodeScore);
            if (normalizedCodeScore > 5 && !message.Content.Contains("```"))
            {
                EmbedBuilder embed = new()
                {
                    // Embed property can be set within object initializer
                    Title = "⚠WARNING!⚠",
                    Description = "Format your code!!!"
                };

                await message.Channel.SendMessageAsync(embed: embed.Build());
            }
        }
    }
}