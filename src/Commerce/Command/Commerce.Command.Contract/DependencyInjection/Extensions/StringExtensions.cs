using System.Text.RegularExpressions;
using Commerce.Command.Contract.Shared;

namespace Commerce.Command.Contract.DependencyInjection.Extensions
{
    /// <summary>
    /// Extensions for string
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Replace arguments in message by argument values
        /// </summary>
        /// <param name="msgText"></param>
        /// <param name="args"></param>
        public static string FillArgs(this string msgText, List<MessageArgs> args)
        {
            if (string.IsNullOrEmpty(msgText))
                return msgText;

            foreach (var arg in args)
            {
                msgText = msgText.Replace(arg.ArgName, arg.ArgValue, StringComparison.OrdinalIgnoreCase);
            }
            return msgText;
        }
    }
}