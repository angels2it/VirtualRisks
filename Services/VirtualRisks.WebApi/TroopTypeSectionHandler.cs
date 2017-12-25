using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;

namespace CastleGo.WebApi
{
    ///// <summary>
    ///// 
    ///// </summary>
    //public class TroopTypesCollection : List<CastleTroopTypeModel> { }
    ///// <summary>
    ///// 
    ///// </summary>
    //public class TroopTypeSectionHandler : IConfigurationSectionHandler
    //{
    //    /// <inheritdoc />
    //    public object Create(object parent, object configContext, XmlNode section)
    //    {
    //        var app = new TroopTypesCollection();
    //        app.AddRange(from XmlElement childNode in section.ChildNodes select GetTroopData(childNode));
    //        return app;
    //    }

    //    private string[] keyStrings = new[]
    //    {
    //        "resourceType",
    //        "minAttackStrength",
    //        "maxAttackStrength",
    //        "minHealth",
    //        "maxHealth",
    //        "minMovementSpeed",
    //        "maxMovementSpeed",
    //        "minProductionSpeed",
    //        "maxProductionSpeed",
    //        "minUpkeepCoins",
    //        "maxUpkeepCoins",
    //        "isFlight",
    //        "isOverComeWalls"
    //    };
    //    private CastleTroopTypeModel GetTroopData(XmlElement p)
    //    {
    //        foreach (var keyString in keyStrings)
    //        {
    //            if (!p.HasAttribute(keyString))
    //                throw new KeyNotFoundException($"TroopData: {keyString} key is required");
    //        }
    //        return new CastleTroopTypeModel
    //        {
    //            ResourceType = Enum.Parse(typeof(TroopType), p.GetAttribute(keyStrings[0])).ToString(),
    //            MinAttackStrength = int.Parse(p.GetAttribute(keyStrings[1])),
    //            MaxAttackStrength = int.Parse(p.GetAttribute(keyStrings[2])),
    //            MinHealth = int.Parse(p.GetAttribute(keyStrings[3])),
    //            MaxHealth = int.Parse(p.GetAttribute(keyStrings[4])),
    //            MinMovementSpeed = int.Parse(p.GetAttribute(keyStrings[5])),
    //            MaxMovementSpeed = int.Parse(p.GetAttribute(keyStrings[6])),
    //            MinProductionSpeed = int.Parse(p.GetAttribute(keyStrings[7])),
    //            MaxProductionSpeed = int.Parse(p.GetAttribute(keyStrings[8])),
    //            MinUpkeepCoins = int.Parse(p.GetAttribute(keyStrings[9])),
    //            MaxUpkeepCoins = int.Parse(p.GetAttribute(keyStrings[10])),
    //            IsFlight = bool.Parse(p.GetAttribute(keyStrings[11])),
    //            IsOverComeWalls = bool.Parse(p.GetAttribute(keyStrings[12]))
    //        };
    //    }
    //}
}