using Newtonsoft.Json.Serialization;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace CastleGo.WebApi
{
    /// <summary>Represents formatter configuration.</summary>
    public static class FormatterConfig
    {
        /// <summary>
        /// Configures formatter to use JSON and removes XML formatter.
        /// </summary>
        /// <param name="configuration"></param>
        public static HttpConfiguration UsingFormatter(this HttpConfiguration configuration)
        {
            configuration.Formatters.Remove(configuration.Formatters.XmlFormatter);
            configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add
              (new Newtonsoft.Json.Converters.StringEnumConverter());
            configuration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            configuration.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;
            return configuration;
        }
    }
}
