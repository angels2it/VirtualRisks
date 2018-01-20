using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System;
using System.Web.Http;

namespace CastleGo.WebApi
{
    /// <summary>SwaggerConfig</summary>
    public static class SwaggerConfig
    {
        /// <summary>Register</summary>
        public static HttpConfiguration UsingSwagger(this HttpConfiguration configuration)
        {
            configuration.EnableSwagger(c =>
            {
                c.SingleApiVersion("v1", "VirtualRisks API");
                c.IncludeXmlComments($"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\CastleGo.WebApi.XML");
                c.DescribeAllEnumsAsStrings();
                c.ApiKey("Token").Description("Filling bearer token here").Name("Authorization").In("header");
            }).EnableSwaggerUi(c => c.EnableApiKeySupport("Authorization", "header"));
            return configuration;
        }
    }
}
