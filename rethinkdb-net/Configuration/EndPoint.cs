using System.ComponentModel.DataAnnotations;

namespace RethinkDb.Configuration
{
    public class EndPoint : IValidatable
    {
        [Required]
        public string Address { get; set; }

        [Range(0, 65535)]
        public int Port { get; set; }

        public void Validate()
        {
            this.ValidateWithAnnotations();
        }
    }
}
