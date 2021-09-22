using System;
using Discord;

namespace ZebraCorn.Rules.MessagesRules
{
    public class ContainsReply: IMessagesRule
    {
        public bool IsValid(IMessage message)
        {
            if (message.Reference == null)
                return false;
            
            if (message.Reference.MessageId.IsSpecified )
                return true;

            return false;
        }
    }
}