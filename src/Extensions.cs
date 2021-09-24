using System;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

using Discord.WebSocket;

namespace ZebraCorn
{
    public static class Extensions
    {
        [MethodImpl(methodImplOptions: AggressiveInlining)]
        public static Boolean IsMod(this SocketUser author) => Program.Mods.Contains(author.Id.ToString()); //TODO: Actually Check Author Role?
    }
}