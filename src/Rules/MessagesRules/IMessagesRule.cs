using Discord;

namespace ZebraCorn.Rules.MessagesRules
{
    public interface IMessagesRule
    {
        bool IsValid(IMessage message);
    }
}