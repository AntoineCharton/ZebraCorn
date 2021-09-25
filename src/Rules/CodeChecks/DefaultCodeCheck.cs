using System;

namespace ZebraCorn.Rules.CodeChecks
{
    public class DefaultCodeCheck : ICodeCheck
    {
        public Single CodeRating(String text)
        {
            String[] code = {
                "()", "void ", "class ", "public ", "private ", "internal ", "float ", "string ", "(this ", " = ", "-=", "+=", "++",
            };
                
            Int32 count = text.CountOf(code);
                
            Console.WriteLine("DefaultCodeCheck Count = " + count);

            return count;
        }
    }
}