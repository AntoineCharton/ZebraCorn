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

            if (message.Content.Contains("```")) return; //exit early

            String[] contains = {
                "MonoBehaviour", "using", "UnityEngine", "0f", "1f", "System.Collections", "System.Collections.Generic",
                "public", "private", "internal", "int ", "float", "void", "Start()", "OnEnable()", "OnDisable()", "Update()", "Vector2", "Vector3", "Vector4", "Quaternion", "class", "-=", "+="
            };

            var codeScore = contains.Count(value => message.Content.Contains(value));

            codeScore += message.Content.Count(character => character is '{' or '}' or ';' or '(' or ')' or '=' or '+' or '-' or '*' or '\n');

            var normalizedCodeScore = ((float) codeScore / message.Content.Length) * 100;
            
            //For some reason messages containing URL Increase the count a lot
            var ContainsURL = new ContainsUrl();
            if (ContainsURL.IsValid(message))
                normalizedCodeScore -= 3;

            if (message.Content.Contains("c++"))
                normalizedCodeScore -= 5;
            
            Console.WriteLine("Code score" + normalizedCodeScore);

            if (normalizedCodeScore > 6)
            {
                if(message.Content.Contains('`'))
                {
                    if (normalizedCodeScore > 7 && message.Content.Contains('\n'))
                    {
                        Console.WriteLine("Warning given to user to format code " + normalizedCodeScore);
                        EmbedBuilder embed = new()
                        {
                            Title = "🗨️USE MARKDOWN🗨️",
                            Description =
                                "\n ***\\`\\`\\`cs \n class YourClass \n { \n your awesome code \n } \n \\`\\`\\`*** \n "
                        };
                        await message.Channel.SendMessageAsync(embed: embed.Build());
                    }
                    else if(message.Content.Contains('\n'))
                    {
                        Console.WriteLine("Tip given to improve formatting" + normalizedCodeScore);
                        EmbedBuilder embed = new()
                        {
                            Title = "🗨️USE MARKDOWN🗨️",
                            Description = "\n \n ***\\` YourLineOfCode () \\`*** \n \n OR \n \n ***\\`\\`\\`cs \n class YourClass \n { \n your awesome code \n } \n \\`\\`\\`***"
                        };
                        await message.Channel.SendMessageAsync(embed: embed.Build());
                    }
                }
                else
                {
                    if (message.Content.Contains('\n'))
                    {
                        Console.WriteLine("Tip given to improve formatting" + normalizedCodeScore);
                        EmbedBuilder embed = new()
                        {
                            Title = "🗨️USE MARKDOWN🗨️",
                            Description =
                                "\n ***\\`\\`\\`cs \n class YourClass \n { \n your awesome code \n } \n \\`\\`\\`*** \n "
                        };

                        await message.Channel.SendMessageAsync(embed: embed.Build());
                    }
                }   
            }
        }
    }
}