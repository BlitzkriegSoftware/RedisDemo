using System.Collections.Generic;

namespace Blitz.Redis.WebDemo.Models
{
    /// <summary>
    /// Error Payload
    /// </summary>
    public class ErrorPayload
    {
        /// <summary>
        /// HTTP Status Code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Stack Trace
        /// </summary>
        public string StackTrace { get; set; }

        private Dictionary<string, string> _data;

        /// <summary>
        /// Additional Data
        /// </summary>
        public Dictionary<string, string> Data
        {
            get
            {
                if (this._data == null)
                {
                    this._data = new Dictionary<string, string>();
                }
                return this._data;
            }
        }

    }
}
