using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using RethinkDb.Configuration;
using RethinkDb.Logging;

namespace RethinkDb.ConnectionFactories
{
    public class ConnectionFactoryBuilder
    {
        public RethinkDbClientSection Configuration { get; set; }

        public Func<IEnumerable<EndPoint>, string, ILogger, IConnectionFactory> BaseFactory { get; set; } = (endpoints, authorizationKey, logger) =>
        {
            return new DefaultConnectionFactory(endpoints, authorizationKey, logger);
        };

        /// <summary>
        /// Creates a connection factory.
        /// </summary>
        /// <param name="clusterName">The name of the cluster to use. This cluster must be present in the configuration.</param>
        public IConnectionFactory Build(string clusterName)
        {
            if (Configuration == null)
                throw new InvalidOperationException("Please load a configuration first.");

            try
            {
                return Build(Configuration.Clusters.First(c => c.Name == clusterName));
            }
            catch (InvalidOperationException)
            {
                throw new ArgumentException("Cluster name could not be found in configuration", "clusterName");
            }
        }

        /// <summary>
        /// Creates a connection factory from a <see cref="RethinkDb.Configuration.ClusterElement" /> object.
        /// </summary>
        /// <param name="cluster">The cluster configuration.</param>
        public IConnectionFactory Build(ClusterElement cluster)
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

            var authorizationKey = !string.IsNullOrEmpty(cluster.AuthorizationKey) ? 
                                   cluster.AuthorizationKey : null;
            var logger = cluster.DefaultLogger != null && cluster.DefaultLogger.Enabled ?
                         new DefaultLogger(cluster.DefaultLogger.Category, Console.Out) : null;  

            var connectionFactory = BaseFactory(endpoints, authorizationKey, logger);

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
}
