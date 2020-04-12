using Blitz.Redis.Demo.Models;
using Microsoft.Extensions.Logging;

namespace Blitz.Redis.Demo.Workers
{
    public class RedisWorker : IRedisWorker
    {
        private readonly ILogger _logger;

        public RedisWorker(ILogger<RedisWorker> logger)
        {
            _logger = logger;
        }

        public void Run(CommandOptions o)
        {
            if(o == null) throw new System.ArgumentNullException(nameof(o));

            var redisConfig = new Blitz.Redis.Library.Models.RedisConfiguration() { ConnectionString = o.RedisConnectionString };

            var client = new Blitz.Redis.Library.BlitzRedisClient(redisConfig);

            for(int i=0; i<o.MessageCount; i++)
            {
                var m = MakeKeyValue(i);
                _logger.LogInformation(m.ToString());
                
                client.Set(m.Key, m.Value);

                var n = new Models.FakeRedisKeyValue()
                {
                    Key = m.Key,
                    Value = client.Get(m.Key)
                };

                if (!m.Equals(n)) _logger.LogWarning($"No Match M to N");
            }

        }

        private Models.FakeRedisKeyValue MakeKeyValue(int index)
        {
            return new Models.FakeRedisKeyValue()
            {
                Key = index.ToString(),
                Value = Faker.Lorem.Sentence()
            };
        }


    }
}
