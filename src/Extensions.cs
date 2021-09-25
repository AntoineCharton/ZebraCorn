using System;
using System.Collections.Generic;
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

        public static Int32 CountOf(this String text, params String[] checks)
        {
            Int32 __count = 0;
            
            foreach (String __check in checks)
            {
                Int32 __index = 0;
                
                while ((__index = text.IndexOf(value: __check, startIndex: __index, comparisonType: StringComparison.Ordinal)) != -1)
                {
                    __index += __check.Length;
                    __count++;
                }
            }
            return __count;
        }

        public static Int32[] IndexesOf(this String text, String check)
        {
            Int32 __index = 0;

            List<Int32> __occurrences = new();
                
            while ((__index = text.IndexOf(value: check, startIndex: __index, comparisonType: StringComparison.Ordinal)) != -1)
            {
                __occurrences.Add(__index);
                __index += check.Length;
            }

            return __occurrences.ToArray();
        }
    }
}