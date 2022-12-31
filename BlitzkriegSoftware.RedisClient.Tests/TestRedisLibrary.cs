using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BlitzkriegSoftware.RedisClient;
using BlitzkriegSoftware.RedisClient.Models;

namespace BlitzkriegSoftware.RedisClient.Tests
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

        private Tests.Models.FakeRedisKeyValue MakeKeyValue(int index)
        {
            var model =  new Tests.Models.FakeRedisKeyValue()
            {
                Key = index.ToString(),
                Value = Faker.Lorem.Sentence()
            };
            TestRedisLibrary.testContext.WriteLine($"{model}");
            return model;
        }

        #endregion

        [TestMethod]
        [TestCategory("Unit")]
        public void ConfigTest()
        {
            var c = new BlitzkriegSoftware.RedisClient.Models.RedisConfiguration();
            Assert.IsTrue(c.IsValid);
            var s = c.ConnectionString;
            Assert.IsFalse(string.IsNullOrWhiteSpace(s));
            var t = c.ToString();
            Assert.IsFalse(string.IsNullOrWhiteSpace(t));
            c.ConnectionString = RedisConfiguration.RedisLocalHostDefault;
            c.SetProperty("redis-connection", RedisConfiguration.RedisLocalHostDefault);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void BadPropSet()
        {
            var c = new RedisConfiguration();
            var cs = c.ConnectionString;
            Assert.IsFalse(string.IsNullOrWhiteSpace(cs));
            TestRedisLibrary.testContext.WriteLine($"{c}");
            c.SetProperty(null, null);
        }


        [TestMethod]
        [TestCategory("Integration")]
        public void TestSimpleString()
        {
            var redisConfig = new RedisConfiguration();
            var client = new BlitzRedisClient(redisConfig);

            var cfg = client.Config;
            Assert.IsFalse(string.IsNullOrWhiteSpace(cfg.ConnectionString));

            var m = this.MakeKeyValue(1);

            client.Set(m.Key, m.Value);

            var v = client.Get(m.Key);

            Assert.AreEqual(m.Value, v);
        }

        [TestMethod]
        [TestCategory("Integration")]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void BadClient()
        {
            _ = new BlitzRedisClient(null);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestTypeT()
        {
            var redisConfig = new RedisConfiguration();
            var client = new BlitzRedisClient(redisConfig);

            var m = this.MakeKeyValue(2);

            client.Set<Models.FakeRedisKeyValue>(m.Key, m);

            var v = client.Get<Models.FakeRedisKeyValue>(m.Key);

            Assert.AreEqual(m.Value, v.Value);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void TestDelete()
        {
            var redisConfig = new RedisConfiguration();
            var client = new BlitzRedisClient(redisConfig);
            var m = this.MakeKeyValue(2);
            client.Set<Models.FakeRedisKeyValue>(m.Key, m);
            var deleted = client.Delete(m.Key);
            Assert.IsTrue(deleted);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void ComplexTest()
        {
            var redisConfig = new RedisConfiguration();
            var client = new BlitzRedisClient(redisConfig);

            client.ClearAll();

            var list = new Dictionary<string, string> {
                { "A01", "Tree"  },
                { "A02", "Cat" },
                { "A03", "Dog" },
                { "B01", "Red" },
                { "B02", "Green" },
                { "B03", "Blue" },
                { "B04", "Yellow" }
            };

            foreach (var key in list.Keys)
            {
                client.Set(key, list[key]);
            }

            string searchExp = "A*";

            var matches = client.MatchedKeyValues(searchExp);

            Assert.AreEqual(3, matches.Count);

            searchExp = "B*";
            var keys = client.MatchedKeys(searchExp);
            Assert.AreEqual(4, keys.Length);

            searchExp = "C*";
            keys = client.MatchedKeys(searchExp);
            Assert.AreEqual(0, keys.Length);

            searchExp = "*01";
            var deleted = client.ClearMatching(searchExp);
            Assert.AreEqual(2, deleted);
        }


        [TestMethod]
        public void NullTest()
        {
            var redisConfig = new RedisConfiguration();
            var client = new BlitzRedisClient(redisConfig);

            var actual = client.Multiplexer;
            Assert.IsNotNull(actual);
        }

        [TestMethod]
        public void DisposeTest() {
            var redisConfig = new RedisConfiguration();
#pragma warning disable IDE0063 // Use simple 'using' statement
            using (var client = new BlitzRedisClient(redisConfig))
            {
                client.Dispose();
            }
#pragma warning restore IDE0063 // Use simple 'using' statement

        }
    }
}
