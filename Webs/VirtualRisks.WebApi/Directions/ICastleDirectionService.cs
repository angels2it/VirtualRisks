using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GoogleMapsAPI.NET.API.Client.Interfaces;
using GoogleMapsAPI.NET.API.Common.Components.Locations;
using GoogleMapsAPI.NET.API.Common.Components.Locations.Common;
using GoogleMapsAPI.NET.API.Common.Components.Locations.Interfaces.Combined;
using GoogleMapsAPI.NET.API.Directions.Enums;
using VirtualRisks.WebApi.Controllers;

namespace VirtualRisks.WebApi.Directions
{
    public static class LocationExtensions
    {
        public static GeoCoordinatesLocation GetLatLng(this LocationModel location)
        {
            return new GeoCoordinatesLocation(location.Lat, location.Lng);
        }
    }

    public interface ICastleDirectionService
    {
        List<LocationModel> GetDirection(LocationModel start, List<LocationModel> saleorders);
    }
    public class CastleDirectionService : ICastleDirectionService
    {
        private readonly IMapsAPIClient _client;

        public CastleDirectionService(IMapsAPIClient client)
        {
            _client = client;
        }

        public List<LocationModel> GetDirection(LocationModel start, List<LocationModel> saleorders)
        {
            var response = _client.DistanceMatrix.GetDistanceMatrix(new List<IAddressOrGeoCoordinatesLocation>()
                {
                    start.GetLatLng()
                },
                saleorders.Select(e => e.GetLatLng()).ToList()
            );
            if (response.HasErrorMessage)
            {
                throw new Exception(response.ErrorMessage);
            }

            var matrixData = response.Rows.First();
            var maxDistance = matrixData.Elements.Max(e => e.Distance.Value);
            var maxDistanceIndex = matrixData.Elements.ToList().FindIndex(e => e.Distance.Value == maxDistance);
            var end = saleorders[maxDistanceIndex];
            var waypoints = saleorders.Except(new List<LocationModel>()
            {
                end
            }).ToList();
            var directionResponse = _client.Directions.GetDirections(
                start.GetLatLng(),
                end.GetLatLng(),
                TransportationModeEnum.Driving,
                waypoints.Select(e => e.GetLatLng()).Cast<Location>().ToList(),
                optimizeWaypoints: true);
            if (directionResponse.HasErrorMessage)
            {
                Console.WriteLine("Unable to geocode.  Status={0} and ErrorMessage={1}", response.Status,
                    response.ErrorMessage);
                throw new Exception(directionResponse.ErrorMessage);
            }
            var route = directionResponse.Routes.First();
            var sortedWaypoints = waypoints.OrderBy(e => route.WaypointOrder.IndexOf(waypoints.IndexOf(e))).ToList();
            sortedWaypoints.Add(end);
            return sortedWaypoints;
        }
    }

}