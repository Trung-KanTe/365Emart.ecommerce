namespace Commerce.Command.Contract.Shared
{
    /// <summary>
    /// Define message arguments contain argument name and argument value
    /// </summary>
    public class MessageArgs
    {
        public string ArgName { get; }
        public string ArgValue { get; }

        public MessageArgs(string argName, string argValue)
        {
            ArgName = argName;
            ArgValue = argValue;
        }
    }
}