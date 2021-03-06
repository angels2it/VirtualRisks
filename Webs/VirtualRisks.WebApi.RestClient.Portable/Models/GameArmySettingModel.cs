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

    public partial class GameArmySettingModel
    {
        /// <summary>
        /// Initializes a new instance of the GameArmySettingModel class.
        /// </summary>
        public GameArmySettingModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GameArmySettingModel class.
        /// </summary>
        public GameArmySettingModel(string name = default(string), IList<GameCastleSettingModel> castles = default(IList<GameCastleSettingModel>), string id = default(string))
        {
            Name = name;
            Castles = castles;
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
        [JsonProperty(PropertyName = "castles")]
        public IList<GameCastleSettingModel> Castles { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

    }
}
