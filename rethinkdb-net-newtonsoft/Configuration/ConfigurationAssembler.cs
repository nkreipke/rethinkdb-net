using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RethinkDb.Newtonsoft.Converters;

namespace RethinkDb.Newtonsoft.Configuration
{
    public class ConfigurationAssembler
    {
        public static JsonSerializerSettings DefaultJsonSerializerSettings { get; set; }

        static ConfigurationAssembler()
        {
            DefaultJsonSerializerSettings = new JsonSerializerSettings()
                {
                    Converters =
                        {
                            new TimeSpanConverter()
                        },
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
        }
    }
}