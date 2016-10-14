namespace RethinkDb.Configuration
{
    public class ConnectionPool : IValidatable
    {
        public bool Enabled { get; set; }

        public int QueryTimeout { get; set; }

        public void Validate()
        {
            this.ValidateWithAnnotations();
        }
    }
}
