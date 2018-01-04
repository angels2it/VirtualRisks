using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using CastleGo.Domain;
using CastleGo.Shared;
using CastleGo.Shared.Common;
using GoogleMapsApi.Entities.Directions.Request;
using GoogleMapsApi.Entities.Directions.Response;

namespace CastleGo.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectionProvider : IDirectionProvider
    {
        private readonly GameSettings _gameSettings;

        public DirectionProvider(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }

        public Task<DirectionsResponse> GetDirection(PositionModel @from, PositionModel to)
        {
            try
            {
                return GoogleMapsApi.GoogleMaps.Directions.QueryAsync(new DirectionsRequest()
                {
                    Origin = $"{@from.Lat},{@from.Lng}",
                    Destination = $"{to.Lat},{to.Lng}",
                    ApiKey = ConfigurationManager.AppSettings["GoogleApiKey"]
                });
            }
            catch (Exception)
            {
                return Task.FromResult<DirectionsResponse>(null);
            }
        }

        public RouteModel FormatRoute(Leg selectedRouteLeg, GameSpeed gameSpeed)
        {
            return new RouteModel
            {
                Steps = selectedRouteLeg.Steps.Select(step => new RouteStepModel()
                {
                    StartLocation =
                        new PositionModel { Lat = step.StartLocation.Latitude, Lng = step.StartLocation.Longitude },
                    EndLocation = new PositionModel { Lat = step.EndLocation.Latitude, Lng = step.EndLocation.Longitude },
                    Distance = step.Distance.Value,
                    Duration = TimeSpan.FromSeconds(step.Distance.Value / _gameSettings.GetMovementSpeedOfGame(GameSpeedHelper.GetSpeedValue(gameSpeed)))
                }).ToList(),
                Distance = selectedRouteLeg.Distance.Value,
                Duration = TimeSpan.FromSeconds(selectedRouteLeg.Distance.Value / _gameSettings.GetMovementSpeedOfGame(GameSpeedHelper.GetSpeedValue(gameSpeed)))
            };
        }
        public async Task<RouteModel> GetFirstRoute(PositionModel @from, PositionModel to, GameSpeed gameSpeed)
        {
            var direction = await GetDirection(@from, to);
            if (direction.Status == DirectionsStatusCodes.OK && direction.Routes != null && direction.Routes.Any() && direction.Routes.ElementAt(0).Legs != null && direction.Routes.ElementAt(0).Legs.Any())
            {
                var selectedRouteLeg = direction.Routes.ElementAt(0).Legs.ElementAt(0);
                return FormatRoute(selectedRouteLeg, gameSpeed);
            }
            return null;
        }
    }
}