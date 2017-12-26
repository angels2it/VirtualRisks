using CastleGo.Shared.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoRepository;
using System;
using System.Collections.Generic;

namespace CastleGo.Entities
{
    public class Game : Entity
    {
        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string OpponentId { get; set; }

        public string UserHeroId { get; set; }

        public string OpponentHeroId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public GameStatus Status { get; set; }

        public List<string> Castles { get; set; }

        public Position Position { get; set; }

        public int RedCastleAmount { get; set; }

        public int BlueCastleAmount { get; set; }

        public int NeutrualCastleAmount { get; set; }
        public OpponentExtInfo OpponentExtInfo { get; set; }
        public int OpponentStreamVersion { get; set; }
        public int UserStreamVersion { get; set; }
        public bool SelfPlaying { get; set; }
        [BsonRepresentation(BsonType.String)]
        public GameSpeed Speed { get; set; }
        [BsonRepresentation(BsonType.String)]
        public GameDifficulfy Difficulty { get; set; }

        public bool IsRemoved { get; set; }
        public GameArmySetting UserArmySetting { get; set; }
        public GameArmySetting OpponentArmySetting { get; set; }
        public List<CastleRoute> Routes { get; set; }
    }

    public class CastleRoute
    {
        public string FromCastle { get; set; }
        public string ToCastle { get; set; }
        public Route Route { get; set; }
    }

    public class OpponentExtInfo
    {
        public string Provider { get; set; }
        public string Key { get; set; }
        public string KeyName { get; set; }
    }
}
