using System;

namespace ZebraCorn.Rules.CodeChecks
{
    public class MemberAccessCheck : ICodeCheck
    {
        public Single CodeRating(String text)
        {
            Int32[] __periodIndexes = text.IndexesOf(check: ".");

            Int32 memberAccesses = 0;

            foreach (Int32 __periodIndex in __periodIndexes)
            {
                Char before = text[__periodIndex - 1];
                Char after  = text[__periodIndex + 1];

                if (before is not (' ' or '\n') && after is not (' ' or '\n')) //both before and after . must be occupied.
                {
                    memberAccesses += 1;
                }
            }
                
            Console.WriteLine("Member Accesses = " + memberAccesses);

            return memberAccesses;
        }
    }
}