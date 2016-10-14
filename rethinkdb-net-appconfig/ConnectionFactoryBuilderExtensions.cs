using System;
using System.Configuration;
using RethinkDb.Configuration;
using RethinkDb.ConnectionFactories;

using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace RethinkDb.AppConfig
{
    public static class ConfigurationFactoryBuilderExtensions
    {
        private static readonly Lazy<MapperConfiguration> MapperConfig = new Lazy<MapperConfiguration>(() => LoadMapperConfiguration());

        /// <summary>
        /// Loads the application configuration.
        /// </summary>
        public static ConnectionFactoryBuilder FromAppConfiguration(this ConnectionFactoryBuilder builder)
        {
            var configuration = ConfigurationManager.GetSection("rethinkdb") as RethinkDbClientSection;
            if (configuration == null)
                throw new ConfigurationErrorsException("No rethinkdb client configuration section located");
            
            return new ConnectionFactoryBuilder
            {
                Configuration = MapperConfig.Value.CreateMapper().Map<RethinkDbConfiguration>(configuration)
            };
        }

        private static MapperConfiguration LoadMapperConfiguration()
        {
            return new MapperConfiguration(cfg => {
                cfg.CreateMap<ClusterElement, Cluster>();
                cfg.CreateMap<ClusterElementCollection, List<ClusterElement>>().ConvertUsing(x => x.Cast<ClusterElement>().ToList());
                cfg.CreateMap<ConnectionPoolElement, ConnectionPool>();
                cfg.CreateMap<DefaultLoggerElement, DefaultLogger>();
                cfg.CreateMap<EndPointElement, EndPoint>();
                cfg.CreateMap<EndPointElementCollection, List<EndPointElement>>().ConvertUsing(x => x.Cast<EndPointElement>().ToList());
                cfg.CreateMap<NetworkErrorHandlingElement, NetworkErrorHandling>();
                cfg.CreateMap<RethinkDbClientSection, RethinkDbConfiguration>();
            });
        }
    }
}

