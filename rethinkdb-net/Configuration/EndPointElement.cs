using System.ComponentModel.DataAnnotations;

namespace RethinkDb.Configuration
{
    public class EndPointElement
    {
        [Required]
        public string Address { get; set; }

        [Range(0, 65535)]
        public int Port { get; set; }
    }
}
