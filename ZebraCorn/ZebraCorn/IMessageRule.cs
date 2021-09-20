namespace ZebraCorn
{
    public interface IMessageRule
    {
        bool IsValid(string message);
    }
}