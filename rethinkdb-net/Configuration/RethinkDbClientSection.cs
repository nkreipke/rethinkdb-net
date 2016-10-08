using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RethinkDb.Configuration
{
    public class RethinkDbClientSection
    {
        [Required]
        public List<ClusterElement> Clusters { get; set; }
    }
}
