using System.Configuration;

namespace RethinkDb.AppConfig
{
    internal class RethinkDbClientSection : ConfigurationSection
    {
        [ConfigurationProperty("clusters", IsRequired = true)]
        public ClusterElementCollection Clusters
        {
            get { return (ClusterElementCollection)base["clusters"]; }
        }
    }
}
