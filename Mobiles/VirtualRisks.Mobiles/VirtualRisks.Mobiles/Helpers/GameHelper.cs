using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using CastleGo.Shared.Games.Events;
using CastleGo.Shared.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VirtualRisks.Mobiles.Helpers
{
    public static class GameHelper
    {
        public static string GetSiegeIcon(Army army)
        {
            switch (army)
            {
                case Army.Red:
                    return "marker_tank_red";
                default:
                    return "marker_tank_blue";
            }
        }
        public static string GetCastleIcon(GameStateModel state, CastleStateModel castle, bool includeSiege = true)
        {
            string icon = GetCastleIconByArmy(castle.Army);
            return icon;
        }

        public static string GetCastleIconByArmy(Army army, bool hasBattle = false)
        {
            string hasbattleicon = string.Empty;
            if (hasBattle)
                hasbattleicon = "_has_battle";
            switch (army)
            {
                case Army.Red:
                    return "red_castle" + hasbattleicon;
                case Army.Blue:
                    return "blue_castle" + hasbattleicon;
                default:
                    return "gray_castle" + hasbattleicon;
            }
        }
        public static string GetSiegeCastleIconByArmy(Army army, Army siegebyArmy)
        {
            return GetCastleIconByArmy(army);
            var castleIcon = GetCastleIconByArmy(army);
            castleIcon += "_";
            if (siegebyArmy == Army.Red)
                castleIcon += "red_siege";
            else
                castleIcon += "blue_siege";
            return castleIcon;
        }
        public static Color GetColorByArmy(Army userArmy)
        {
            switch (userArmy)
            {
                case Army.Red:
                    return Color.Red;
                case Army.Blue:
                    return Color.Blue;
                default:
                    return Color.Gray;
            }

        }

        public static string GetOpponentScore(GameModel context, Army userArmy)
        {
            if (userArmy == Army.Blue)
                return context.RedCastleAmount.ToString();
            return context.BlueCastleAmount.ToString();
        }

        public static string GetUserScore(GameModel context, Army userArmy)
        {
            switch (userArmy)
            {
                case Army.Red:
                    return context.RedCastleAmount.ToString();
                case Army.Blue:
                    return context.BlueCastleAmount.ToString();
                default:
                    return context.NeutrualCastleAmount.ToString();
            }
        }

        public static Army GetUserArmy(GameModel context)
        {
            if (context == null)
            {
#if DEBUG
                Debugger.Break();
#endif
                return Army.Neutrual;
            }
            return context.CreatedBy == Settings.UserId ? Army.Blue : Army.Red;
        }
        public static Army GetOpponentArmy(GameModel context)
        {
            if (context == null)
            {
#if DEBUG
                Debugger.Break();
#endif
                return Army.Neutrual;
            }
            return context.CreatedBy == Settings.UserId ? Army.Red : Army.Blue;
        }
        public static string GetOpponentName(GameModel context, Army currentArmy)
        {
            if (context == null)
                return string.Empty;
            if (currentArmy == Army.Blue)
            {
                if (context.SelfPlaying)
                    return "Computer";
                if (context.Opponent != null)
                {
                    UserModel opponent = context.Opponent;
                    return opponent?.Name ?? string.Empty;
                }
                return context.OpponentExtInfo?.KeyName;
            }
            UserModel user = context.User;
            return user?.Name ?? string.Empty;
        }

        //public static FormattedString GetVsFormattedString(GameModel context)
        //{
        //    Army userArmy = GetUserArmy(context);
        //    FormattedString formattedString = new FormattedString();
        //    formattedString.Spans.Add(new Span
        //    {
        //        Text = App.Locator.CommonVr.You,
        //        ForegroundColor = Color.Blue
        //    });
        //    formattedString.Spans.Add(new Span
        //    {
        //        Text = $" {App.Locator.CommonVr.Vs} ",
        //        ForegroundColor = Color.Black
        //    });
        //    formattedString.Spans.Add(new Span
        //    {
        //        Text = GetOpponentName(context, userArmy),
        //        ForegroundColor = Color.Red
        //    });
        //    return formattedString;
        //}

        //public static FormattedString GetScoreFormattedString(GameModel bindingContext)
        //{
        //    Army userArmy = GetUserArmy(bindingContext);
        //    var formattedString2 = new FormattedString();
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = $"{App.Locator.CommonVr.Score} ",
        //        ForegroundColor = Color.Black
        //    });
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = GetUserScore(bindingContext, userArmy),
        //        ForegroundColor = GetColorByArmy(userArmy)
        //    });
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = " - ",
        //        ForegroundColor = Color.Black
        //    });
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = GetOpponentScore(bindingContext, userArmy),
        //        ForegroundColor = GetColorByArmy(userArmy == Army.Blue ? Army.Red : Army.Blue)
        //    });
        //    return formattedString2;
        //}
        //public static FormattedString GetScoreFormattedString(Army userArmy, string userName, string opponentName, string userScore, string opponentScore)
        //{
        //    var opponentArmy = userArmy == Army.Blue ? Army.Red : Army.Blue;
        //    var formattedString2 = new FormattedString();
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = $"{userName} ",
        //        ForegroundColor = GetColorByArmy(userArmy)
        //    });
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = userScore,
        //        ForegroundColor = GetColorByArmy(userArmy)
        //    });
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = " - ",
        //        ForegroundColor = Color.Black
        //    });
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = $"{opponentName} ",
        //        ForegroundColor = GetColorByArmy(opponentArmy)
        //    });
        //    formattedString2.Spans.Add(new Span
        //    {
        //        Text = opponentScore,
        //        ForegroundColor = GetColorByArmy(opponentArmy)
        //    });
        //    return formattedString2;
        //}

        public static string GetHeroIcon(Army army)
        {
            switch (army)
            {
                case Army.Red:
                    return "hero_red_small";
                case Army.Blue:
                    return "hero_blue_small";
                default:
                    return "icon";
            }
        }

        internal static string GetUserHeroId(GameModel game)
        {
            var army = GetUserArmy(game);
            if (army == Army.Blue)
                return game.UserHeroId;
            return game.OpponentHeroId;
        }

        public static List<EventBaseModel> GetEventFormatted(List<WebApi.RestClient.Models.EventBaseModel> events)
        {
            var eventsFormated = new List<EventBaseModel>();
            if (events == null)
                return eventsFormated;
            foreach (var @event in events)
            {
                var typeName = @event.Type;
                var type = Type.GetType(typeName);
                var jobj = JObject.Parse(JsonConvert.SerializeObject(@event));
                var obj = jobj.ToObject(type) as EventBaseModel;
                eventsFormated.Add(obj);
            }
            return eventsFormated;
        }

        public static Army GetArmyByUserId(GameStateModel state, string id)
        {
            if (state.SelfPlaying && string.IsNullOrEmpty(id))
                return Army.Red;
            return state.UserId == id ? Army.Blue : Army.Red;
        }

        public static string GetOpponentId(GameModel context, Army userArmy)
        {
            if (context == null)
                return string.Empty;
            if (userArmy == Army.Blue)
            {
                if (context.SelfPlaying)
                    return string.Empty;
                return context.OpponentId;
            }
            return context.CreatedBy;
        }
        static readonly Dictionary<int, string> BlueCastleRedSiege = new Dictionary<int, string>()
        {
            {100, "blue_castle_red_siege" },
            {90, "blue_castle_red_siege_10" },
            {80, "blue_castle_red_siege_20" },
            {70, "blue_castle_red_siege_30" },
            {60, "blue_castle_red_siege_40" },
            {50, "blue_castle_red_siege_50" },
            {40, "blue_castle_red_siege_60" },
            {30, "blue_castle_red_siege_70" },
            {20, "blue_castle_red_siege_80" },
            {10, "blue_castle_red_siege_90" },
            {0, "blue_castle_red_siege_100" },
        };
        static readonly Dictionary<int, string> RedCastleBlueSiege = new Dictionary<int, string>()
        {
            {100, "red_castle_blue_siege" },
            {90, "red_castle_blue_siege_10" },
            {80, "red_castle_blue_siege_20" },
            {70, "red_castle_blue_siege_30" },
            {60, "red_castle_blue_siege_40" },
            {50, "red_castle_blue_siege_50" },
            {40, "red_castle_blue_siege_60" },
            {30, "red_castle_blue_siege_70" },
            {20, "red_castle_blue_siege_80" },
            {10, "red_castle_blue_siege_90" },
            {0, "red_castle_blue_siege_100" },
        };
        static readonly Dictionary<int, string> NeutralBlueSiege = new Dictionary<int, string>()
        {
            {100, "gray_castle_blue_siege" },
            {90, "gray_castle_blue_siege_10" },
            {80, "gray_castle_blue_siege_20" },
            {70, "gray_castle_blue_siege_30" },
            {60, "gray_castle_blue_siege_40" },
            {50, "gray_castle_blue_siege_50" },
            {40, "gray_castle_blue_siege_60" },
            {30, "gray_castle_blue_siege_70" },
            {20, "gray_castle_blue_siege_80" },
            {10, "gray_castle_blue_siege_90" },
            {0, "gray_castle_blue_siege_100" },
        };
        static readonly Dictionary<int, string> NeutralRedSiege = new Dictionary<int, string>()
        {
            {100, "gray_castle_red_siege" },
            {90, "gray_castle_red_siege_10" },
            {80, "gray_castle_red_siege_20" },
            {70, "gray_castle_red_siege_30" },
            {60, "gray_castle_red_siege_40" },
            {50, "gray_castle_red_siege_50" },
            {40, "gray_castle_red_siege_60" },
            {30, "gray_castle_red_siege_70" },
            {20, "gray_castle_red_siege_80" },
            {10, "gray_castle_red_siege_90" },
            {0, "gray_castle_red_siege_100" },
        };
        public static string GetCastleIconByBattleAtPercent(GameStateModel state, CastleStateModel castle, double percent)
        {
            try
            {
                if (castle.Army == Army.Blue)
                {
                    return BlueCastleRedSiege.Where(e => e.Key <= percent).OrderByDescending(e => e.Key).First().Value;
                }
                if (castle.Army == Army.Red)
                {
                    return RedCastleBlueSiege.Where(e => e.Key <= percent).OrderByDescending(e => e.Key).First().Value;
                }
                bool isBlue = castle.Siege.OwnerUserId == state?.UserId;
                if (isBlue)
                    return NeutralBlueSiege.Where(e => e.Key <= percent).OrderByDescending(e => e.Key).First().Value;
                return NeutralRedSiege.Where(e => e.Key <= percent).OrderByDescending(e => e.Key).First().Value;
            }
            catch (Exception e)
            {
                return GetCastleIcon(state, castle) + "_100";
            }
        }

        //public static Object3DFixedIcon GetObject3DHeroIconByArmy(Army army)
        //{
        //    string armyStr = army.ToString().ToLower();
        //    return new Object3DFixedIcon()
        //    {
        //        Stay = $"hero_{armyStr}_small",
        //        Angle0 = $"hero_{armyStr}_0",
        //        Angle45 = $"hero_{armyStr}_45",
        //        Angle90 = $"hero_{armyStr}_90",
        //        Angle135 = $"hero_{armyStr}_135",
        //        Angle180 = $"hero_{armyStr}_180",
        //        Angle225 = $"hero_{armyStr}_225",
        //        Angle270 = $"hero_{armyStr}_270",
        //        Angle315 = $"hero_{armyStr}_315",
        //    };
        //}

        public static string GetArmyName(GameModel game, Army army)
        {
            return (army == Army.Blue ? game.UserArmySetting?.Name : game.OpponentArmySetting?.Name) ?? string.Empty;
        }

        //public static string GetImageUrl(string icon)
        //{
        //    if (string.IsNullOrEmpty(icon))
        //        return string.Empty;
        //    return $"{Settings.ApiDomain}upload/images/{icon}";
        //}
    }
}
