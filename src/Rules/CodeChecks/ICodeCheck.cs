using System;

namespace ZebraCorn.Rules.CodeChecks
{
    internal interface ICodeCheck
    {
        /// <summary> Returns your the likelihood of this text being code on a [0-1] scale. </summary>
        public Single CodeRating(String text);
    }
}