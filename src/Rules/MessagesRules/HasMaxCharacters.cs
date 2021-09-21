using Discord;

namespace ZebraCorn.Rules.MessagesRules
{
    public class HasMaxCharacters: IMessagesRule
    {
        private readonly int _maxCharacters;
        
        public HasMaxCharacters(int number = 90)
        {
            _maxCharacters = number;
        }
        
        public bool IsValid(IMessage message)
        {
            if (message.Content.Length > _maxCharacters)
                return true;

            return false;
        }
    }
}