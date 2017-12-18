using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CSharpFunctionalExtensions;
using Swashbuckle.Swagger.Annotations;
using VirtualRisks.WebApi.Directions;

namespace VirtualRisks.WebApi.Controllers
{
    public class CastleModel
    {
        public LocationModel Position { get; set; }
        public int Index { get; set; }
        public int RouteCount { get; set; }
    }

    public class RouteModel
    {
        public RouteModel(CastleModel fromCastle, CastleModel toCastle)
        {
            FromCastle = fromCastle;
            ToCastle = toCastle;
        }

        public CastleModel FromCastle { get; set; }
        public CastleModel ToCastle { get; set; }
    }
    public class LocationModel
    {
        public double Lat { get; set; }
        public double Lng { get; set; }

        public LocationModel(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }
    }
    public class CastlesController : ApiController
    {
        private LocationModel _myLocation = new LocationModel(10.78761, 106.6987);
        List<LocationModel> _locations = new List<LocationModel>()
        {
            new LocationModel(10.78739,106.69848),
            new LocationModel(10.78698,106.69809),
            new LocationModel(10.78646,106.69762),
            new LocationModel(10.78646,106.69869),
            new LocationModel(10.78551,106.69867),
            new LocationModel(10.78487,106.69932),
            new LocationModel(10.78454,106.69966),
            new LocationModel(10.78454,106.69972),
            new LocationModel(10.78506,106.70013),
            new LocationModel(10.78509,106.70016),
            new LocationModel(10.78528,106.70034),
            new LocationModel(10.78544,106.7005),
            new LocationModel(10.78581,106.70084),
            new LocationModel(10.78604,106.70109),
            new LocationModel(10.78644,106.70149),
            new LocationModel(10.78644,106.70182),
            new LocationModel(10.78654,106.70162),
            new LocationModel(10.78659,106.70157),
            new LocationModel(10.78784,106.70023)
        };

        private readonly ICastleDirectionService _directionService;

        public CastlesController(ICastleDirectionService directionService)
        {
            _directionService = directionService;
        }
        [HttpGet]
        [SwaggerOperation("GetCastles")]
        [SwaggerResponse(HttpStatusCode.OK, "Successfull", typeof(GetCastlesResponse))]
        public async Task<IHttpActionResult> GetCastles()
        {
            var locations = _directionService.GetDirection(_myLocation, _locations);
            locations.Insert(0, _myLocation);
            var routes = new List<RouteModel>();
            var castles = new List<CastleModel>();
            castles.Add(new CastleModel()
            {
                Position = locations[0],
                Index = 0
            });
            for (int i = 1; i < locations.Count; i++)
            {
                var castle = new CastleModel()
                {
                    Position = locations[i],
                    Index = i
                };
                castles.Add(castle);
                routes.Add(new RouteModel(castles[i - 1], castles[i]));
                castles[i - 1].RouteCount++;
                castles[i].RouteCount++;
            }
            routes.Add(new RouteModel(castles[0], castles[castles.Count - 1]));
            castles[0].RouteCount++;
            castles[castles.Count - 1].RouteCount++;
            var min = 0;
            var max = 18;
            while (max - min > 1)
            {
                routes.Add(new RouteModel(castles[min], castles[max]));
                castles[min].RouteCount++;
                castles[max].RouteCount++;
                min++;
                max--;
            }
            return Ok(new GetCastlesResponse
            {
                Castles = castles,
                Routes = routes
            });
        }
    }

    public class GetCastlesResponse
    {
        public List<CastleModel> Castles { get; set; }
        public List<RouteModel> Routes { get; set; }
    }
}