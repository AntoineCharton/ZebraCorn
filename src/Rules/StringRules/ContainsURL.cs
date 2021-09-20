using System;
using System.Text.RegularExpressions;

namespace ZebraCorn.StringRules
{
    public class ContainsURL: IStringRule
    {
        public bool IsValid(string value)
        {
            var text = Regex.Replace(value,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "<a target='_blank' href='$1'>$1</a>");

            if (text != value)
                return true;

            return false;
        }
    }
}