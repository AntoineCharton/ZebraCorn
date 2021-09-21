using Discord;

namespace ZebraCorn.Rules.MessagesRules
{
    public class ContainsSticker : IMessagesRule
    {
        public bool IsValid(IMessage message)
        {
            if (message.Stickers.Count != 0)
                return true;

            return false;
        }
    }
}