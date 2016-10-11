using RethinkDb.ConnectionFactories;
using RethinkDb.Newtonsoft.Configuration;

namespace RethinkDb.Newtonsoft
{
    public static class ConnectionFactoryBuilderExtensions
    {
        /// <summary>
        /// Enables the alternative Newtonsoft.Json object serializer.
        /// </summary>
        public static ConnectionFactoryBuilder UseNewtonsoftJsonSerializer(this ConnectionFactoryBuilder builder)
        {
            builder.BaseFactory = (endpoints, authorizationKey, logger) => 
            {
                return new NewtonsoftConnectionFactory(endpoints, authorizationKey, logger);
            };

            return builder;
        }
    }
}