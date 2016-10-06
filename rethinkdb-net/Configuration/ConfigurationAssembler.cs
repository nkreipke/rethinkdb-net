using System;
using System.Net;
using System.Collections.Generic;
using RethinkDb.Logging;
using RethinkDb.ConnectionFactories;

#if NETSTANDARD
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
#else
using System.Configuration;
#endif

namespace RethinkDb.Configuration
{
    public class ConfigurationAssembler
    {
        internal static readonly Lazy<RethinkDbClientSection> DefaultSettings = new Lazy<RethinkDbClientSection>(() => GetDefaultSettings());

        public static IConnectionFactory CreateConnectionFactory(string clusterName)
        {
            foreach (ClusterElement cluster in DefaultSettings.Value.Clusters)
            {
                if (cluster.Name == clusterName)
                {
                    IConnectionFactory connectionFactory = CreateDefaultConnectionFactory(cluster);

                    if (cluster.NetworkErrorHandling != null && cluster.NetworkErrorHandling.Enabled)
                        connectionFactory = new ReliableConnectionFactory(connectionFactory);

                    if (cluster.ConnectionPool != null && cluster.ConnectionPool.Enabled)
                    {
                        if (cluster.ConnectionPool.QueryTimeout != 0)
                            connectionFactory = new ConnectionPoolingConnectionFactory(connectionFactory, TimeSpan.FromSeconds(cluster.ConnectionPool.QueryTimeout));
                        else
                            connectionFactory = new ConnectionPoolingConnectionFactory(connectionFactory);
                    }

                    return connectionFactory;
                }
            }

            throw new ArgumentException("Cluster name could not be found in configuration", "clusterName");
        }

        private static IConnectionFactory CreateDefaultConnectionFactory(ClusterElement cluster)
        {
            List<EndPoint> endpoints = new List<EndPoint>();
            foreach (EndPointElement ep in cluster.EndPoints)
            {
                IPAddress ip;
                if (IPAddress.TryParse(ep.Address, out ip))
                    endpoints.Add(new IPEndPoint(ip, ep.Port));
                else
                    endpoints.Add(new DnsEndPoint(ep.Address, ep.Port));
            }

            var connectionFactory = new DefaultConnectionFactory(endpoints);

            if (!String.IsNullOrEmpty(cluster.AuthorizationKey))
                connectionFactory.AuthorizationKey = cluster.AuthorizationKey;

            if (cluster.DefaultLogger != null && cluster.DefaultLogger.Enabled)
                connectionFactory.Logger = new DefaultLogger(cluster.DefaultLogger.Category, Console.Out);

            return connectionFactory;
        }

#if NETSTANDARD
        private static RethinkDbClientSection GetDefaultSettings()
        {
            var configuration = new RethinkDbClientSection();

            new ConfigurationBuilder().AddJsonFile("rethinkdb.json").Build().Bind(configuration);
            Validator.ValidateObject(configuration, new ValidationContext(configuration));

            return configuration;
        }
#else
        private static RethinkDbClientSection GetDefaultSettings()
        {
            var configuration = ConfigurationManager.GetSection("rethinkdb") as RethinkDbClientSection;
            if (configuration == null)
                throw new ConfigurationErrorsException("No rethinkdb client configuration section located");

            return configuration;
        }
#endif
    }
}

