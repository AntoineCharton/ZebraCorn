using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using Tommy;

namespace ZebraCorn
{
    internal static class Program
    {
        #region Fields
        
        private static String[] _illegalTags;

        public static DiscordSocketClient Client { get; private set; }

        #endregion

        #region Methods
        
        private static void Main() => Run().GetAwaiter().GetResult();
        
        public static async Task Run()
        {
            // Setup
            DiscordSocketConfig config = new() { MessageCacheSize = 100 };
            Client = new DiscordSocketClient(config: config);

            using(StreamReader reader = File.OpenText(path: "config.toml"))
            {
                // Parse the table
                TomlTable table = TOML.Parse(reader);

                String token = table[key: "bot-token"];

                _illegalTags = (from TomlNode __node in table[key: "illegal-tags"] select __node.ToString()).ToArray();

                await Client.LoginAsync(tokenType: TokenType.Bot, token: token);
                await Client.StartAsync();
            }

            Client.Ready += () =>
            {
                Console.WriteLine(value: "Sam-Unchained is Online! \nAll Systems nominal. \nWEAPONS: Hot \nMISSION: The destruction of any and all rule-breakers!");
                return Task.CompletedTask;
            };
            
            //Rules
            Client.AddLogMessages();
            Client.AddRuleGrouping(applyToAllChannels: true); //appliedChannels: _groupingRuleAppliedChannels);
            Client.AddRuleTagging(applyToAllChannels: true, illegalTags: _illegalTags);
            ConsoleTalk.CheckForMessages();
            //"Don't @ mods unless it's urgent. Better be a life and death situation!!! \nUse reply instead.");

            await Task.Delay(millisecondsDelay: -1);
            Client.Dispose();
        }
        
        #endregion
    }
}