// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class MoveSoldierModel
    {
        /// <summary>
        /// Initializes a new instance of the MoveSoldierModel class.
        /// </summary>
        public MoveSoldierModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the MoveSoldierModel class.
        /// </summary>
        public MoveSoldierModel(string castleId = default(string), IList<string> soldiers = default(IList<string>))
        {
            CastleId = castleId;
            Soldiers = soldiers;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "castleId")]
        public string CastleId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "soldiers")]
        public IList<string> Soldiers { get; set; }

    }
}
