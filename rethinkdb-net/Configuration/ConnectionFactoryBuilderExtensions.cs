using System;
using System.ComponentModel.DataAnnotations;
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
            builder.Configuration = new RethinkDbClientSection();

            configuration.Bind(builder.Configuration);
            Validator.ValidateObject(builder.Configuration, new ValidationContext(builder.Configuration));

            return builder;
        }
    }
}

