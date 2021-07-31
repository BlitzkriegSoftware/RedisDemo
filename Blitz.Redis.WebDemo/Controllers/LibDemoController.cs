using Blitz.Redis.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blitz.Redis.WebDemo.Controllers
{

    /// <summary>
    /// Demo of <c>Blitz.Redis.Library</c>
    /// </summary>
    [Route("v1/LibDemo")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class LibDemoController : ControllerBase
    {
        private readonly ILogger<LibDemoController> _logger;
        private readonly BlitzRedisClient _client;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">ILogger</param>
        public LibDemoController(ILogger<LibDemoController> logger)
        {
            this._logger = logger;
            this._client = new BlitzRedisClient(new Library.Models.RedisConfiguration());
        }

        /// <summary>
        /// Set key value
        /// </summary>
        /// <param name="key">(sic)</param>
        /// <param name="value">(sic)</param>
        [HttpPost("Set")]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(Models.ErrorPayload), 400)]
        public void RedisSet(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            this._logger.LogInformation($"Set({key}, {value})");
            this._client.Set(key, value);
        }

        /// <summary>
        /// RedisGet
        /// </summary>
        /// <param name="key">(sic)</param>
        /// <returns>value</returns>
        [HttpGet("Get")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(Models.ErrorPayload), 400)]
        public string RedisGet(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            var value = this._client.Get(key);
            this._logger.LogInformation($"Get({key}) => {value}");
            return value;
        }

        /// <summary>
        /// Search
        /// </summary>
        /// <param name="keyMatchExpression">(sic)</param>
        /// <returns>value</returns>
        [HttpGet("Search")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(typeof(Models.ErrorPayload), 400)]
        public List<KeyValuePair<string, string>> RedisKvList(string keyMatchExpression)
        {
            if (string.IsNullOrWhiteSpace(keyMatchExpression)) throw new ArgumentNullException(nameof(keyMatchExpression));
            var values = this._client.MatchedKeyValues(keyMatchExpression);
            this._logger.LogInformation($"Get({keyMatchExpression}) => {values?.Count}");
            return values;
        }


    }
}
