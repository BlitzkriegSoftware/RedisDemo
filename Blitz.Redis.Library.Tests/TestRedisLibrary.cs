using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Blitz.Redis.Library.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class TestRedisLibrary
    {
        #region "Boilerplate"

        private static TestContext testContext;

        [ClassInitialize]
        public static void InitClass(TestContext testContext)
        {
            TestRedisLibrary.testContext = testContext;
        }

        #endregion

        #region "Helpers"

        private Models.FakeRedisKeyValue MakeKeyValue(int index)
        {
            return new Models.FakeRedisKeyValue()
            {
                Key = index.ToString(),
                Value = Faker.Lorem.Sentence()
            };
        }

        #endregion

        [TestMethod]
        [TestCategory("Unit")]
        public void ConfigTest()
        {
            var c = new Library.Models.RedisConfiguration();
            Assert.IsTrue(c.IsValid);
            var s = c.ConnectionString;
            Assert.IsFalse(string.IsNullOrWhiteSpace(s));
            var t = c.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(t));
            c.ConnectionString = Library.Models.RedisConfiguration.RedisLocalHostDefault;
            c.SetProperty("redis-connection", Library.Models.RedisConfiguration.RedisLocalHostDefault);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestSimpleString()
        {
            var redisConfig = new Blitz.Redis.Library.Models.RedisConfiguration();
            var client = new Blitz.Redis.Library.BlitzRedisClient(redisConfig);

            var cfg = client.Config;
            Assert.IsFalse(string.IsNullOrWhiteSpace(cfg.ConnectionString));

            var m = MakeKeyValue(1);

            client.Set(m.Key, m.Value);

            var v = client.Get(m.Key);

            Assert.AreEqual(m.Value, v);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestTypeT()
        {
            var redisConfig = new Blitz.Redis.Library.Models.RedisConfiguration();
            var client = new Blitz.Redis.Library.BlitzRedisClient(redisConfig);

            var m = MakeKeyValue(2);

            client.Set<Models.FakeRedisKeyValue>(m.Key, m);

            var v = client.Get<Models.FakeRedisKeyValue>(m.Key);

            Assert.AreEqual(m.Value, v.Value);
        }

    }
}
