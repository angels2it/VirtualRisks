// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CastleModel
    {
        /// <summary>
        /// Initializes a new instance of the CastleModel class.
        /// </summary>
        public CastleModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CastleModel class.
        /// </summary>
        public CastleModel(LocationModel position = default(LocationModel), int? index = default(int?), int? routeCount = default(int?))
        {
            Position = position;
            Index = index;
            RouteCount = routeCount;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Position")]
        public LocationModel Position { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Index")]
        public int? Index { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "RouteCount")]
        public int? RouteCount { get; set; }

    }
}
