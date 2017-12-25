// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CreateGameModel
    {
        /// <summary>
        /// Initializes a new instance of the CreateGameModel class.
        /// </summary>
        public CreateGameModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CreateGameModel class.
        /// </summary>
        /// <param name="speed">Possible values include: 'UltraFast', 'Fast',
        /// 'Speedy', 'Normal', 'Slow', 'UltraSlow'</param>
        /// <param name="difficulty">Possible values include: 'Easy', 'Normal',
        /// 'Hard'</param>
        public CreateGameModel(double? lat = default(double?), double? lng = default(double?), string opponentId = default(string), OpponentExtInfoModel opponentExtInfo = default(OpponentExtInfoModel), bool? selfPlaying = default(bool?), string speed = default(string), string difficulty = default(string), GameArmySettingModel userArmySetting = default(GameArmySettingModel))
        {
            Lat = lat;
            Lng = lng;
            OpponentId = opponentId;
            OpponentExtInfo = opponentExtInfo;
            SelfPlaying = selfPlaying;
            Speed = speed;
            Difficulty = difficulty;
            UserArmySetting = userArmySetting;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "lat")]
        public double? Lat { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "lng")]
        public double? Lng { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "opponentId")]
        public string OpponentId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "opponentExtInfo")]
        public OpponentExtInfoModel OpponentExtInfo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "selfPlaying")]
        public bool? SelfPlaying { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'UltraFast', 'Fast',
        /// 'Speedy', 'Normal', 'Slow', 'UltraSlow'
        /// </summary>
        [JsonProperty(PropertyName = "speed")]
        public string Speed { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Easy', 'Normal', 'Hard'
        /// </summary>
        [JsonProperty(PropertyName = "difficulty")]
        public string Difficulty { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "userArmySetting")]
        public GameArmySettingModel UserArmySetting { get; set; }

    }
}
