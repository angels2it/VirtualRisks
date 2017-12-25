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
    /// Extension methods for Setting.
    /// </summary>
    public static partial class SettingExtensions
    {
            /// <summary>
            /// Get settings metadata
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object Metadata(this ISetting operations)
            {
                return operations.MetadataAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get settings metadata
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> MetadataAsync(this ISetting operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.MetadataWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object AddOrUpdateTroopType(this ISetting operations, AddTroopTypeModel model)
            {
                return operations.AddOrUpdateTroopTypeAsync(model).GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> AddOrUpdateTroopTypeAsync(this ISetting operations, AddTroopTypeModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddOrUpdateTroopTypeWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get all armies
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object GetArmies(this ISetting operations)
            {
                return operations.GetArmiesAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get all armies
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetArmiesAsync(this ISetting operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetArmiesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Add/Update new army
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            public static object AddOrUpdateArmy(this ISetting operations, GameArmySettingModel model)
            {
                return operations.AddOrUpdateArmyAsync(model).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Add/Update new army
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='model'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> AddOrUpdateArmyAsync(this ISetting operations, GameArmySettingModel model, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddOrUpdateArmyWithHttpMessagesAsync(model, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete army
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            public static object DeleteArmy(this ISetting operations, string id)
            {
                return operations.DeleteArmyAsync(id).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Delete army
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteArmyAsync(this ISetting operations, string id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteArmyWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get all artifacts
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static object GetArtifactMetaData(this ISetting operations)
            {
                return operations.GetArtifactMetaDataAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get all artifacts
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetArtifactMetaDataAsync(this ISetting operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetArtifactMetaDataWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
