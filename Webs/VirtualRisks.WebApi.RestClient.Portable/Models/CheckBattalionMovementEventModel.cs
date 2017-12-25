// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CheckBattalionMovementEventModel
    {
        /// <summary>
        /// Initializes a new instance of the CheckBattalionMovementEventModel
        /// class.
        /// </summary>
        public CheckBattalionMovementEventModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CheckBattalionMovementEventModel
        /// class.
        /// </summary>
        public CheckBattalionMovementEventModel(System.Guid? gameId = default(System.Guid?), System.Guid? eventId = default(System.Guid?), int? streamVersion = default(int?))
        {
            GameId = gameId;
            EventId = eventId;
            StreamVersion = streamVersion;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "gameId")]
        public System.Guid? GameId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "eventId")]
        public System.Guid? EventId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "streamVersion")]
        public int? StreamVersion { get; set; }

    }
}
