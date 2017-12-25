using System.Threading.Tasks;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using GoogleMapsApi.Entities.Directions.Response;

namespace CastleGo.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDirectionProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        Task<DirectionsResponse> GetDirection(PositionModel from, PositionModel to);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        RouteModel FormatRoute(Leg selectedRouteLeg, GameSpeed gameSpeed);

        Task<RouteModel> GetFirstRoute(PositionModel @from, PositionModel to, GameSpeed gameSpeed);
    }
}
