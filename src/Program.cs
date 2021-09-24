using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;

using Tommy;

namespace ZebraCorn
{
    using Rules;
    using Rules.MessagesRules;
    
    internal static class Program
    {
        #region Fields
        
        private static String[] _illegalTags;
        private static String[] _appliedChannels;

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
                
                _appliedChannels = (from TomlNode __node in table[key: "applied-channels"] select __node.ToString()).ToArray();

                await Client.LoginAsync(tokenType: TokenType.Bot, token: token);
                await Client.StartAsync();
            }

            Client.Ready += () =>
            {
                Console.WriteLine(value: "Sam-Unchained is Online! \nAll Systems nominal. \nWEAPONS: Hot \nMISSION: The destruction of any and all rule-breakers!");
                return Task.CompletedTask;
            };
            
            //StringRules
            var containsUrl = new ContainsUrl();
            var maxCharacters = new HasMaxCharacters();
            var containsAttachment = new ContainsAttachement();
            var containsSticker = new ContainsSticker();
            var containsReply = new ContainsReply();
            
            Client.AddLogMessages();
            //Rules
            Client.AddRuleGrouping(appliedChannels:   _appliedChannels, repetitions: 6, ruleExceptions: new IMessagesRule[]{containsUrl, maxCharacters, containsAttachment, containsSticker, containsReply});
            Client.AddRuleTagging(appliedChannels:    _appliedChannels, illegalTags: _illegalTags);
            Client.AddRuleFormatCode(appliedChannels: _appliedChannels);

            await Task.Delay(-1);
            Client.Dispose();
        }
        
        #endregion
    }
}
