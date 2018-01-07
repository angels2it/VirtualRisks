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

    public partial class GameModel
    {
        /// <summary>
        /// Initializes a new instance of the GameModel class.
        /// </summary>
        public GameModel()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GameModel class.
        /// </summary>
        /// <param name="status">Possible values include: 'Requesting',
        /// 'Playing', 'Rejected', 'Ended'</param>
        /// <param name="speed">Possible values include: 'UltraFast', 'Fast',
        /// 'Speedy', 'Normal', 'Slow', 'UltraSlow'</param>
        /// <param name="difficulty">Possible values include: 'Easy', 'Normal',
        /// 'Hard'</param>
        public GameModel(System.DateTime? createdAt = default(System.DateTime?), string createdBy = default(string), string opponentId = default(string), string userHeroId = default(string), string opponentHeroId = default(string), string status = default(string), IList<string> castles = default(IList<string>), PositionModel position = default(PositionModel), UserModel user = default(UserModel), HeroModel userHero = default(HeroModel), UserModel opponent = default(UserModel), HeroModel opponentHero = default(HeroModel), int? redCastleAmount = default(int?), int? blueCastleAmount = default(int?), int? neutrualCastleAmount = default(int?), OpponentExtInfoModel opponentExtInfo = default(OpponentExtInfoModel), bool? selfPlaying = default(bool?), string speed = default(string), string difficulty = default(string), GameArmySettingModel userArmySetting = default(GameArmySettingModel), GameArmySettingModel opponentArmySetting = default(GameArmySettingModel), IList<CastleRouteDto> routes = default(IList<CastleRouteDto>), string id = default(string))
        {
            CreatedAt = createdAt;
            CreatedBy = createdBy;
            OpponentId = opponentId;
            UserHeroId = userHeroId;
            OpponentHeroId = opponentHeroId;
            Status = status;
            Castles = castles;
            Position = position;
            User = user;
            UserHero = userHero;
            Opponent = opponent;
            OpponentHero = opponentHero;
            RedCastleAmount = redCastleAmount;
            BlueCastleAmount = blueCastleAmount;
            NeutrualCastleAmount = neutrualCastleAmount;
            OpponentExtInfo = opponentExtInfo;
            SelfPlaying = selfPlaying;
            Speed = speed;
            Difficulty = difficulty;
            UserArmySetting = userArmySetting;
            OpponentArmySetting = opponentArmySetting;
            Routes = routes;
            Id = id;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "createdAt")]
        public System.DateTime? CreatedAt { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "opponentId")]
        public string OpponentId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "userHeroId")]
        public string UserHeroId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "opponentHeroId")]
        public string OpponentHeroId { get; set; }

        /// <summary>
        /// Gets or sets possible values include: 'Requesting', 'Playing',
        /// 'Rejected', 'Ended'
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "castles")]
        public IList<string> Castles { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "position")]
        public PositionModel Position { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "user")]
        public UserModel User { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "userHero")]
        public HeroModel UserHero { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "opponent")]
        public UserModel Opponent { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "opponentHero")]
        public HeroModel OpponentHero { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "redCastleAmount")]
        public int? RedCastleAmount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "blueCastleAmount")]
        public int? BlueCastleAmount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "neutrualCastleAmount")]
        public int? NeutrualCastleAmount { get; set; }

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

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "opponentArmySetting")]
        public GameArmySettingModel OpponentArmySetting { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "routes")]
        public IList<CastleRouteDto> Routes { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

    }
}
