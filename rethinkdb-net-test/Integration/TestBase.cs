using System;
using NUnit.Framework;
using RethinkDb;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using RethinkDb.Configuration;
using RethinkDb.ConnectionFactories;
using RethinkDb.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace RethinkDb.Test.Integration
{
    public class TestBase
    {
        public static IConnectionFactory ConnectionFactory;

        static TestBase()
        {
            var config = new ConfigurationBuilder().AddJsonFile("rethinkdb.json").Build();

            ConnectionFactory = new ConnectionFactoryBuilder().FromConfiguration(config).Build("testCluster");
        }

        protected IConnection connection;

        [OneTimeSetUp]
        public virtual void TestFixtureSetUp()
        {
            try
            {
                DoTestFixtureSetUp().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine("TestFixtureSetUp failed: {0}", e);
                throw;
            }
        }

        private async Task DoTestFixtureSetUp()
        {
            connection = await ConnectionFactory.GetAsync();

            try
            {
                var dbList = await connection.RunAsync(Query.DbList());
                if (dbList.Contains("test"))
                    await connection.RunAsync(Query.DbDrop("test"));
            }
            catch (Exception)
            {
            }
        }

        [OneTimeTearDown]
        public virtual void TestFixtureTearDown()
        {
            connection.Dispose();
            connection = null;
        }
    }
}
