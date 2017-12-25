using System;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CastleGo.Application.Artifacts;
using CastleGo.Application.Artifacts.Dtos;
using CastleGo.Application.CastleTroopTypes;
using CastleGo.Application.Settings;
using CastleGo.Application.Settings.Dtos;
using CastleGo.Shared.Common;
using CastleGo.Shared.Games;
using CastleGo.Shared.Settings;

namespace CastleGo.WebApi.Controllers
{
    /// <summary>
    /// settings
    /// </summary>
    [RoutePrefix(Startup.ApiPrefix + "/settings")]
    public class SettingController : ProtectedController
    {
        private readonly ICastleTroopTypeService _castleTroopTypeService;
        private readonly IGameArmySettingService _gameArmySettingService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="castleTroopTypeService"></param>
        /// <param name="gameArmySettingService"></param>
        public SettingController(ICastleTroopTypeService castleTroopTypeService, IGameArmySettingService gameArmySettingService)
        {
            _castleTroopTypeService = castleTroopTypeService;
            _gameArmySettingService = gameArmySettingService;
        }

        /// <summary>
        /// Get settings metadata
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("metadata")]
        public async Task<IHttpActionResult> Metadata()
        {
            var troopTypes = _castleTroopTypeService.GetAllAsync();
            await Task.WhenAll(troopTypes);
            return Ok(new
            {
                TroopTypes = troopTypes.Result
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("troopTypes")]
        public async Task<IHttpActionResult> AddOrUpdateTroopType(AddTroopTypeModel model)
        {
            var troop = await _castleTroopTypeService.GetByTypeAsync(model.ResourceType);
            if (troop == null)
            {
                await _castleTroopTypeService.InsertAsync(Mapper.Map<CastleTroopTypeModel>(model));
            }
            else
            {
                var updatedTroop = Mapper.Map<CastleTroopTypeModel>(model);
                updatedTroop.Id = troop.Id;
                await _castleTroopTypeService.UpdateAsync(updatedTroop);
            }
            return Ok();
        }

        /// <summary>
        /// Get all armies
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("armies")]
        public async Task<IHttpActionResult> GetArmies()
        {
            return Ok(await _gameArmySettingService.GetAllAsync());
        }
        /// <summary>
        /// Add/Update new army
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("armies")]
        public Task<IHttpActionResult> AddOrUpdateArmy(GameArmySettingModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
                return AddArmy(model);
            return UpdateArmy(model);
        }
        /// <summary>
        /// Delete army
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("armies/{id}")]
        public async Task<IHttpActionResult> DeleteArmy(string id)
        {
            await _gameArmySettingService.RemoveAsync(id);
            return Ok();
        }

        private async Task<IHttpActionResult> UpdateArmy(GameArmySettingModel model)
        {
            var armyByName = await _gameArmySettingService.GetByNameAsync(model.Name);
            if (armyByName != null && armyByName.Id != model.Id)
                return BadRequest("Army name is already existed!");
            var army = await _gameArmySettingService.GetByIdAsync(model.Id);
            if (army == null)
                return NotFound();
            model.Castles.ForEach(castle =>
            {
                castle.Id = Guid.NewGuid().ToString();
            });
            await _gameArmySettingService.UpdateAsync(model);
            return Ok();
        }

        private async Task<IHttpActionResult> AddArmy(GameArmySettingModel model)
        {
            var isExisting = await _gameArmySettingService.IsArmyExistingAsync(model.Name);
            if (isExisting)
                return BadRequest("Army is already existed");
            model.Id = Guid.NewGuid().ToString();
            model.Castles.ForEach(castle =>
            {
                castle.Id = Guid.NewGuid().ToString();
            });
            await _gameArmySettingService.InsertAsync(model);
            return Ok(model);
        }

        /// <summary>
        /// Get all artifacts
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("artifacts/metadata")]
        public async Task<IHttpActionResult> GetArtifactMetaData()
        {
            return Ok(new
            {
                Prizes = Enum.GetNames(typeof(Prize))
            });
        }
        
        
    }
}