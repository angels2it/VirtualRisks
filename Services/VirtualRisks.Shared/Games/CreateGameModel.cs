using CastleGo.Application.Settings.Dtos;
using CastleGo.Shared.Common;
using CastleGo.Shared.Users;

namespace CastleGo.Shared.Games
{
    public class CreateGameModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string OpponentId { get; set; }
        public OpponentExtInfoModel OpponentExtInfo { get; set; }
        public bool SelfPlaying { get; set; }
        public GameSpeed Speed { get; set; }
        public GameDifficulfy Difficulty { get; set; }
        public GameArmySettingModel UserArmySetting { get; set; }
    }
}
