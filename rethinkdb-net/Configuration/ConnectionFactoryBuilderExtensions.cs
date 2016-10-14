using System;
using Microsoft.Extensions.Configuration;
using RethinkDb.ConnectionFactories;

namespace RethinkDb.Configuration
{
    public static class ConfigurationFactoryBuilderExtensions
    {
        /// <summary>
        /// Loads the configuration.
        /// </summary>
        /// <param name="configuration">The configuration to load.</param>
        public static ConnectionFactoryBuilder FromConfiguration(this ConnectionFactoryBuilder builder, IConfiguration configuration)
        {
            builder.Configuration = new RethinkDbConfiguration();
            configuration.Bind(builder.Configuration);
            return builder;
        }
    }
}

