namespace RethinkDb.Configuration
{
    public class NetworkErrorHandling : IValidatable
    {
        public bool Enabled { get; set; }

        public void Validate()
        {
            this.ValidateWithAnnotations();
        }
    }
}
