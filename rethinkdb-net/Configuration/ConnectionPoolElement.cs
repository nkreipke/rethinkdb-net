namespace RethinkDb.Configuration
{
    public class ConnectionPoolElement
    {
        public bool Enabled { get; set; }

        public int QueryTimeout { get; set; }
    }
}
