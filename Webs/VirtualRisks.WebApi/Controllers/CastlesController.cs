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
            new LocationModel(10.78646,106.69762),
            new LocationModel(10.78551,106.69867),
            new LocationModel(10.78487,106.69932),
            new LocationModel(10.78454,106.69966),
            new LocationModel(10.78454,106.69966),
            new LocationModel(10.78506,106.70013),
            new LocationModel(10.78509,106.70016),
            new LocationModel(10.78528,106.70034),
            new LocationModel(10.78544 ,106.7005),
            new LocationModel(10.78581,106.70084),
            new LocationModel(10.78604,106.70109),
            new LocationModel(10.78644,106.70149),
            new LocationModel(10.78644,106.70149),
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
        [SwaggerResponse(HttpStatusCode.OK, "Successfull", typeof(List<CastleModel>))]
        public async Task<IHttpActionResult> GetCastles()
        {
            var castlesLocation = _directionService.GetDirection(_myLocation, _locations);
            Result<List<CastleModel>> result = Result.Ok(castlesLocation.Select(e => new CastleModel()
            {
                Position = e
            }).ToList());
            if (result.IsFailure)
                return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}