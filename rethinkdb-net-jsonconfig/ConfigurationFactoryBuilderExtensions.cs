using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using RethinkDb.ConnectionFactories;
using RethinkDb.Configuration;

namespace RethinkDb.JsonConfig
{
    public static class ConfigurationFactoryBuilderExtensions
    {
        /// <summary>
        /// Loads the configuration from a JSON file.
        /// </summary>
        /// <param name="fileName">The name of the file to load. This is "rethinkdb.json" by default.</param>
        public static ConnectionFactoryBuilder FromJsonConfiguration(this ConnectionFactoryBuilder builder, string fileName = "rethinkdb.json")
        {
            return builder.FromConfiguration(new ConfigurationBuilder().AddJsonFile(fileName).Build());
        }
    }
}

