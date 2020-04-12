using CommandLine;

namespace Blitz.Redis.Demo.Models
{
    public class CommandOptions
    {
        /// <summary>
        /// Verbose Output
        /// </summary>
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        public const int MessageCountDefault = 50;

        /// <summary>
        /// Message Count
        /// </summary>
        [Option('m', "MessageCount", Required = false, HelpText = "How many messages to generate", Default = MessageCountDefault)]
        public int MessageCount { get; set; } = MessageCountDefault;

        /// <summary>
        /// REDIS Connection String
        /// </summary>
        [Option('c', "ConnectionString", Required = false, HelpText = "Redis Commandline", Default = "127.0.0.1:6379")]
        public string RedisConnectionString { get; set; }
    }
}
