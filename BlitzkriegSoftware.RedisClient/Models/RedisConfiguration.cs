namespace BlitzkriegSoftware.RedisClient.Models
{
    /// <summary>
    /// REDIS Coniguration
    /// </summary>
    public class RedisConfiguration
    {
        /// <summary>
        /// Default: 127.0.0.1:6379 (admin)
        /// </summary>
        public const string RedisLocalHostDefault = "127.0.0.1:6379,allowAdmin=true";

        /// <summary>
        /// Connection String
        /// </summary>
        public string ConnectionString { get; set; } = RedisLocalHostDefault;


        /// <summary>
        /// Is connection configuration valid
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(this.ConnectionString);
            }
        }

        /// <summary>
        /// From a config key/value pair set the correct property
        /// </summary>
        /// <param name="key">(sic)</param>
        /// <param name="value">(sic)</param>
        public void SetProperty(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            switch (key.ToLowerInvariant())
            {
                case "redis-connection": this.ConnectionString = value; break;
            }
        }

        /// <summary>
        /// Debug String
        /// </summary>
        /// <returns>Debug String</returns>
        public override string ToString()
        {
            return $"{this.ConnectionString}";
        }

    }
}
