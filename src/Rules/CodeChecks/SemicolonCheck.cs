using System;
using System.Linq;

namespace ZebraCorn.Rules.CodeChecks
{
    public class SemicolonCheck : ICodeCheck
    {
        public Single CodeRating(String text)
        {
            Int32[] __semicolonIndexes = text.IndexesOf(check: ";");
                
            Int32 endingsWithSemiColons = __semicolonIndexes.Select(index => text[index + 1] == '\n').Count();
                
            Console.WriteLine("Endings With Semicolons = " + endingsWithSemiColons);
                
            return endingsWithSemiColons;
        }
    }
}