using System;
using CastleGo.Shared.Common;
using MongoRepository;
using System.Collections.Generic;

namespace CastleGo.Entities
{
    public class Castle : Entity
    {
        public Army Army { get; set; }

        public string OwnerId { get; set; }

        public string OwnerUserId { get; set; }

        public Position Position { get; set; }

        public int MaxResourceLimit { get; set; }

        public List<string> Soldiers { get; set; }

        public CastleTroopType CastleTroopType { get; set; }
    }

    public class Artifact : Entity
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public TimeSpan VisibilityInterval { get; set; }
        public List<ArtifactPrize> Prizes { get; set; }
    }

    public class ArtifactPrize : Entity
    {
        public Prize Prize { get; set; }
        public string Data { get; set; }
        public string DataType { get; set; }
    }

    
}
