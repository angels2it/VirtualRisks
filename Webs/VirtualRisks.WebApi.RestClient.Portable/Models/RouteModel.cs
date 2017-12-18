// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RouteModel
    {
        /// <summary>
        /// Initializes a new instance of the RouteModel class.
        /// </summary>
        public RouteModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RouteModel class.
        /// </summary>
        public RouteModel(CastleModel fromCastle = default(CastleModel), CastleModel toCastle = default(CastleModel))
        {
            FromCastle = fromCastle;
            ToCastle = toCastle;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "FromCastle")]
        public CastleModel FromCastle { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ToCastle")]
        public CastleModel ToCastle { get; set; }

    }
}
