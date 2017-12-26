// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class GameCastleSettingModel
    {
        /// <summary>
        /// Initializes a new instance of the GameCastleSettingModel class.
        /// </summary>
        public GameCastleSettingModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GameCastleSettingModel class.
        /// </summary>
        public GameCastleSettingModel(string name = default(string), string id = default(string))
        {
            Name = name;
            Id = id;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

    }
}