using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace ZebraCorn
{
    internal class Program
    {
        //IDs
        private const string SamId = "595460255245139968";
        private const string ChizaruuId = "209324808742240256";
        private const string WalterId = "221195885680394240";
        private const string AnishId = "769917059604807702";
        private const string DevGameId = "734754644286504991";
        private const string GeekZebraId = "136268213464858624";
        private const string ModId = "733691105383809034";
        private const string TestModId = "889375781522915368";
        private DiscordSocketClient _client;
        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
	
        public async Task MainAsync()
        {
            // Setup
            var config = new DiscordSocketConfig { MessageCacheSize = 100 };
            _client = new DiscordSocketClient(config);
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("DiscordBot"));
            await _client.StartAsync();
            _client.Ready += () =>
            {
                Console.WriteLine("Bot is connected!");
                return Task.CompletedTask;
            };
            
            //Rules
            _client.AddLogMessages();
            _client.AddGroupingRule("help");
            _client.AddForbiddenStringRule(new []{SamId, ChizaruuId, WalterId, AnishId, GeekZebraId, DevGameId, TestModId, ModId}, "Don't @ mods unless it's urgent. Better be a life and death situation!!! \nUse reply instead.");

            await Task.Delay(-1);
            _client.Dispose();
        }
    }
}