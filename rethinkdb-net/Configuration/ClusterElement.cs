using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RethinkDb.Configuration
{
    public class ClusterElement
    {
        [Required]
        public string Name { get; set; }

        public string AuthorizationKey { get; set; }

        public List<EndPointElement> EndPoints { get; set; }

        public ConnectionPoolElement ConnectionPool { get; set; }

        public NetworkErrorHandlingElement NetworkErrorHandling { get; set; }

        public DefaultLoggerElement DefaultLogger { get; set; }
    }
}
