using System;
using System.Text.RegularExpressions;

namespace ZebraCorn
{
    public class MessageContainsHyperlink: IMessageRule
    {
        public bool IsValid(string message)
        {
            Match url = Regex.Match(message, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            Console.WriteLine(url.ToString());
            return true;
        }
    }
}