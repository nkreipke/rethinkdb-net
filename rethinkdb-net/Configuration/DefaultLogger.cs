using RethinkDb.Logging;

namespace RethinkDb.Configuration
{
    public class DefaultLogger : IValidatable
    {
        public bool Enabled { get; set; }

        public LoggingCategory Category { get; set; }

        public void Validate()
        {
            this.ValidateWithAnnotations();
        }
    }
}
