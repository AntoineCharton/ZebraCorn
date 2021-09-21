using Discord;

namespace ZebraCorn.Rules.MessagesRules
{
    public class ContainsAttachement : IMessagesRule
    {
        public bool IsValid(IMessage message)
        {
            if (message.Attachments.Count != 0)
                return true;

            return false;
        }
    }
}