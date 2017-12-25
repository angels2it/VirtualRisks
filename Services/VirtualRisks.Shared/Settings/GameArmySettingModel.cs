using System;
using System.Collections.Generic;
using CastleGo.Shared;

namespace CastleGo.Application.Settings.Dtos
{
    public class GameArmySettingModel : BaseModel
    {
        public string Name { get; set; }
        public List<GameCastleSettingModel> Castles { get; set; }

        public void GenerateDefaultCastles()
        {
            Castles = new List<GameCastleSettingModel>();
            for (int i = 1; i <= 10; i++)
            {
                Castles.Add(new GameCastleSettingModel()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = $"Castle{i}"
                });
            }
        }
    }

    public class GameCastleSettingModel : BaseModel
    {
        public string Name { get; set; }
    }
}
