using RethinkDb.Logging;

namespace RethinkDb.Configuration
{
    public class DefaultLoggerElement
    {
        public bool Enabled { get; set; }

        public LoggingCategory Category { get; set; }
    }
}
