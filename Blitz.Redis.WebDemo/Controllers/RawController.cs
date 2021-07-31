using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Blitz.Redis.WebDemo.Controllers
{
    /// <summary>
    /// Raw Demo (REDIS Cached Values)
    /// </summary>
    [Route("v1/values")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "v1")]
    [ApiController]
    public class RawController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RawController> _logger;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="cache">IDistributedCache</param>
        /// <param name="logger">ILogger</param>
        public RawController(IDistributedCache cache, ILogger<RawController> logger)
        {
            this._cache = cache;
            this._logger = logger;
        }

        /// <summary>
        /// Stash a name, value pair
        /// </summary>
        /// <param name="name">(sic)</param>
        /// <param name="value">(sic)</param>
        [HttpPost("Stash")]
        [ProducesResponseType(typeof(void), 201)]
        [ProducesResponseType(typeof(Models.ErrorPayload), 400)]
        public void Stash(string name, string value)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var msg = $"Stash({name}, {value})";
            
            this._logger.LogInformation(msg);

            this._cache.SetString(name, value);
            
            this.Response.StatusCode = (int)HttpStatusCode.Created;
        }

        /// <summary>
        /// Fetch value from cache
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Value</returns>
        [HttpGet("Fetch")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Models.ErrorPayload), 400)]
        [ProducesResponseType(typeof(Models.ErrorPayload), 404)]
        public string Fetch(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var value = this._cache.GetString(name);
            
            var msg = $"{value} = Fetch({name})";
            
            this._logger.LogInformation(msg);
            
            if(string.IsNullOrWhiteSpace(value))
            {
                throw new KeyNotFoundException(msg);
            }

            return value;
        }

        /// <summary>
        /// Delete a key
        /// </summary>
        /// <param name="name">Name</param>
        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(Models.ErrorPayload), 400)]
        [ProducesResponseType(typeof(Models.ErrorPayload), 404)]
        public void Delete(string name)
        {
            this._cache.Remove(name);
            var msg = $"Delete({name})";
            this._logger.LogInformation(msg);
        }
        
    }
}
