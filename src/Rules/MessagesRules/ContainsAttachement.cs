using System.Linq;
using Discord;

namespace ZebraCorn.Rules.MessagesRules
{
    public class ContainsAttachement : IMessagesRule
    {
        public bool IsValid(IMessage message)
        {
            if (message.Attachments!= null && message.Attachments.ToArray().Length == 0)
                return false;
            
            if (message.Attachments != null && message.Attachments.Count != 0)
                return true;

            return false;
        }
    }
}