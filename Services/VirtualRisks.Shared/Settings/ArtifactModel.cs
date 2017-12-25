using System;
using System.Collections.Generic;
using CastleGo.Shared;
using CastleGo.Shared.Common;

namespace CastleGo.Application.Artifacts.Dtos
{
    public class ArtifactModel : BaseModel
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public TimeSpan VisibilityInterval { get; set; }
        public List<ArtifactPrizeModel> Prizes { get; set; }
    }

    public class ArtifactOnMapModel : ArtifactModel
    {
        public PositionModel Position { get; set; }
        public bool IsOccupied { get; set; }
    }

    public class ArtifactPrizeModel
    {
        public Prize Prize { get; set; }
        public string Data { get; set; }
        public string DataType { get; set; }
    }
}