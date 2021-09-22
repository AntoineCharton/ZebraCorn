using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using ZebraCorn.Rules.MessagesRules;

namespace ZebraCorn.Rules
{
    public static class RuleFormatCode
    {
        public static void AddRuleFormatCode(this DiscordSocketClient client, Boolean applyToAllChannels = false,
            params String[] appliedChannels)
        {
            client.MessageReceived += (message) =>
                OnMessageReceived(message, applyToAllChannels, appliedChannels);
            Console.WriteLine("ADDED: Grouping rule");
        }

        private static async Task OnMessageReceived(SocketMessage message, Boolean applyToAllChannels,
            String[] appliedChannels)
        {
            Boolean isRuleApplied = (appliedChannels.Contains(message.Channel.Name) ||
                                     appliedChannels.Contains(message.Channel.Id.ToString())) || applyToAllChannels;

            if (message.Author.IsBot || message.Content.Length < 50 && !isRuleApplied)
                return;

            var codeScore = 0;
            var contains = new string[]
            {
                "MonoBehaviour", "Using", "UnityEngine", "0f", "System.Collections", "System.Collections.Generic",
                "public", "private", "internal", "float", "void", "Update", "Quaternion", "class", "-=", "+="
            };

            foreach (var value in contains)
            {
                if (message.Content.Contains(value))
                    codeScore++;
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
            
            //For some reason messages containing URL Increase the count a lot
            var ContainsURL = new ContainsUrl();
            if (ContainsURL.IsValid(message))
                normalizedCodeScore -= 3;
            
            Console.WriteLine("Code score" + normalizedCodeScore);
            if (normalizedCodeScore > 5 && !message.Content.Contains("```") && !message.Content.Contains('`'))
            {
                Console.WriteLine("Warning given to user to format code " + normalizedCodeScore);
                if (normalizedCodeScore > 7 && message.Content.Contains('\n'))
                {
                    EmbedBuilder embed = new()
                    {
                        // Embed property can be set within object initializer
                        Title = "⚠WARNING!⚠",
                        Description =
                            "Use markdown!!! \n \n ***\\`\\`\\` \n class YourClass \n { \n your awsome code \n } \n \\`\\`\\`*** \n "
                    };
                    await message.Channel.SendMessageAsync(embed: embed.Build());
                }
                else
                {
                    EmbedBuilder embed = new()
                    {
                        // Embed property can be set within object initializer
                        Title = "🗨️USE MARKDOWN🗨️",
                        Description = "\n \n ***\\` YourLineOfCode () \\`*** \n \n OR \n \n ***\\`\\`\\` \n class YourClass \n { \n your awsome code \n } \n \\`\\`\\`***"
                    };
                    await message.Channel.SendMessageAsync(embed: embed.Build());
                }
            }
            else if(normalizedCodeScore > 5 && !message.Content.Contains("```"))
            {
                if (message.Content.Contains('\n') && normalizedCodeScore > 6)
                {
                    Console.WriteLine("Tip given to improve formating" + normalizedCodeScore);
                    EmbedBuilder embed = new()
                    {
                        // Embed property can be set within object initializer
                        Title = "🗨️USE MARKDOWN🗨️",
                        Description =
                            "\n ***\\`\\`\\` \n class YourClass \n { \n your awsome code \n } \n \\`\\`\\`*** \n "
                    };

                    await message.Channel.SendMessageAsync(embed: embed.Build());
                }
            }
        }
    }
}