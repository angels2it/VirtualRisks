using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CSharpFunctionalExtensions;

namespace VirtualRisks.WebApi.Controllers
{
    public class CastleModel
    {
        
    }
    public class CastlesController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetCastles()
        {
            Result<List<CastleModel>> result = Result.Ok(new List<CastleModel>());
            if (result.IsFailure)
                return BadRequest(result.Error);
            return Ok(result.Value);
        }
    }
}