namespace CastleGo.Shared.Games
{
    public class SoldierModel : BaseModel
    {
        public CastleTroopTypeModel CastleTroopType { get; set; }
        public double UpkeepCoins { get; set; }
    }
}
