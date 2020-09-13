using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Blitz.Redis.WebDemo.Libs
{
    /// <summary>
    /// Global Exception Filter
    /// <para>See: www.talkingdotnet.com/global-exception-handling-in-aspnet-core-webapi/</para>
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        /// <summary>
        /// Field: ILogger
        /// </summary>
        private readonly ILogger _logger = null;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="logger">Logger to inject</param>
        public GlobalExceptionFilter(ILoggerFactory logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this._logger = logger.CreateLogger("Global Exception Filter");
        }

        #region "Dispose"
        // Flag: Has Dispose already been called?
        bool disposed = false;

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implementation of Dispose pattern.
        /// </summary>
        /// <param name="disposing">bool</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
            {
                // dispose stuff as needed
            }

            this.disposed = true;
        }

        #endregion

        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var data = new Dictionary<string, string>();
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            var message = String.Empty;

            if (context == null)
            {
                return;
            }

            var ex = context.Exception;

            TypeSwitch.Do(ex,
                    TypeSwitch.Case<ArgumentException>(() => { statusCode = HttpStatusCode.BadRequest; }),
                    TypeSwitch.Case<ArgumentNullException>(() => { statusCode = HttpStatusCode.BadRequest; }),
                    TypeSwitch.Case<ArgumentOutOfRangeException>(() => { statusCode = HttpStatusCode.BadRequest; }),
                    TypeSwitch.Case<KeyNotFoundException>(() => { statusCode = HttpStatusCode.NotFound; }),
                    TypeSwitch.Case<StackExchange.Redis.RedisConnectionException>(() => { statusCode = HttpStatusCode.InternalServerError; })
                );

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";

            var err = new Models.ErrorPayload()
            {
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                StatusCode = (int)statusCode
            };

            foreach (var d in data) err.Data.Add(d.Key, d.Value);

            var json = JsonConvert.SerializeObject(err);

            this._logger.LogError(ex, json);

            response.WriteAsync(json);
        }
    }
}
