// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace VirtualRisks.WebApi.RestClient
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for Game.
    /// </summary>
    public static partial class GameExtensions
    {
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='page'>
            /// </param>
            /// <param name='take'>
            /// </param>
            public static object Paging(this IGame operations, int page, int take)
            {
                return operations.PagingAsync(page, take).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='page'>
            /// </param>
            /// <param name='take'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> PagingAsync(this IGame operations, int page, int take, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.PagingWithHttpMessagesAsync(page, take, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Remove game
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static object Remove(this IGame operations, string id)
            {
                return operations.RemoveAsync(id).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Remove game
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> RemoveAsync(this IGame operations, string id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RemoveWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='streamVersion'>
            /// </param>
            public static GameStateModel Build(this IGame operations, string id, int? streamVersion = default(int?))
            {
                return operations.BuildAsync(id, streamVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='streamVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GameStateModel> BuildAsync(this IGame operations, string id, int? streamVersion = default(int?), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.BuildWithHttpMessagesAsync(id, streamVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='streamVersion'>
            /// </param>
            public static object GetGame(this IGame operations, string id, int streamVersion)
            {
                return operations.GetGameAsync(id, streamVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='streamVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetGameAsync(this IGame operations, string id, int streamVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetGameWithHttpMessagesAsync(id, streamVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static GameModel Info(this IGame operations, string id)
            {
                return operations.InfoAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GameModel> InfoAsync(this IGame operations, string id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.InfoWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static CreateGameModelResult Create(this IGame operations, CreateGameModel model)
            {
                return operations.CreateAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<CreateGameModelResult> CreateAsync(this IGame operations, CreateGameModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='castleId'>
            /// </param>
            /// <param name='streamVersion'>
            /// </param>
            public static DetailCastleStateModel Castle(this IGame operations, string id, string castleId, int streamVersion)
            {
                return operations.CastleAsync(id, castleId, streamVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='castleId'>
            /// </param>
            /// <param name='streamVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<DetailCastleStateModel> CastleAsync(this IGame operations, string id, string castleId, int streamVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CastleWithHttpMessagesAsync(id, castleId, streamVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='model'>
            /// </param>
            public static string Battalion(this IGame operations, string id, BattalionModel model)
            {
                return operations.BattalionAsync(id, model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<string> BattalionAsync(this IGame operations, string id, BattalionModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.BattalionWithHttpMessagesAsync(id, model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='model'>
            /// </param>
            public static string MoveSoldier(this IGame operations, string id, MoveSoldierModel model)
            {
                return operations.MoveSoldierAsync(id, model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<string> MoveSoldierAsync(this IGame operations, string id, MoveSoldierModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.MoveSoldierWithHttpMessagesAsync(id, model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Change troop type of castle
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object ChangeTroopType(this IGame operations, string id, ChangeTroopTypeModel model)
            {
                return operations.ChangeTroopTypeAsync(id, model).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Change troop type of castle
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> ChangeTroopTypeAsync(this IGame operations, string id, ChangeTroopTypeModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ChangeTroopTypeWithHttpMessagesAsync(id, model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get route between two castle
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object Route(this IGame operations, BattalionModel model)
            {
                return operations.RouteAsync(model).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get route between two castle
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> RouteAsync(this IGame operations, BattalionModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RouteWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static object GetStreamVersion(this IGame operations, string id)
            {
                return operations.GetStreamVersionAsync(id).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetStreamVersionAsync(this IGame operations, string id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetStreamVersionWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='streamVersion'>
            /// </param>
            public static object UpdateStreamVersion(this IGame operations, string id, int streamVersion)
            {
                return operations.UpdateStreamVersionAsync(id, streamVersion).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='streamVersion'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> UpdateStreamVersionAsync(this IGame operations, string id, int streamVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateStreamVersionWithHttpMessagesAsync(id, streamVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object IsBattalionMovementExecuted(this IGame operations, CheckBattalionMovementEventModel model)
            {
                return operations.IsBattalionMovementExecutedAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> IsBattalionMovementExecutedAsync(this IGame operations, CheckBattalionMovementEventModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.IsBattalionMovementExecutedWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object Accepted(this IGame operations, string id, GameAcceptedModel model)
            {
                return operations.AcceptedAsync(id, model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> AcceptedAsync(this IGame operations, string id, GameAcceptedModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AcceptedWithHttpMessagesAsync(id, model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Suspend troop production
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// GameId
            /// </param>
            /// <param name='castleId'>
            /// CastleId
            /// </param>
            public static object SuspendCastleProduction(this IGame operations, string id, string castleId)
            {
                return operations.SuspendCastleProductionAsync(id, castleId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Suspend troop production
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// GameId
            /// </param>
            /// <param name='castleId'>
            /// CastleId
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> SuspendCastleProductionAsync(this IGame operations, string id, string castleId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.SuspendCastleProductionWithHttpMessagesAsync(id, castleId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Suspend troop production
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// GameId
            /// </param>
            /// <param name='castleId'>
            /// CastleId
            /// </param>
            public static object RestartCastleProduction(this IGame operations, string id, string castleId)
            {
                return operations.RestartCastleProductionAsync(id, castleId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Suspend troop production
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// GameId
            /// </param>
            /// <param name='castleId'>
            /// CastleId
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> RestartCastleProductionAsync(this IGame operations, string id, string castleId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RestartCastleProductionWithHttpMessagesAsync(id, castleId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Upgrade castle strength
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='castleId'>
            /// </param>
            public static object UpgradeCastle(this IGame operations, string id, string castleId)
            {
                return operations.UpgradeCastleAsync(id, castleId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Upgrade castle strength
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='castleId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> UpgradeCastleAsync(this IGame operations, string id, string castleId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpgradeCastleWithHttpMessagesAsync(id, castleId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='artifactId'>
            /// </param>
            public static object OccupiedArtifact(this IGame operations, string id, string artifactId)
            {
                return operations.OccupiedArtifactAsync(id, artifactId).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='artifactId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> OccupiedArtifactAsync(this IGame operations, string id, string artifactId, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.OccupiedArtifactWithHttpMessagesAsync(id, artifactId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
