using System.Text.RegularExpressions;
using Discord;

namespace ZebraCorn.Rules.MessagesRules
{
    public class ContainsUrl: IMessagesRule
    {
        public bool IsValid(IMessage message)
        {
            var text = Regex.Replace(message.Content,
                @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)",
                "<a target='_blank' href='$1'>$1</a>");

            return text != message.Content;
        }
    }
}