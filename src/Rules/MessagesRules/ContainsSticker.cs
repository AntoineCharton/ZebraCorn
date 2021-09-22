using System;
using System.Linq;
using Discord;

namespace ZebraCorn.Rules.MessagesRules
{
    public class ContainsSticker : IMessagesRule
    {
        public bool IsValid(IMessage message)
        {
            //Stickers was throwing errors. Only sticker post can have empty messages
            if (message.Attachments!= null && message.Attachments.ToArray().Length != 0)
                return false;
            
            if (message.Content == "")
                return true;

            return false;
        }
    }
}