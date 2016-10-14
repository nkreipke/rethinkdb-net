using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RethinkDb.Configuration
{
    public class RethinkDbConfiguration : IValidatable
    {
        [Required]
        public List<Cluster> Clusters { get; set; }

        public void Validate()
        {
            this.ValidateWithAnnotations();

            Clusters.Validate();
        }
    }
}
