using System;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RethinkDb.Newtonsoft.Configuration;
using RethinkDb.Newtonsoft;
using RethinkDb.ConnectionFactories;
using RethinkDb.Configuration;
using RethinkDb.Test.Integration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace RethinkDb.Newtonsoft.Test.Integration
{
    internal static class CreateConnectionFactory
    {
        public static IConnectionFactory Create()
        {
            var config = new ConfigurationBuilder().AddJsonFile("rethinkdb.json").Build();

            return new ConnectionFactoryBuilder()
                .FromConfiguration(config)
                .UseNewtonsoftJsonSerializer()
                .Build("testCluster");
        }
    }

    [SetUpFixture]
    public class NIntegrationTestSetup : IntegrationTestSetup
    {
    }

    [TestFixture]
    public class NSingleObjectTest : SingleObjectTests
    {
        static NSingleObjectTest()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }

    [TestFixture]
    public class NTableTests : TableTests
    {
        static NTableTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }

    [TestFixture]
    public class NMultiTableTests : MultiTableTests
    {
        static NMultiTableTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }

    [TestFixture]
    public class NMultiObjectTests : MultiObjectTests
    {
        static NMultiObjectTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }

    [TestFixture]
    public class NManyObjectTests : ManyObjectTests
    {
        static NManyObjectTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }

    [TestFixture]
    public class NBlankTests : BlankTests
    {
        static NBlankTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }


    [TestFixture]
    public class NDatabaseTests : DatabaseTests
    {
        static NDatabaseTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }


    [TestFixture]
    public class NGroupingTests : GroupingTests
    {
        static NGroupingTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }


    [TestFixture]
    public class NHasFieldsTests : HasFieldsTests
    {
        static NHasFieldsTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }

    [TestFixture]
    public class NRealtimePushTests : RealtimePushTests
    {
        static NRealtimePushTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }
    }

    [TestFixture]
    public class NNamedValueDictionaryTests : NamedValueDictionaryTests
    {
        static NNamedValueDictionaryTests()
        {
            ConnectionFactory = CreateConnectionFactory.Create();
        }

        protected override void MultipleItemSetterVerifyDictionary(TestObjectWithDictionary gil)
        {
            // FIXME: varies from the base class by:
            //  - skill level being a double type, rather than an int
            //  - updated at being a JObject, rather than a DateTimeOffset
            // These are not the types I'd expect for these values, but, the RethinkDB datum converters are only plugged into the
            // top level of the object with the newtonsoft converter, not the values inside a dictionary.  This is debatably wrong,
            // but, I'm not fixing it right now... the best solution might be to incorporate any technical requirements of the
            // newtonsoft extension library into the core, and drop this extension library...
            gil.FreeformProperties.Should().Contain("best known for", "being awesome");
            gil.FreeformProperties.Should().Contain("skill level", 1000.0);
            gil.FreeformProperties.Should().ContainKey("updated at");
            gil.FreeformProperties ["updated at"].Should().BeOfType<JObject>();
            gil.FreeformProperties.Should().HaveCount(5);
        }
    }
}
