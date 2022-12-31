using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BlitzkriegSoftware.RedisClient
{
    /// <summary>
    /// Client: Blitzkrieg Software Redis
    /// <para>Avoid recreating connection multiplexers by instancing this as a singleton</para>
    /// <para>See: <![CDATA[https://stackexchange.github.io/StackExchange.Redis/Basics]]></para>
    /// </summary>
    public class BlitzRedisClient : IDisposable
    {

        #region "Vars, Constants"

        /// <summary>
        /// Default: REDIS DB (-1)
        /// </summary>
        public const int RedisDefaultDb = -1;

        /// <summary>
        /// Default: REDIS Server (0)
        /// </summary>
        public const int RedisDefaultServer = 0;

        private readonly Models.RedisConfiguration _config;

        private ConnectionMultiplexer _redisConnectMultiplexer;

        #endregion

        #region "CTOR"

        [ExcludeFromCodeCoverage]
        private BlitzRedisClient() { }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="config">RedisConfiguration</param>
        public BlitzRedisClient(Models.RedisConfiguration config)
        {
            this._config = config ?? throw new ArgumentNullException(nameof(config));
            this._redisConnectMultiplexer = ConnectionMultiplexer.Connect($"{this._config.ConnectionString}");
        }

        #endregion

        #region "Properties"

        /// <summary>
        /// Redis Configuration
        /// </summary>
        public Models.RedisConfiguration Config
        {
            get
            {
                return this._config;
            }
        }

        /// <summary>
        /// Multiplexer
        /// </summary>
        public ConnectionMultiplexer Multiplexer
        {
            get
            {
                return this._redisConnectMultiplexer;
            }
        }

        #endregion

        /// <summary>
        /// Set a Key/Value
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        public void Set(string key, string value, int dbIndex = RedisDefaultDb)
        {
            IDatabase db = this._redisConnectMultiplexer.GetDatabase(dbIndex);
            db.StringSet(key, value);
        }

        /// <summary>
        /// Set a Key/Value
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="thing">of T</param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        public void Set<T>(string key, T thing, int dbIndex = RedisDefaultDb) where T : new()
        {
            IDatabase db = this._redisConnectMultiplexer.GetDatabase(dbIndex);
            var json = JsonConvert.SerializeObject(thing);
            db.StringSet(key, json);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        /// <returns>value</returns>
        public string Get(string key, int dbIndex = RedisDefaultDb)
        {
            IDatabase db = this._redisConnectMultiplexer.GetDatabase(dbIndex);
            return db.StringGet(key);
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">key</param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        /// <returns>instance of T</returns>
        public T Get<T>(string key, int dbIndex = RedisDefaultDb)
        {
            IDatabase db = this._redisConnectMultiplexer.GetDatabase(dbIndex);
            var json = db.StringGet(key);
            T value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }

        /// <summary>
        /// Delete a key/value, ignores not found
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        public bool Delete(string key, int dbIndex = RedisDefaultDb)
        {
            IDatabase db = this._redisConnectMultiplexer.GetDatabase(dbIndex);
            return db.KeyDelete(key);
        }

        /// <summary>
        /// Clear All Keys
        /// <para>Warning! Use carefully</para>
        /// </summary>
        /// <param name="serverIndex">(optional) Server Index, default <c>RedisDefaultServer</c></param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        public void ClearAll(int serverIndex = RedisDefaultServer, int dbIndex = RedisDefaultDb)
        {
            var endpoint = this._redisConnectMultiplexer.GetEndPoints()[serverIndex];
            var redisServer = this._redisConnectMultiplexer.GetServer(endpoint);
            redisServer.FlushDatabase(dbIndex);
        }

        /// <summary>
        /// Get all matching keys
        /// <para>
        /// <list type="bullet">
        /// <listheader>
        /// <term>Expression</term>
        /// <description>Matches</description>
        /// </listheader>
        /// <item>
        /// <term><![CDATA[?]]></term>
        /// <description>Matches single character</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[*]]></term>
        /// <description>Matches any number of characters (good for begins with or ends with)</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[[{chars}]]]></term>
        /// <description>Matches chars in list</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[\]]></term>
        /// <description>Escape litteral chars</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="keyMatchExpression">Simple RegEx</param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        /// <returns>List or empty</returns>
        public RedisKey[] MatchedKeys(string keyMatchExpression, int dbIndex = RedisDefaultDb)
        {
            IDatabase db = this._redisConnectMultiplexer.GetDatabase(dbIndex);
            var matches = (RedisKey[])db.Execute("KEYS", keyMatchExpression);
            return matches;
        }

        /// <summary>
        /// Fetch a list of Key/Value Pairs that match
        /// <para>
        /// <list type="bullet">
        /// <listheader>
        /// <term>Expression</term>
        /// <description>Matches</description>
        /// </listheader>
        /// <item>
        /// <term><![CDATA[?]]></term>
        /// <description>Matches single character</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[*]]></term>
        /// <description>Matches any number of characters (good for begins with or ends with)</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[[{chars}]]]></term>
        /// <description>Matches chars in list</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[\]]></term>
        /// <description>Escape litteral chars</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="keyMatchExpression">Simple RegEx</param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        /// <returns>Key Value List</returns>
        public List<KeyValuePair<string, string>> MatchedKeyValues(string keyMatchExpression, int dbIndex = RedisDefaultDb)
        {
            var d = new List<KeyValuePair<string, string>>();
            IDatabase db = this._redisConnectMultiplexer.GetDatabase(dbIndex);
            var matches = this.MatchedKeys(keyMatchExpression, dbIndex);
            if ((matches != null) && (matches.Length > 0))
            {
                foreach (var key in matches)
                {
                    var value = db.StringGet(key);
                    d.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            return d;
        }

        /// <summary>
        /// Delete keys based on a key regex expression (case sensitive) or you can pass a literal string
        /// <para>
        /// <list type="bullet">
        /// <listheader>
        /// <term>Expression</term>
        /// <description>Matches</description>
        /// </listheader>
        /// <item>
        /// <term><![CDATA[?]]></term>
        /// <description>Matches single character</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[*]]></term>
        /// <description>Matches any number of characters (good for begins with or ends with)</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[[{chars}]]]></term>
        /// <description>Matches chars in list</description>
        /// </item>
        /// <item>
        /// <term><![CDATA[\]]></term>
        /// <description>Escape litteral chars</description>
        /// </item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="keyMatchExpression">Simple RegEx</param>
        /// <param name="dbIndex">(optional) index, default <c>RedisDefaultDb</c></param>
        /// <returns>Count of deleted keys</returns>
        public long ClearMatching(string keyMatchExpression, int dbIndex = RedisDefaultDb)
        {
            long deleted = 0;
            var matches = this.MatchedKeys(keyMatchExpression, dbIndex);
            if ((matches != null) && (matches.Length > 0))
            {
                IDatabase db = this._redisConnectMultiplexer.GetDatabase(dbIndex);
                deleted = db.KeyDelete(matches);
            }
            return deleted;
        }

        #region "IDisposable"

        private bool disposed; // to detect redundant calls

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this._redisConnectMultiplexer?.Dispose();
                    this._redisConnectMultiplexer = null;
                }
                this.disposed = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
