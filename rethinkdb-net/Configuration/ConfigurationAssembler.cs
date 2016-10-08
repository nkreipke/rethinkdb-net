using System;

namespace RethinkDb.Configuration
{
    public class ConfigurationAssembler
    {
        [Obsolete("Use ConnectionFactoryBuilder instead. If you previously used an App.config or Web.config based configuration, " +
                  "you may want to install rethinkdb-net-appconfig and use \"new ConfigurationBuilder().FromAppConfig().Build(clusterName)\".")]
        public static IConnectionFactory CreateConnectionFactory(string clusterName)
        {
            throw new NotSupportedException("Sorry, this call is unsupported:\n" +
                "Use ConnectionFactoryBuilder instead. If you previously used an App.config or Web.config based configuration, " +
                "you may want to install rethinkdb-net-appconfig and use \"new ConfigurationBuilder().FromAppConfig().Build(clusterName)\".");
        }
    }
}
