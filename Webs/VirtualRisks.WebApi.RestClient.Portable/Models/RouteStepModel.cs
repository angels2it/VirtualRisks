// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RouteStepModel
    {
        /// <summary>
        /// Initializes a new instance of the RouteStepModel class.
        /// </summary>
        public RouteStepModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RouteStepModel class.
        /// </summary>
        public RouteStepModel(PositionModel startLocation = default(PositionModel), PositionModel endLocation = default(PositionModel), double? distance = default(double?), string duration = default(string))
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
            Distance = distance;
            Duration = duration;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "startLocation")]
        public PositionModel StartLocation { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "endLocation")]
        public PositionModel EndLocation { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "distance")]
        public double? Distance { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }

    }
}