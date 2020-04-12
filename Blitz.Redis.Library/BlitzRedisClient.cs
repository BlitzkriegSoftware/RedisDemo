using System;
using StackExchange.Redis;
using Newtonsoft.Json;

// See: https://stackexchange.github.io/StackExchange.Redis/Basics

namespace Blitz.Redis.Library
{
    /// <summary>
    /// Client: Blitzkrieg Software Redis
    /// </summary>
    public class BlitzRedisClient
    {
        private readonly Models.RedisConfiguration _config;

        private readonly ConnectionMultiplexer redis;

        private BlitzRedisClient() { }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="config">RedisConfiguration</param>
        public BlitzRedisClient(Models.RedisConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            redis = ConnectionMultiplexer.Connect($"{_config.ConnectionString}");
        }

        /// <summary>
        /// Store a value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Set(string key, string value)
        {
            IDatabase db = redis.GetDatabase();
            db.StringSet(key, value);
        }

        /// <summary>
        /// Store
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="thing">of T</param>
        public void Set<T>(string key, T thing) where T : new()
        {
            IDatabase db = redis.GetDatabase();
            var json = JsonConvert.SerializeObject(thing);
            db.StringSet(key, json);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public string Get(string key)
        {
            IDatabase db = redis.GetDatabase();
            return db.StringGet(key);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <returns>instance of T</returns>
        public T Get<T>(string key)
        {
            IDatabase db = redis.GetDatabase();
            var json = db.StringGet(key);
            T value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }

        /// <summary>
        /// Redis Configuration
        /// </summary>
        public Models.RedisConfiguration Config
        {
            get
            {
                return _config;
            }
        }

    }
}
