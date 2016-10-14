using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RethinkDb.Configuration
{
    public class Cluster : IValidatable
    {
        [Required]
        public string Name { get; set; }

        public string AuthorizationKey { get; set; }

        public List<EndPoint> EndPoints { get; set; }

        public ConnectionPool ConnectionPool { get; set; }

        public NetworkErrorHandling NetworkErrorHandling { get; set; }

        public DefaultLogger DefaultLogger { get; set; }

        public void Validate()
        {
            this.ValidateWithAnnotations();
            
            EndPoints.Validate();
            ConnectionPool.Validate();
            NetworkErrorHandling.Validate();
            DefaultLogger.Validate();
        }
    }
}
