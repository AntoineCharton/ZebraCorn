namespace ZebraCorn.StringRules
{
    public class HasMaxCharacters: IStringRule
    {
        private readonly int maxCharacters;
        
        public HasMaxCharacters(int number = 90)
        {
            maxCharacters = number;
        }
        
        public bool IsValid(string value)
        {
            if (value.Length > maxCharacters)
                return true;

            return false;
        }
    }
}