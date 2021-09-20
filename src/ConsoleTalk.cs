using System;
using System.Threading.Tasks;

namespace ZebraCorn
{
    public static class ConsoleTalk
    {
        public static async Task CheckForMessages()
        {
            string? input = Console.ReadLine();

            if (input != null)
            {
                await Reply(input);
            }
            
            await Task.Delay(1000);

            CheckForMessages();
        }

        
        static async Task Reply(String message)
        {
            await Program.Client.GetGuild(id: 889277421994520577).GetTextChannel(id: 889277422766288930).SendMessageAsync(message);
        }
    }
}