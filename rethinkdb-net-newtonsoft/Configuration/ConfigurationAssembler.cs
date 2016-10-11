using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RethinkDb.Newtonsoft.Converters;

namespace RethinkDb.Newtonsoft.Configuration
{
    public class ConfigurationAssembler
    {
        public static JsonSerializerSettings DefaultJsonSerializerSettings { get; set; }

        static ConfigurationAssembler()
        {
            DefaultJsonSerializerSettings = new JsonSerializerSettings()
                {
                    Converters =
                        {
                            new TimeSpanConverter()
                        },
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
        }

        [Obsolete("Use ConnectionFactoryBuilder instead. If you previously used an App.config or Web.config based configuration, " +
                  "you may want to install rethinkdb-net-appconfig and use \"new ConfigurationBuilder().FromAppConfig().UseNewtonsoftJsonSerializer().Build(clusterName)\".")]
        public static IConnectionFactory CreateConnectionFactory(string clusterName)
        {
            throw new NotSupportedException("Sorry, this call is unsupported:\n" +
                "Use ConnectionFactoryBuilder instead. If you previously used an App.config or Web.config based configuration, " +
                "you may want to install rethinkdb-net-appconfig and use \"new ConfigurationBuilder().FromAppConfig().UseNewtonsoftJsonSerializer().Build(clusterName)\".");
        }
    }
}